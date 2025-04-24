using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using PersonelYonetim.Server.Domain.ZamanYonetimler;
using TS.Result;

namespace PersonelYonetim.Server.Application.IzinTalepler;
public sealed record IzinTalepDegerlendirmeCommand(
    Guid Id,
    int DegerlendirmeDurum,
    string? Yorum
    ) : IRequest<Result<string>>;

internal sealed class IzinTalepDegerlendirmeCommandHandler(
    ICurrentUserService currentUserService,
    IIzinTalepRepository izinTalepRepository,
    ITalepDegerlendirmeRepository talepDegerlendirmeRepository,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    ICalismaCizelgeRepository calismaCizelgeRepository,
    IGunlukCalismaRepository gunlukCalismaRepository,
    IBildirimService bildirimService,
    IIzinPeriyotRepository izinPeriyotRepository,
    ICalismaTakvimRepository calismaTakvimRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<IzinTalepDegerlendirmeCommand, Result<string>>
{
    public async Task<Result<string>> Handle(IzinTalepDegerlendirmeCommand request, CancellationToken cancellationToken)
    {
        using(var transaction = unitOfWork.BeginTransaction())
        {
            try
            {

                Guid? userId = currentUserService.UserId;
                Guid? tenantId = currentUserService.TenantId;

                if (!userId.HasValue || !tenantId.HasValue)
                    return Result<string>.Failure("User bulunamamdı");

                var talepDegerlendirme = await talepDegerlendirmeRepository.Where(p => p.TalepId == request.Id && p.AtananOnayciPersonel!.UserId == userId).Include(p => p.AtananOnayciPersonel).FirstOrDefaultAsync();

                if (talepDegerlendirme is null)
                    return Result<string>.Failure("Talep bulunamamdı");

                var res = talepDegerlendirme.DurumuGuncelle(DegerlendirmeDurumEnum.FromValue(request.DegerlendirmeDurum), talepDegerlendirme.AtananOnayciPersonelId, request.Yorum, DateTimeOffset.Now);

                if (!res.IsSuccessful)
                    return Result<string>.Failure(res.ErrorMessages![0]);

                talepDegerlendirmeRepository.Update(talepDegerlendirme);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                var izinTalep = await izinTalepRepository.Where(p => p.Id == talepDegerlendirme.TalepId).Include(p => p.IzinTur).FirstOrDefaultAsync();
                if (izinTalep is null)
                    return Result<string>.Failure("izin Talep bulunammadı");

                var sonDegerlendirme = await talepDegerlendirmeRepository.Where(p => p.TalepId == request.Id && p.AtananOnayciPersonel!.UserId == userId).Include(p => p.AtananOnayciPersonel).OrderByDescending(p => p.AdimSirasi).FirstOrDefaultAsync();

                if (sonDegerlendirme is null)
                    return Result<string>.Failure("Son degerlendirme bulunamadı");

                if (sonDegerlendirme.DegerlendirmeDurumu != DegerlendirmeDurumEnum.Beklemede)
                {
                    Bildirim bildirim = new()
                    {
                        Baslik = $"İzin talebi {sonDegerlendirme.DegerlendirmeDurumu.Name}",
                        Aciklama = $"{talepDegerlendirme.AtananOnayciPersonel!.FullName} tarafından izin talebiniz {sonDegerlendirme.DegerlendirmeDurumu.Name}",
                        CreatedAt = DateTimeOffset.Now,
                        BildirimTipi = BildirimTipiEnum.Onay,
                        AliciTipi = AliciTipiEnum.Personel,
                        AliciId = izinTalep.PersonelId,
                        TenantId = tenantId.Value,
                    };

                    await bildirimService.KullaniciyaBildirimGonderAsync(bildirim, izinTalep.PersonelId);

                    if (sonDegerlendirme.DegerlendirmeDurumu == DegerlendirmeDurumEnum.Onaylandi)
                    {
                        var personelGorevlendirme = await personelGorevlendirmeRepository.Where(p => p.PersonelId == izinTalep.PersonelId && !p.IsDeleted && p.TenantId == tenantId).Include(p => p.CalismaTakvimi).ThenInclude(p => p!.CalismaGunler).FirstOrDefaultAsync(cancellationToken);

                        if (personelGorevlendirme is null)
                            return Result<string>.Failure("Çalışma takvimi bulunamamdı");

                        var cizelge = await calismaCizelgeRepository.Where(p => p.PersonelId == izinTalep.PersonelId && p.Yil == izinTalep.BaslangicTarihi.Year && p.Ay == izinTalep.BaslangicTarihi.Month).Include(p => p.GunlukCalismalar).FirstOrDefaultAsync();
                        if (cizelge is null)
                        {
                            CalismaCizelge calismaCizelge = new()
                            {
                                PersonelId = izinTalep.PersonelId,
                                Yil = izinTalep.BaslangicTarihi.Year,
                                Ay = izinTalep.BaslangicTarihi.Month,
                                TenantId = tenantId.Value,
                            };
                            cizelge = calismaCizelge;
                            calismaCizelgeRepository.Add(calismaCizelge);

                            await unitOfWork.SaveChangesAsync(cancellationToken);

                            var ilkGun = DateTimeOffset.Now;

                            DateTimeOffset firstDayOfThisMonth = new DateTimeOffset(ilkGun.Year, ilkGun.Month, 1, 0, 0, 0, ilkGun.Offset);

                            ilkGun = firstDayOfThisMonth;

                            if (personelGorevlendirme.IseGirisTarihi > firstDayOfThisMonth)
                            {
                                ilkGun = new DateTimeOffset(personelGorevlendirme.IseGirisTarihi.Year, personelGorevlendirme.IseGirisTarihi.Month, personelGorevlendirme.IseGirisTarihi.Day, 0, 0, 0, personelGorevlendirme.IseGirisTarihi.Offset);
                            }
                            DateTimeOffset sonGun = new DateTimeOffset(ilkGun.Year, ilkGun.Month, DateTime.DaysInMonth(ilkGun.Year, ilkGun.Month), 0, 0, 0, ilkGun.Offset);

                            while (ilkGun <= sonGun)
                            {
                                if (!await gunlukCalismaRepository.AnyAsync(p => p.Tarih == ilkGun))
                                {
                                    GunlukCalisma gunlukCalisma = new()
                                    {
                                        CalismaCizelgesiId = calismaCizelge.Id,
                                        Tarih = ilkGun,
                                        PersonelId = izinTalep.PersonelId,
                                        TenantId = tenantId.Value,
                                    };

                                    gunlukCalismaRepository.Add(gunlukCalisma);
                                }
                                ilkGun = ilkGun.AddDays(1);
                            }
                            await unitOfWork.SaveChangesAsync(cancellationToken);
                        }
                        var cizelgeNew = await calismaCizelgeRepository.Where(p => p.PersonelId == izinTalep.PersonelId && p.Yil == izinTalep.BaslangicTarihi.Year && p.Ay == izinTalep.BaslangicTarihi.Month).Include(p => p.GunlukCalismalar).FirstOrDefaultAsync();

                        var calismaTakvim = await calismaTakvimRepository.Where(p => p.Id == personelGorevlendirme.CalismaTakvimId).Include(p => p.CalismaGunler).FirstOrDefaultAsync();
                        var calismaGunler = calismaTakvim!.CalismaGunler;

                        var gunlukCalismalar = cizelgeNew!.GunlukCalismalar;

                        DateTimeOffset baslangic = izinTalep.BaslangicTarihi.ToLocalTime();
                        DateTimeOffset bitis = izinTalep.BitisTarihi.ToLocalTime();

                        while (baslangic.Date <= bitis.Date)
                        {
                            var calismaGun = calismaGunler.Where(p => p.Gun == baslangic.DayOfWeek).FirstOrDefault();
                            if (calismaGun!.IsCalismaGunu)
                            {
                                var gunlukCalisma = gunlukCalismalar.Where(p => p.Tarih == new DateTimeOffset(baslangic.Year, baslangic.Month, baslangic.Day, 0, 0, 0, baslangic.Offset)).FirstOrDefault();

                                if (baslangic.Date == bitis.Date)
                                {
                                    IzinPeriyodu izinPeriyodu = new()
                                    {
                                        GunlukCalismaId = gunlukCalisma!.Id,
                                        BaslangicSaati = new TimeOnly(baslangic.Hour, baslangic.Minute),
                                        BitisSaati = calismaGun.CalismaBitis <= new TimeOnly(bitis.Hour, bitis.Minute) ? calismaGun.CalismaBitis.Value : new TimeOnly(bitis.Hour, bitis.Minute),
                                        IzinTipi = izinTalep.IzinTur.Ad,
                                        TenantId = tenantId.Value,
                                    };
                                    izinPeriyotRepository.Add(izinPeriyodu);

                                }
                                else
                                {
                                    if (baslangic.Date == izinTalep.BaslangicTarihi.Date)
                                    {
                                        IzinPeriyodu izinPeriyodu = new()
                                        {
                                            GunlukCalismaId = gunlukCalisma!.Id,
                                            BaslangicSaati = new TimeOnly(baslangic.Hour, baslangic.Minute),
                                            BitisSaati = calismaGun.CalismaBitis!.Value,
                                            IzinTipi = izinTalep.IzinTur.Ad,
                                            TenantId = tenantId.Value,
                                        };
                                        izinPeriyotRepository.Add(izinPeriyodu);
                                    }
                                    else if (baslangic.Date == izinTalep.BitisTarihi.Date)
                                    {
                                        IzinPeriyodu izinPeriyodu = new()
                                        {
                                            GunlukCalismaId = gunlukCalisma!.Id,
                                            BaslangicSaati = calismaGun.CalismaBaslangic!.Value,
                                            BitisSaati = calismaGun.CalismaBitis <= new TimeOnly(bitis.Hour, bitis.Minute) ? calismaGun.CalismaBitis.Value : new TimeOnly(bitis.Hour, bitis.Minute),
                                            IzinTipi = izinTalep.IzinTur.Ad,
                                            TenantId = tenantId.Value,
                                        };
                                        izinPeriyotRepository.Add(izinPeriyodu);
                                    }
                                    else
                                    {
                                        IzinPeriyodu izinPeriyodu = new()
                                        {
                                            GunlukCalismaId = gunlukCalisma!.Id,
                                            BaslangicSaati = calismaGun.CalismaBaslangic!.Value,
                                            BitisSaati = calismaGun.CalismaBitis!.Value,
                                            IzinTipi = izinTalep.IzinTur.Ad,
                                            TenantId = tenantId.Value,
                                        };
                                        izinPeriyotRepository.Add(izinPeriyodu);
                                    }
                                }
                            }
                            baslangic = baslangic.AddDays(1);
                        }
                        await unitOfWork.SaveChangesAsync(cancellationToken);
                    }

                }
                await unitOfWork.CommitTransactionAsync(transaction);
                return Result<string>.Succeed("İzin talebi başarıyla değerlendirildi");

            }
            catch (Exception ex) { 
                await unitOfWork.RollbackTransactionAsync( transaction );
                return Result<string>.Failure("Hata oluştu: " + ex.Message );
            }


        }

       
    }
}
