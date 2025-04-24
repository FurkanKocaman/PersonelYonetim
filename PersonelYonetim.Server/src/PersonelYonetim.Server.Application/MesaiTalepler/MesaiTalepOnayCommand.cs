using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.Mesailer;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.UnitOfWork;
using PersonelYonetim.Server.Domain.ZamanYonetimler;
using TS.Result;


namespace PersonelYonetim.Server.Application.MesaiTalepler;
public sealed record MesaiTalepOnayCommand(
   Guid Id,
    int DegerlendirmeDurum,
    string? Yorum
    ) : IRequest<Result<string>>;

internal sealed class MesaiTalepOnayCommandHandler(
    ICalismaCizelgeRepository calismaCizelgeRepository,
    ICalismaTakvimRepository calismaTakvimRepository,
    IFazlaMesaiPeriyotRepository fazlaMesaiPeriyotRepository,
    IGunlukCalismaRepository gunlukCalismaRepository,
    IMesaiTalepRepository mesaiTalepRepository,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    ICurrentUserService currentUserService,
    ITalepDegerlendirmeRepository talepDegerlendirmeRepository,
    IBildirimService bildirimService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<MesaiTalepOnayCommand, Result<string>>
{
    public async Task<Result<string>> Handle(MesaiTalepOnayCommand request, CancellationToken cancellationToken)
    {
        using (var transaction = unitOfWork.BeginTransaction())
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

                var mesaiTalep = await mesaiTalepRepository.Where(p => p.Id == talepDegerlendirme.TalepId).FirstOrDefaultAsync(cancellationToken);
                if (mesaiTalep is null)
                    return Result<string>.Failure("izin Talep bulunammadı");

                var sonDegerlendirme = await talepDegerlendirmeRepository.Where(p => p.TalepId == request.Id && p.AtananOnayciPersonel!.UserId == userId).Include(p => p.AtananOnayciPersonel).OrderByDescending(p => p.AdimSirasi).FirstOrDefaultAsync();

                if (sonDegerlendirme is null)
                    return Result<string>.Failure("Son degerlendirme bulunamadı");

                if (sonDegerlendirme.DegerlendirmeDurumu != DegerlendirmeDurumEnum.Beklemede)
                {
                    Bildirim bildirim = new()
                    {
                        Baslik = $"Mesai talebi {sonDegerlendirme.DegerlendirmeDurumu.Name}",
                        Aciklama = $"{talepDegerlendirme.AtananOnayciPersonel!.FullName} tarafından mesai talebiniz {sonDegerlendirme.DegerlendirmeDurumu.Name}",
                        CreatedAt = DateTimeOffset.Now,
                        BildirimTipi = BildirimTipiEnum.Onay,
                        AliciTipi = AliciTipiEnum.Personel,
                        AliciId = mesaiTalep.PersonelId,
                        TenantId = tenantId.Value,
                    };

                    await bildirimService.KullaniciyaBildirimGonderAsync(bildirim, mesaiTalep.PersonelId);

                    if (sonDegerlendirme.DegerlendirmeDurumu == DegerlendirmeDurumEnum.Onaylandi)
                    {
                        var personelGorevlendirme = await personelGorevlendirmeRepository.Where(p => p.PersonelId == mesaiTalep.PersonelId && !p.IsDeleted && p.TenantId == tenantId).Include(p => p.CalismaTakvimi).ThenInclude(p => p!.CalismaGunler).FirstOrDefaultAsync(cancellationToken);

                        if (personelGorevlendirme is null)
                            return Result<string>.Failure("Çalışma takvimi bulunamamdı");

                        var cizelge = await calismaCizelgeRepository.Where(p => p.PersonelId == mesaiTalep.PersonelId && p.Yil == mesaiTalep.BaslangicTarihi.Year && p.Ay == mesaiTalep.BaslangicTarihi.Month).Include(p => p.GunlukCalismalar).FirstOrDefaultAsync();
                        if (cizelge is null)
                        {
                            CalismaCizelge calismaCizelge = new()
                            {
                                PersonelId = mesaiTalep.PersonelId,
                                Yil = mesaiTalep.BaslangicTarihi.Year,
                                Ay = mesaiTalep.BaslangicTarihi.Month,
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
                                        PersonelId = mesaiTalep.PersonelId,
                                        TenantId = tenantId.Value,
                                    };

                                    gunlukCalismaRepository.Add(gunlukCalisma);
                                }
                                ilkGun = ilkGun.AddDays(1);
                            }
                            await unitOfWork.SaveChangesAsync(cancellationToken);
                        }
                        var cizelgeNew = await calismaCizelgeRepository.Where(p => p.PersonelId == mesaiTalep.PersonelId && p.Yil == mesaiTalep.BaslangicTarihi.Year && p.Ay == mesaiTalep.BaslangicTarihi.Month).Include(p => p.GunlukCalismalar).FirstOrDefaultAsync();

                        var calismaTakvim = await calismaTakvimRepository.Where(p => p.Id == personelGorevlendirme.CalismaTakvimId).Include(p => p.CalismaGunler).FirstOrDefaultAsync();
                        var calismaGunler = calismaTakvim!.CalismaGunler;

                        var gunlukCalismalar = cizelgeNew!.GunlukCalismalar;

                        DateTimeOffset baslangic = mesaiTalep.BaslangicTarihi.ToLocalTime();
                        DateTimeOffset bitis = mesaiTalep.BitisTarihi.ToLocalTime();

                        while (baslangic < bitis)
                        {
                            var bugun = baslangic.Date;
                            var sonrakiGun = bugun.AddDays(1);

                            var gunlukCalisma = gunlukCalismalar.FirstOrDefault(p => p.Tarih == bugun);

                            if (gunlukCalisma != null)
                            {
                                if (baslangic.Date == bitis.Date)
                                {
                                    FazlaMesaiPeriyodu fazlaMesaiPeriyodu = new()
                                    {
                                        GunlukCalismaId = gunlukCalisma.Id,
                                        BaslangicSaati = new TimeOnly(baslangic.Hour, baslangic.Minute),
                                        BitisSaati = new TimeOnly(bitis.Hour, bitis.Minute),
                                        ToplamFazlaMesaiSuresi = bitis - baslangic,
                                        TenantId = tenantId.Value
                                    };
                                    fazlaMesaiPeriyotRepository.Add(fazlaMesaiPeriyodu);
                                }
                                else
                                {
                                    FazlaMesaiPeriyodu fazlaMesaiPeriyoduIlkGun = new()
                                    {
                                        GunlukCalismaId = gunlukCalisma.Id,
                                        BaslangicSaati = new TimeOnly(baslangic.Hour, baslangic.Minute),
                                        BitisSaati = new TimeOnly(23, 59),
                                        ToplamFazlaMesaiSuresi = new DateTimeOffset(bugun.AddDays(1).AddTicks(-1)) - baslangic,
                                        TenantId = tenantId.Value
                                    };
                                    fazlaMesaiPeriyotRepository.Add(fazlaMesaiPeriyoduIlkGun);

                                    FazlaMesaiPeriyodu fazlaMesaiPeriyoduIkinciGun = new()
                                    {
                                        GunlukCalismaId = gunlukCalisma.Id,
                                        BaslangicSaati = new TimeOnly(0, 0), 
                                        BitisSaati = new TimeOnly(bitis.Hour, bitis.Minute),
                                        ToplamFazlaMesaiSuresi = bitis - new DateTimeOffset(bugun.AddDays(1)),
                                        TenantId = tenantId.Value
                                    };
                                    fazlaMesaiPeriyotRepository.Add(fazlaMesaiPeriyoduIkinciGun);
                                }
                            }

                            baslangic = sonrakiGun; 
                        }

                        await unitOfWork.SaveChangesAsync(cancellationToken);
                    }

                }
                await unitOfWork.CommitTransactionAsync(transaction);
                return Result<string>.Succeed("İzin talebi başarıyla değerlendirildi");

            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure("Hata oluştu: " + ex.Message);
            }


        }


    }
}

