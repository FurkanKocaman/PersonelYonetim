using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using PersonelYonetim.Server.Domain.ZamanYonetimler;
using TS.Result;

namespace PersonelYonetim.Server.Application.CalismaCizelgeleri;
public sealed record GunlukCalismaPeriyotCreateCommand(
    Guid PersonelId,
    Guid? GunlukCalismaId,
    CalismaPeriyoduTipi CalismaPeriyoduTipi,
    TimeOnly BaslangicSaati,
    TimeOnly BitisSaati) : IRequest<Result<string>>;

internal sealed class GunlukCalismaPeriyotCreateCommandHandler(
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    IGunlukCalismaRepository gunlukCalismaRepository,
    ICalismaPeriyotRepository calismaPeriyotRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<GunlukCalismaPeriyotCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(GunlukCalismaPeriyotCreateCommand request, CancellationToken cancellationToken)
    {
        var personelAtama = personelGorevlendirmeRepository.Where(p => p.PersonelId == request.PersonelId && p.IsActive & !p.IsDeleted).Include(p => p.CalismaTakvimi).ThenInclude(p => p!.CalismaGunler).FirstOrDefault();

        if (personelAtama is null)
            return Result<string>.Failure("Personel ataması bulunamadı.");

        Guid GunlukCalismaId = Guid.Empty;

        if (request.GunlukCalismaId is not null)
            GunlukCalismaId = request.GunlukCalismaId.Value;
        else
        {
            var bugun = DateTimeOffset.Now.Date;
            GunlukCalismaId = gunlukCalismaRepository.Where(p => p.PersonelId == personelAtama.PersonelId && p.Tarih == bugun).FirstOrDefault()!.Id;
        }
          

        if(request.CalismaPeriyoduTipi == CalismaPeriyoduTipi.FazlaMesai)
        {
            var gunlukCalisma = gunlukCalismaRepository.Where(g => g.Id == GunlukCalismaId).Include(g => g.CalismaPeriyotlari).Include(g => g.IzinPeriyotlari).FirstOrDefault();
            if (gunlukCalisma is null)
                return Result<string>.Failure("Gunluk calisma bulunamadi");

            if (personelAtama.CalismaTakvimi is null)
                return Result<string>.Failure("Personelin çalışma takvimi bulunamadı");

            var calismaGun = personelAtama.CalismaTakvimi.CalismaGunler.Where(p => p.Gun == gunlukCalisma.Tarih.DayOfWeek).FirstOrDefault();

            if (gunlukCalisma.CalismaPeriyotlari.Where(p => p.CalismaPeriyoduTipi == CalismaPeriyoduTipi.FazlaMesai).Any(p => (p.BaslangicSaati <= request.BaslangicSaati && p.BitisSaati >= request.BitisSaati) || (request.BaslangicSaati <= p.BaslangicSaati && request.BitisSaati > p.BaslangicSaati) || (request.BaslangicSaati < p.BitisSaati && request.BitisSaati >= p.BitisSaati)))
                return Result<string>.Failure("Fazla mesai periyodu mesai saatleri içinde olamaz");

            var isIzinPeriyotExist = gunlukCalisma.IzinPeriyotlari.Any(p => (p.BaslangicSaati >= request.BaslangicSaati && p.BitisSaati >= request.BitisSaati) || (request.BaslangicSaati < p.BaslangicSaati && request.BitisSaati <= p.BaslangicSaati) || (request.BitisSaati < p.BitisSaati && request.BaslangicSaati >= p.BaslangicSaati) || (request.BaslangicSaati <= p.BaslangicSaati && request.BitisSaati >= p.BitisSaati));
            if (isIzinPeriyotExist)
                return Result<string>.Failure("Bu saat aralığında zaten bir izin periyodu mevcut");
        }

        if(request.CalismaPeriyoduTipi == CalismaPeriyoduTipi.Normal)
        {
            var gunlukCalisma = gunlukCalismaRepository.Where(g => g.Id == GunlukCalismaId).Include(g => g.CalismaPeriyotlari).FirstOrDefault();
            if (gunlukCalisma is null)
                return Result<string>.Failure("Gunluk calisma bulunamadi");

            var isCalismaPeriyotExist = gunlukCalisma.CalismaPeriyotlari.Where(p => p.CalismaPeriyoduTipi == CalismaPeriyoduTipi.Normal).Any(p => (p.BaslangicSaati == request.BaslangicSaati && p.BitisSaati == request.BitisSaati) || (request.BaslangicSaati < p.BitisSaati && request.BaslangicSaati > p.BaslangicSaati) || (request.BitisSaati < p.BitisSaati && request.BitisSaati > p.BaslangicSaati) || (request.BaslangicSaati < p.BaslangicSaati && request.BitisSaati > p.BitisSaati) );
            if(isCalismaPeriyotExist)
                return Result<string>.Failure("Bu saat aralığında zaten bir çalışma periyodu mevcut");

            var isIzinPeriyotExist = gunlukCalisma.IzinPeriyotlari.Any(p => (p.BaslangicSaati >= request.BaslangicSaati && p.BitisSaati >= request.BitisSaati) || (request.BaslangicSaati < p.BaslangicSaati && request.BitisSaati <= p.BaslangicSaati) || (request.BitisSaati < p.BitisSaati && request.BaslangicSaati >= p.BaslangicSaati) || (request.BaslangicSaati <= p.BaslangicSaati && request.BitisSaati >= p.BitisSaati));
            if (isIzinPeriyotExist)
                return Result<string>.Failure("Bu saat aralığında zaten bir izin periyodu mevcut");
        }

        CalismaPeriyodu calismaPeriyodu = new()
        {
            GunlukCalismaId = GunlukCalismaId,
            BaslangicSaati = request.BaslangicSaati,
            BitisSaati = request.BitisSaati,
            CalismaPeriyoduTipi = request.CalismaPeriyoduTipi
        };
        if(calismaPeriyodu.BitisSaati > calismaPeriyodu.BaslangicSaati)
            calismaPeriyodu.ToplamCalismaSuresi = calismaPeriyodu.BitisSaati - calismaPeriyodu.BaslangicSaati;
        else
            calismaPeriyodu.ToplamCalismaSuresi = calismaPeriyodu.BaslangicSaati - calismaPeriyodu.BitisSaati;

        calismaPeriyotRepository.Add(calismaPeriyodu);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Çalışma periyodu oluşturuldu");
    }
}
