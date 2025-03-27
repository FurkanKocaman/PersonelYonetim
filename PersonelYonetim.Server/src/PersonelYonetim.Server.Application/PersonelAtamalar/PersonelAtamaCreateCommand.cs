using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.PersonelIzinler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Roller;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Domain.UnitOfWork;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Cryptography;
using TS.Result;

namespace PersonelYonetim.Server.Application.PersonelAtamalar;

public sealed record PersonelAtamaCreateCommand(
    Guid PersonelId,
    Guid SirketId,
    Guid? SubeId,
    Guid? DepartmanId,
    Guid? PozisyonId,
    Guid? YoneticiId,
    int RolTipiValue,
    Guid? CalismaTakvimId,
    int SozlesmeTuruValue,
    DateTimeOffset? SozlesmeBitisTarihi,
    DateTimeOffset? PozisyonBaslamaTarihi,
    Guid? IzinKuralId
    ) : IRequest<Result<string>>;

internal sealed class PersonelAtamaCreateCommandHandler(
    UserManager<AppUser> userManager,
    IPersonelRepository personelRepository,
    IUserRoleRepository userRoleRepository,
    RoleManager<AppRole> roleManager,
    IPersonelAtamaRepository personelAtamaRepository,
    IPersonelIzinRepository personelIzinRepository,
    IIzinTurIzinKuralRepository izinTurIzinKuralRepository,
    ICalismaTakvimRepository calismaTakvimRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<PersonelAtamaCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelAtamaCreateCommand request, CancellationToken cancellationToken)
    {
        //using (var transaction = unitOfWork.BeginTransaction())
        //{
        //    try
        //    {
        PersonelAtama personelAtamaEski = personelAtamaRepository.FirstOrDefault(p => p.PersonelId == request.PersonelId && p.IsActive == true);
        if (personelAtamaEski != null)
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

        var personelIzin = personelIzinRepository.GetAll()
                .FirstOrDefault(p => p.PersonelId == request.PersonelId && p.SirketId == request.SirketId);

        if (personelIzin == null)
        {
            var izinTurIzinKural = request.IzinKuralId == null
                ? await izinTurIzinKuralRepository.GetAll()
                    .Include(p => p.IzinTur)
                    .Include(p => p.IzinKural)
                    .Where(p => p.IzinKural.SirketId == request.SirketId)
                    .ToListAsync()
                : await izinTurIzinKuralRepository.GetAll()
                    .Include(p => p.IzinTur)
                    .Where(p => p.IzinKuralId == request.IzinKuralId)
                    .ToListAsync();

            if (izinTurIzinKural == null || !izinTurIzinKural.Any())
            {
                return Result<string>.Failure("İzin türü ve/veya izin kuralı bulunamadı");
            }

            var izinTur = izinTurIzinKural
                    .FirstOrDefault(p => p.IzinTur.Ad.Equals("Yillik Izin", StringComparison.OrdinalIgnoreCase));

            if (izinTur != null)
            {
                personelIzin = new PersonelIzin
                {
                    PersonelId = request.PersonelId,
                    SirketId = request.SirketId,
                    ToplamIzin = izinTur.IzinTur.LimitGunSayisi,
                    KullanilanIzin = 0
                };
                personelIzinRepository.Add(personelIzin);
                personelAtama.IzinKuralId = izinTur.IzinKuralId;
                personelAtamaRepository.Update(personelAtama);
                await unitOfWork.SaveChangesAsync();
            }
            else
            {
                return Result<string>.Failure("Yıllık izin türü bulunamadı");
            }
        }

        if(request.CalismaTakvimId == null)
        {
            var defaultTakvim = await calismaTakvimRepository.FirstOrDefaultAsync(p => p.SirketId == request.SirketId);
            if (defaultTakvim == null)
                return Result<string>.Failure("Default takvim bulunamadı");

            personelAtama.CalismaTakvimId = defaultTakvim.Id;
        }

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
        //await unitOfWork.CommitTransactionAsync(transaction);

        return Result<string>.Succeed("Personel atama oluşturuldu");
        //    }catch(Exception ex)
        //    {
        //        await unitOfWork.RollbackTransactionAsync(transaction);
        //        return Result<string>.Failure("Hata oluştu:"+ex.ToString());
        //    }
        //}

    }
}
