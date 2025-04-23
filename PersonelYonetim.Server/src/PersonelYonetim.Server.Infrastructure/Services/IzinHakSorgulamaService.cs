using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using TS.Result;

namespace PersonelYonetim.Server.Infrastructure.Services;
internal sealed class IzinHakSorgulamaService(
    IGorevlendirmeIzinKuraliRepository gorevlendirmeIzinKuraliRepository,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    IIzinTalepRepository izinTalepRepository
    ) : IIzinHakSorgulamaService
{
    public async Task<Result<bool>> TalepHakkiKontrolEtAsync(Guid personelGorevlendirmeId, Guid izinTurId, decimal talepEdilenGun, DateTimeOffset talepBaslangicTarihi, CancellationToken cancellationToken)
    {
        var personelGorevlendirme = await personelGorevlendirmeRepository.FirstOrDefaultAsync(p => p.Id == personelGorevlendirmeId);
        if(personelGorevlendirme is null)
            return Result<bool>.Failure("Personel görevlendirme bulunamadı.");

        var gorevlendirmeIzinKurali = await gorevlendirmeIzinKuraliRepository.Where(p => p.PersonelGorevlendirmeId == personelGorevlendirme.Id && p.IsActive && !p.IsDeleted)
                                        .Include(p => p.IzinKural).ThenInclude(p => p.IzinTurler).FirstOrDefaultAsync();

        if (gorevlendirmeIzinKurali is null || gorevlendirmeIzinKurali.IzinKural is null || !gorevlendirmeIzinKurali.IzinKural.IzinTurler.Any())
            return Result<bool>.Failure("Personel için izin kuralı bulunamadı.");

        var izinTur = gorevlendirmeIzinKurali.IzinKural.IzinTurler.FirstOrDefault(p => p.Id == izinTurId);
        if(izinTur is null)
            return Result<bool>.Failure("İzin türü bulunamadı.");

        if (izinTur.LimitTipi == LimitTipiEnum.Limitsiz || izinTur.EksiBakiyeHakkı == EksiBakiyeHakkıEnum.Limitsiz)
            return Result<bool>.Succeed(true);

        var izinTaleplerToplamGun = await izinTalepRepository.Where(p => p.IzinTurId == izinTur.Id).SumAsync(p => p.ToplamSure, cancellationToken);

        var kidemYil = (DateTimeOffset.Now.Year - personelGorevlendirme.IseGirisTarihi.Year);

        var toplamHak = kidemYil == 1 || kidemYil == 0 ? izinTur.LimitGunSayisi : izinTur.LimitGunSayisi + (4 * kidemYil);

        //IzinTurde bulunan özellikler ile hesaplama yapılacak

        var toplamIzinGun = izinTaleplerToplamGun + talepEdilenGun;

        if(toplamIzinGun > toplamHak)
            return Result<bool>.Failure("İzin hakkı dolmuştur");
        else
            return Result<bool>.Succeed(true);
    }
}
