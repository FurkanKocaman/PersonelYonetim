using GenericRepository;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Roller;
using PersonelYonetim.Server.Domain.Rols;
using TS.Result;

namespace PersonelYonetim.Server.Application.PersonelAtamalar;

public sealed record PersonelAtamaUpdateCommand(
    Guid PersonelId,
    Guid SirketId,
    Guid? SubeId,
    Guid? DepartmanId,
    Guid? PozisyonId,
    int RolTipiValue) : IRequest<Result<string>>;

internal sealed class PersonelAtamaUpdateCommandHandler(
    IPersonelAtamaRepository personelAtamaRepository,
    IUserRoleRepository userRoleRepository,
    RoleManager<AppRole> roleManager,
    IUnitOfWork unitOfWork) : IRequestHandler<PersonelAtamaUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelAtamaUpdateCommand request, CancellationToken cancellationToken)
    {
        var personelAtamaOld = await personelAtamaRepository.FirstOrDefaultAsync(p => p.PersonelId == request.PersonelId && p.SirketId == request.SirketId && p.IsActive && !p.IsDeleted);
        if (personelAtamaOld == null)
            return Result<string>.Failure("Mevcut atama bulunamadı");

        personelAtamaOld.IsDeleted = true;

        PersonelAtama personelAtama = personelAtamaOld;

        if(request.SubeId is not null)
            personelAtama.SubeId = request.SubeId;

        if(request.DepartmanId is not null)
            personelAtama.DepartmanId = request.DepartmanId;

        if(request.PozisyonId is not null)
            personelAtama.PozisyonId = request.PozisyonId;

        personelAtama.RolTipi = RolTipiEnum.FromValue(request.RolTipiValue);

        var role = await roleManager.FindByNameAsync(request.RolTipiValue.ToString());
        if (role is null)
            return Result<string>.Failure("Rol bulunamadı");

        AppUserRole appUserRole = await userRoleRepository.FirstOrDefaultAsync(p => p.UserId == personelAtamaOld.Personel.UserId && p.SirketId == personelAtama.SirketId);
        if (appUserRole is null)
        {
            appUserRole = new()
            {
                UserId = personelAtamaOld.Personel.UserId,
                RoleId = role.Id,
                SirketId = personelAtama.SirketId,
            };

            userRoleRepository.Add(appUserRole);
        }
        else
        {
            appUserRole.RoleId = role.Id;
        }

        personelAtamaRepository.Update(personelAtamaOld);
        personelAtamaRepository.Add(personelAtama);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Personel atama güncellendi");
    }
}
