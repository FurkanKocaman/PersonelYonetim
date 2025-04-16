using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.Izinler;
using TS.Result;
using static PersonelYonetim.Server.Application.Services.IIzinHesaplamaService;

namespace PersonelYonetim.Server.Infrastructure.Services;
internal sealed class IzinHesaplamaService(
    ICalismaGunRepository calismaGunRepository,
    IMemoryCache memoryCache) : IIzinHesaplamaService
{
    public async Task<Result<IzinHesaplamaResult>> HesaplaIzinSuresiAsync(Guid calismaTakvimId, DateTimeOffset baslangic, DateTimeOffset bitis, IzinTur izinTur, CancellationToken cancellationToken)
    {
        var cacheKey = $"calismaGunler_{calismaTakvimId}";

        if(!memoryCache.TryGetValue(cacheKey, out List<CalismaGun>? calismaGunler))
        {
            calismaGunler = await calismaGunRepository.Where(p => p.CalismaTakvimId == calismaTakvimId).ToListAsync(cancellationToken);

            memoryCache.Set(cacheKey, calismaGunler, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                SlidingExpiration = TimeSpan.FromMinutes(30),
            });
        }
        if(calismaGunler is null || !calismaGunler.Any())
            return Result<IzinHesaplamaResult>.Failure("Personel için çalışma takvimi günleri bulunamadı.");

        decimal toplamGun = 0;
        DateTimeOffset currentDay = baslangic;

        while(currentDay.Date <= bitis.Date)
        {
           var calismaGun = calismaGunler.FirstOrDefault(g => g.Gun == currentDay.DayOfWeek);

            if(calismaGun is not null && calismaGun.IsCalismaGunu)
            {
                //Mola süresi sonradan çıkarılabilir
                var toplamMesai = (decimal)(new TimeOnly(calismaGun.CalismaBaslangic!.Value.Hour, calismaGun.CalismaBaslangic.Value.Minute).ToTimeSpan()).Hours;

                if (baslangic.Date == bitis.Date)
                {
                    var start = new TimeOnly(baslangic.TimeOfDay.Hours, baslangic.TimeOfDay.Minutes) <= calismaGun.CalismaBaslangic ? calismaGun.CalismaBaslangic : new TimeOnly(baslangic.TimeOfDay.Hours, baslangic.TimeOfDay.Minutes);
                    var end = new TimeOnly(bitis.TimeOfDay.Hours, bitis.TimeOfDay.Minutes) >= calismaGun.CalismaBitis ? calismaGun.CalismaBitis : new TimeOnly(bitis.TimeOfDay.Hours, bitis.TimeOfDay.Minutes);

                    toplamGun += (decimal)(end - start).Value.Hours / toplamMesai ;
                }
                else
                {
                    if(currentDay.Date == baslangic.Date)
                    {
                        var start = new TimeOnly(currentDay.TimeOfDay.Hours, currentDay.TimeOfDay.Minutes) <= calismaGun.CalismaBaslangic ? calismaGun.CalismaBaslangic : new TimeOnly(currentDay.TimeOfDay.Hours, currentDay.TimeOfDay.Minutes);
                        toplamGun += (decimal)(calismaGun.CalismaBitis! - start).Value.Hours / toplamMesai;
                    }
                    else if(currentDay.Date == bitis.Date)
                    {
                        var end = new TimeOnly(bitis.TimeOfDay.Hours, bitis.TimeOfDay.Minutes) >= calismaGun.CalismaBitis ? calismaGun.CalismaBitis : new TimeOnly(bitis.TimeOfDay.Hours, bitis.TimeOfDay.Minutes);
                        toplamGun += (decimal)(end - calismaGun.CalismaBaslangic).Value.Hours / toplamMesai;
                    }
                    else
                    {
                        toplamGun += 1;
                    }

                }
            }
            currentDay = currentDay.AddDays(1);
        }

        DateTimeOffset mesaiBaslangicTarihi = bitis;

        var calismagun = calismaGunler.FirstOrDefault(g => g.Gun == mesaiBaslangicTarihi.DayOfWeek);

        if(calismagun!=null && calismagun.IsCalismaGunu && mesaiBaslangicTarihi.TimeOfDay < new TimeSpan(calismagun.CalismaBitis!.Value.Hour, calismagun.CalismaBitis.Value.Minute, calismagun.CalismaBitis.Value.Second))
        {
            return Result<IzinHesaplamaResult>.Succeed(new IzinHesaplamaResult(toplamGun, mesaiBaslangicTarihi));
        }
        mesaiBaslangicTarihi = mesaiBaslangicTarihi.AddDays(1);
        while (true)
        {
            var calismaGun = calismaGunler.FirstOrDefault(g => g.Gun == mesaiBaslangicTarihi.DayOfWeek);
            if(calismaGun is not null && calismaGun.IsCalismaGunu)
            {
                // TODO: Add public holiday check
                TimeOnly startTime = calismaGun.CalismaBaslangic ?? new TimeOnly(9, 0);
                mesaiBaslangicTarihi = new DateTimeOffset(mesaiBaslangicTarihi.Year, mesaiBaslangicTarihi.Month, mesaiBaslangicTarihi.Day, startTime.Hour,startTime.Minute,startTime.Second, mesaiBaslangicTarihi.Offset);
                break;
            }
            mesaiBaslangicTarihi = mesaiBaslangicTarihi.AddDays(1);
            if (mesaiBaslangicTarihi > bitis.AddDays(30))
            {
                return Result<IzinHesaplamaResult>.Failure("Mesai başlangıç tarihi belirlenemedi (takvim hatası?).");
            }
        }

        var result = new IzinHesaplamaResult(toplamGun, mesaiBaslangicTarihi);
        return Result<IzinHesaplamaResult>.Succeed(result);

    }
}
