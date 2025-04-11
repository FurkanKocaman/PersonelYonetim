using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.PersonelIzinler;
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
    Guid? YoneticiId,
    int RolTipiValue,
    Guid? CalismaTakvimId,
    Guid? IzinKuralId,
    Guid? MesaiOnaySurecId,
    Guid? IzinOnaySurecId,
    int SozlesmeTuruValue,
    int CalismaSekliValue,
    DateTimeOffset? SozlesmeBitisTarihi,
    DateTimeOffset? PozisyonBaslamaTarihi
    ) : IRequest<Result<string>>;

internal sealed class PersonelAtamaCreateCommandHandler(
    UserManager<AppUser> userManager,
    IPersonelRepository personelRepository,
    IUserRoleRepository userRoleRepository,
    RoleManager<AppRole> roleManager,
    IPersonelAtamaRepository personelAtamaRepository,
    ICalismaTakvimRepository calismaTakvimRepository,
    IIzinKuralRepository izinKuralRepository,
    IPersonelIzinKuralRepository personelIzinKuralRepository,
    IOnaySurecRepository onaySurecRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<PersonelAtamaCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelAtamaCreateCommand request, CancellationToken cancellationToken)
    {
        //using(var transaction = unitOfWork.BeginTransaction())
        //{
        try
        {
            //PersonelAtama personelAtamaEski = personelAtamaRepository.FirstOrDefault(p => p.PersonelId == request.PersonelId && p.SirketId == request.SirketId && p.IsDeleted == false);
            //if (personelAtamaEski is not null)
            //{
            //    personelAtamaEski.IsActive = false;
            //    personelAtamaEski.IsDeleted = true;
            //    personelAtamaEski.SozlesmeBitisTarihi = DateTimeOffset.Now;
            //    personelAtamaRepository.Update(personelAtamaEski);
            //    await unitOfWork.SaveChangesAsync();
            //}

            PersonelAtama personelAtama = request.Adapt<PersonelAtama>();
            //personelAtama.PersonelId = request.PersonelId;
            //personelAtama.IsActive = true;
            personelAtama.RolTipi = RolTipiEnum.FromValue(request.RolTipiValue);
            personelAtama.SozlesmeTuru = SozlesmeTuruEnum.FromValue(request.SozlesmeTuruValue);



            //Personele izinKural atama
            //Request.izinKuralId null değilse
            if (request.IzinKuralId != null)
            {
                var personelIzinKural = await personelIzinKuralRepository.FirstOrDefaultAsync(p => p.PersonelId == request.PersonelId && p.SirketId == request.SirketId && p.IsDeleted == false && p.IsActive);
                if (personelIzinKural != null && personelIzinKural.IzinKuralId != request.IzinKuralId)
                {
                    personelIzinKural.IsActive = false;
                    personelIzinKuralRepository.Update(personelIzinKural);

                    PersonelIzinKural newPersonelIzinKural = new()
                    {
                        PersonelId = request.PersonelId,
                        IzinKuralId = request.IzinKuralId.Value,
                        SirketId = request.SirketId,
                    };
                    personelIzinKuralRepository.Add(newPersonelIzinKural);
                }
                else if (personelIzinKural == null)
                {
                    PersonelIzinKural newPersonelIzinKural = new()
                    {
                        PersonelId = request.PersonelId,
                        IzinKuralId = request.IzinKuralId.Value,
                        SirketId = request.SirketId,
                    };
                    personelIzinKuralRepository.Add(newPersonelIzinKural);
                }
            }
            //Request IzinKuralId null ise ve veritabanında personele ait izinKural yoksa o şirket için default olan izinKuralı, createUserId değeri boş olan kuralı bulup default olarak ata
            else
            {
                var personelIzinKural = await personelIzinKuralRepository.FirstOrDefaultAsync(p => p.PersonelId == request.PersonelId && p.SirketId == request.SirketId && p.IsDeleted == false && p.IsActive);

                if (personelIzinKural == null)
                {
                   
                    IzinKural izinKural = await izinKuralRepository.FirstOrDefaultAsync(p => p.SirketId == request.SirketId && p.CreateUserId == Guid.Empty);
                    PersonelIzinKural newPersonelIzinKural = new()
                    {
                        PersonelId = request.PersonelId,
                        IzinKuralId = izinKural.Id,
                        SirketId = request.SirketId,
                        OnaySurecId = request.IzinOnaySurecId
                    };

                    if(request.IzinOnaySurecId == null)
                    {
                        var defaultIzinOnaySurec = await onaySurecRepository.FirstOrDefaultAsync(p => p.SirketId == request.SirketId && p.OnaySurecTuruEnum == OnaySurecTuruEnum.Izin && p.CreateUserId == Guid.Empty);
                        if (defaultIzinOnaySurec == null)
                            return Result<string>.Failure("Default Onay surec bulunamadı");

                        newPersonelIzinKural.OnaySurecId = defaultIzinOnaySurec.Id;

                    }
                    personelIzinKuralRepository.Add(newPersonelIzinKural);
                }
            }

            if (request.CalismaTakvimId == null)
            {
                //if (personelAtamaEski is not null && personelAtamaEski.CalismaTakvimId is not null)
                //{
                //    personelAtama.CalismaTakvimId = personelAtamaEski.CalismaTakvimId;
                //}
                //else
                //{
                var defaultTakvim = await calismaTakvimRepository.FirstOrDefaultAsync(p => p.SirketId == request.SirketId && p.CreateUserId == Guid.Empty);
                if (defaultTakvim == null)
                    return Result<string>.Failure("Default takvim bulunamadı");

                personelAtama.CalismaTakvimId = defaultTakvim.Id;
                //}
            }
            if (request.MesaiOnaySurecId == null)
            {
                var defaultMesaiOnaySurec = await onaySurecRepository.FirstOrDefaultAsync(p => p.SirketId == request.SirketId && p.OnaySurecTuruEnum == OnaySurecTuruEnum.Mesai && p.CreateUserId == Guid.Empty);
                if (defaultMesaiOnaySurec == null)
                    return Result<string>.Failure("Default Onay surec bulunamadı");

                personelAtama.MesaiOnaySurecId = defaultMesaiOnaySurec.Id;
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

            var appUserRoleInDb = await userRoleRepository.FirstOrDefaultAsync(p => p.UserId == user.Id && p.SirketId == personelAtama.SirketId);

            if (appUserRoleInDb is not null)
            {
                userRoleRepository.Delete(appUserRoleInDb);
                await unitOfWork.SaveChangesAsync();
            }

            AppUserRole appUserRole = new()
            {
                UserId = user.Id,
                RoleId = role.Id,
                SirketId = request.SirketId,
            };
            personelAtamaRepository.Add(personelAtama);
            userRoleRepository.Add(appUserRole);

                await unitOfWork.SaveChangesAsync();
                //await unitOfWork.CommitTransactionAsync(transaction);

                return Result<string>.Succeed("Personel atama oluşturuldu");
    }
            catch(Exception ex)
            {
                //await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure("Hata oluştu : "+ex);
            }
        //}
       
    }
}
