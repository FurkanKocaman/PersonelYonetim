
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Roller;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Domain.UnitOfWork;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.Application.PersonelAtamalar;

public sealed record PersonelAtamaCreateCommand(
    Guid PersonelId,
    Guid SirketId,
    Guid? SubeId,
    Guid? DepartmanId,
    Guid? PozisyonId,
    int RolTipiValue,
    Guid? CalismaTakvimId,
    int SozlesmeTuruValue,
    DateTimeOffset? SozlesmeBitisTarihi
    ) : IRequest<Result<string>>;

internal sealed class PersonelAtamaCreateCommandHandler(
    UserManager<AppUser> userManager,
    IPersonelRepository personelRepository,
    IUserRoleRepository userRoleRepository,
    RoleManager<AppRole> roleManager,
    IPersonelAtamaRepository personelAtamaRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<PersonelAtamaCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelAtamaCreateCommand request, CancellationToken cancellationToken)
    {
        PersonelAtama personelAtamaEski = personelAtamaRepository.FirstOrDefault(p => p.PersonelId == request.PersonelId && p.IsActive == true);
        if(personelAtamaEski != null)
        {
            personelAtamaEski.IsActive = false;
            personelAtamaEski.SozlesmeBitisTarihi = DateTimeOffset.Now;
            personelAtamaRepository.Update(personelAtamaEski);
            await unitOfWork.SaveChangesAsync();
        }

        PersonelAtama personelAtama = request.Adapt<PersonelAtama>();
        personelAtama.PersonelId = request.PersonelId;
        personelAtama.IsActive = true;
        personelAtama.RolTipi = RolTipiEnum.FromValue(request.RolTipiValue);
        personelAtama.SozlesmeTuru = SozlesmeTuruEnum.FromValue(request.SozlesmeTuruValue);

        personelAtamaRepository.Add(personelAtama);
        await unitOfWork.SaveChangesAsync();

        var personel = await personelRepository.FirstOrDefaultAsync(p => p.Id == request.PersonelId);
        if (personel == null)
            return Result<string>.Failure("Personel bulunamadı");

        var user = await userManager.FindByIdAsync(personel.UserId.ToString());
        if (user == null)
            return Result<string>.Failure("User bulunamadı");

        var role = await roleManager.FindByNameAsync(request.RolTipiValue.ToString());
        if (role == null)
            return Result<string>.Failure("Rol bulunamadı");

        AppUserRole appUserRole = new()
        {
            UserId = user.Id,
            RoleId = role.Id,
            SirketId = request.SirketId,
        };
        userRoleRepository.Add(appUserRole);
        await unitOfWork.SaveChangesAsync();
       
        return Result<string>.Succeed("Personel atama oluşturuldu");
    }
}
