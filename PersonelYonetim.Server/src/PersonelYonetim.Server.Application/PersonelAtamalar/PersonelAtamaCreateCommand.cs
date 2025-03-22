using GenericRepository;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Roller;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.Application.PersonelAtamalar;

public sealed record PersonelAtamaCreateCommand(
    Personel RequestPersonel,
    Guid SirketId,
    Guid? SubeId,
    Guid? DepartmanId,
    Guid? PozisyonId,
    int RolTipiValue,
    int CalismaSekliValue,
    int SozlesmeTuruValue,
    DateTimeOffset? SozlesmeBitisTarihi
    ) : IRequest<Result<string>>;

internal sealed class PersonelAtamaCreateCommandHandler(
    IUserRoleRepository userRoleRepository,
    RoleManager<AppRole> roleManager,
    IPersonelAtamaRepository personelAtamaRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<PersonelAtamaCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelAtamaCreateCommand request, CancellationToken cancellationToken)
    {
        PersonelAtama personelAtama = request.Adapt<PersonelAtama>();
        personelAtama.PersonelId = request.RequestPersonel.Id;
        personelAtama.RolTipi = RolTipiEnum.FromValue(request.RolTipiValue);
        personelAtama.CalismaSekli = CalismaSekliEnum.FromValue(request.CalismaSekliValue);
        personelAtama.SozlesmeTuru = SozlesmeTuruEnum.FromValue(request.SozlesmeTuruValue);

        personelAtamaRepository.Add(personelAtama);
        await unitOfWork.SaveChangesAsync();

        if (request.RolTipiValue == 6 || request.RolTipiValue == 5)
        {
            var role = await roleManager.FindByNameAsync("SirketSahibi");
            if (role == null)
                return Result<string>.Failure("Rol bulunamadı");
            AppUserRole appUserRole = new()
            {
                UserId = request.RequestPersonel.Id,
                RoleId = role!.Id,
                SirketId = personelAtama.SirketId,
            };
            userRoleRepository.Add(appUserRole);
            await unitOfWork.SaveChangesAsync();
        }
        if(request.RolTipiValue >=1 && request.RolTipiValue <=4)
        {
            var role = await roleManager.FindByNameAsync("Yonetici");
            if (role == null)
                return Result<string>.Failure("Rol bulunamadı");
            AppUserRole appUserRole = new()
            {
                UserId = request.RequestPersonel.Id,
                RoleId = role!.Id,
                SirketId = personelAtama.SirketId,
            };
            userRoleRepository.Add(appUserRole);
            await unitOfWork.SaveChangesAsync();
        }

        if (request.RolTipiValue == 0)
        {
            var role = await roleManager.FindByNameAsync("Calisan");
            if (role == null)
                return Result<string>.Failure("Rol bulunamadı");
            AppUserRole appUserRole = new()
            {
                UserId = request.RequestPersonel.Id,
                RoleId = role!.Id,
                SirketId = personelAtama.SirketId,
            };
            userRoleRepository.Add(appUserRole);
            await unitOfWork.SaveChangesAsync();
        }
       
            return Result<string>.Succeed("Personel atama oluşturuldu");
    }
}
