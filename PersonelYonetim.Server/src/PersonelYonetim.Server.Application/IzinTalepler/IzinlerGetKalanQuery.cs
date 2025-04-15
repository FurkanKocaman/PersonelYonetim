using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.IzinKurallar;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.IzinTalepler;
public sealed record IzinlerGetKalanQuery(
    ) : IRequest<IzinlerGetKalanQueryResponse>;

public sealed class IzinlerGetKalanQueryResponse
{
    public string IzinTurAd { get; set; } = string.Empty;
    public decimal Kullanilan {  get; set; }
    public decimal Kalan {  get; set; }
    public string GuncelHakEdisDonem { get; set; } = string.Empty;
}
internal sealed class IzinlerGetKalanQueryHandler(
    IPersonelRepository personelRepository,
    IIzinKuralRepository izinKuralRepository,
    IIzinTalepRepository izinTalepRepository,
    IPersonelAtamaRepository personelAtamaRepository,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<IzinlerGetKalanQuery, IzinlerGetKalanQueryResponse>
{
    public Task<IzinlerGetKalanQueryResponse> Handle(IzinlerGetKalanQuery request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");

        var personel = personelRepository.GetAll()
            .Where(p => p.UserId == Guid.Parse(userIdString) && !p.IsDeleted)
            .Select(p => new { p.Id })
            .FirstOrDefault();

        if (personel is null)
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");

        var izinKuralKayitlari = izinKuralRepository.GetAll()
            .Where(kural => !kural.IsDeleted)
            .Include(kural => kural.IzinTurler)
            .ToList();

        var atamaKaydi = personelAtamaRepository.GetAll()
            .FirstOrDefault(pa => pa.PersonelId == personel.Id);

        if (atamaKaydi is null)
            throw new Exception("Personelin atama kaydı bulunamadı.");

        // Geçerli yıl ve sadece "Yıllık İzin" için kullanılan talepler
        var izinTalepler = izinTalepRepository
            .Where(talep =>
                talep.PersonelId == personel.Id &&
                talep.BitisTarihi.Year == DateTime.Now.Year &&
                talep.IzinTur.Ad == "Yillik Izin" &&
                //talep.DegerlendirmeDurumu != DegerlendirmeDurumEnum.Reddedildi &&
                !talep.IsDeleted)
            .ToList();

        var kullanilanSure = izinTalepler.Sum(t => t.ToplamSure);

        var kuralKaydi = izinKuralKayitlari
            .FirstOrDefault(kural =>
                                      kural.IzinTurler.Any(t => t.Ad == "Yillik Izin"));

        if (kuralKaydi is null)
            throw new Exception("İlgili yıllık izin kuralı bulunamadı.");

        var izinTur = kuralKaydi.IzinTurler.First(t => t.Ad == "Yillik Izin");

        var kacYildirCalisiyor = Math.Max(1, DateTimeOffset.Now.Year - atamaKaydi.PozisyonBaslamaTarihi.Year);
        var toplamHak = kacYildirCalisiyor * izinTur.LimitGunSayisi;

        var donemBaslangic = new DateTimeOffset(DateTimeOffset.Now.Year, atamaKaydi.PozisyonBaslamaTarihi.Month, atamaKaydi.PozisyonBaslamaTarihi.Day, 0, 0, 0, DateTimeOffset.Now.Offset);
        if (DateTimeOffset.Now < donemBaslangic)
            donemBaslangic = donemBaslangic.AddYears(-1);
        var donemBitis = donemBaslangic.AddYears(1);

        var response = new IzinlerGetKalanQueryResponse
        {
            IzinTurAd = izinTur.Ad,
            Kullanilan = kullanilanSure,
            Kalan = toplamHak - kullanilanSure,
            GuncelHakEdisDonem = $"{donemBaslangic:dd-MM-yyyy} - {donemBitis:dd-MM-yyyy}"
        };

        return Task.FromResult(response);
    }
}
