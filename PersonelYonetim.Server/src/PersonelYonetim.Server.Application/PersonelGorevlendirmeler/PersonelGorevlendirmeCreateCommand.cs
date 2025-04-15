
using Mapster;
using MediatR;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.PersonelGorevlendirmeler;
public sealed record PersonelGorevlendirmeCreateCommand(
    Guid PersonelId,
    Guid KurumsalBirimId,
    Guid PozisyonId,
    Guid RoleId,
    DateTimeOffset BaslangicTarihi,
    DateTimeOffset? BitisTarihi,
    bool BirincilGorevMi,
    int GorevlendirmeTipiValue,
    int CalismaSekliValue,
    Guid? RaporlananGorevlendirmeId,
    Guid? IzinKuralId,
    Guid? CalismaTakvimId,
    decimal BrutUcret,
    Guid TenantId
    ) : IRequest<Result<string>>;

internal sealed class PersonelGorevlendirmeCreateCommandHandler(
    IKurumsalBirimRepository kurumsalBirimRepository,
    IPersonelRepository personelRepository,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    IGorevlendirmeRoluRepository gorevlendirmeRoluRepository,
    IGorevlendirmeIzinKuraliRepository gorevlendirmeIzinKuraliRepository,
    IIzinKuralRepository izinKuralRepository,
    ICalismaTakvimRepository calismaTakvimRepository,
    IOnaySurecRepository onaySurecRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<PersonelGorevlendirmeCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelGorevlendirmeCreateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var isPersonelExist = await personelRepository.AnyAsync(p => p.Id == request.PersonelId && p.TenantId == request.TenantId, cancellationToken);
            if(!isPersonelExist)
            {
                return Result<string>.Failure("Personel bulunamadı");
            }

            var isKurumsalBirimExist = await kurumsalBirimRepository.AnyAsync(p => p.Id == request.KurumsalBirimId && p.TenantId == request.TenantId, cancellationToken);
            if (!isKurumsalBirimExist)
            {
                return Result<string>.Failure("Kurumsal birim bulunamadı");
            }

            Guid IzinkuralId = Guid.Empty;
            Guid CalismaTakvimId = Guid.Empty;
            Guid IzinOnaySurecId = Guid.Empty;
            Guid MesaiOnaySurecId = Guid.Empty;

            var onaySurecleri = onaySurecRepository.Where(p => p.CreateUserId == Guid.Empty && p.TenantId == request.TenantId && p.IsDeleted == false).ToList();

            MesaiOnaySurecId = onaySurecleri.FirstOrDefault(p => p.OnaySurecTuruEnum == OnaySurecTuruEnum.Mesai)!.Id;
            IzinOnaySurecId = onaySurecleri.FirstOrDefault(p => p.OnaySurecTuruEnum == OnaySurecTuruEnum.Izin)!.Id;

            if (onaySurecleri.Count == 0)
                return Result<string>.Failure("Onay süreci bulunamadı");

            if(request.CalismaTakvimId is null)
            {
                var defaultTakvim = await calismaTakvimRepository.FirstOrDefaultAsync(p => p.TenantId == request.TenantId && p.CreateUserId == Guid.Empty && p.IsDeleted == false && p.IsActive);
                if (defaultTakvim == null)
                    return Result<string>.Failure("Default takvim bulunamadı");
                CalismaTakvimId = defaultTakvim.Id;
            }
            else
                CalismaTakvimId = request.CalismaTakvimId.Value;

            PersonelGorevlendirme personelGorevlendirme = request.Adapt<PersonelGorevlendirme>();

            personelGorevlendirme.GorevlendirmeTipi = GorevlendirmeTipiEnum.FromValue(request.GorevlendirmeTipiValue);
            personelGorevlendirme.CalismaSekli = CalismaSekliEnum.FromValue(request.CalismaSekliValue);
            personelGorevlendirme.MesaiOnaySurecId = MesaiOnaySurecId;
            personelGorevlendirme.CalismaTakvimId = CalismaTakvimId;

            personelGorevlendirmeRepository.Add(personelGorevlendirme);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            GorevlendirmeRolu gorevlendirmeRolu = new()
            {
                PersonelGorevlendirmeId = personelGorevlendirme.Id,
                RolId = request.RoleId
            };

            gorevlendirmeRoluRepository.Add(gorevlendirmeRolu);

            if(request.IzinKuralId is null)
            {
                var varsayilanIzinKural = izinKuralRepository.Where(p => p.TenantId == request.TenantId && p.CreateUserId == Guid.Empty && p.IsDeleted == false && p.IsActive).Select(p => new {p.Id}).FirstOrDefault();
                if(varsayilanIzinKural is null)
                    return Result<string>.Failure("Varsayılan izin kuralı bulunamadı");

                IzinkuralId = varsayilanIzinKural.Id;
            }
            else
            {
                IzinkuralId = request.IzinKuralId.Value;
            }

            GorevlendirmeIzinKurali gorevlendirmeIzinKurali = new()
            {
                PersonelGorevlendirmeId = personelGorevlendirme.Id,
                IzinKuralId = IzinkuralId,
                OzelOnaySurecId = IzinOnaySurecId,
            };

            gorevlendirmeIzinKuraliRepository.Add(gorevlendirmeIzinKurali);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Personel görevlendirme oluşturuldu");


        } catch (Exception ex)
        {
            return Result<string>.Failure(ex.Message);
        }
    }
}