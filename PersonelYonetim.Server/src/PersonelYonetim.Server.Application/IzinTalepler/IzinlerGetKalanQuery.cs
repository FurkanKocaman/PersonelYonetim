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

//internal sealed class IzinlerGetKalanQueryHandler(
//    IPersonelRepository personelRepository,
//    IIzinKuralRepository izinKuralRepository,
//    IIzinTalepRepository izinTalepRepository,
//    IPersonelAtamaRepository personelAtamaRepository,
//    IHttpContextAccessor httpContextAccessor) : IRequestHandler<IzinlerGetKalanQuery, IzinlerGetKalanQueryResponse>
//{
//    public Task<IzinlerGetKalanQueryResponse> Handle(IzinlerGetKalanQuery request, CancellationToken cancellationToken)
//    {
//        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//        if (string.IsNullOrEmpty(userIdString))
//        {
//            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
//        }

//        var personel = personelRepository.GetAll()
//            .Where(p => p.UserId == Guid.Parse(userIdString) && !p.IsDeleted)
//        .Select(p => new { p.Id })
//            .FirstOrDefault();

//        if (personel is null)
//            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");

//        var izinKurallar = izinKuralRepository.GetAll()
//        .Where(p => !p.IsDeleted)
//            .Include(p => p.IzinTurler);

//        var izinTalepler = izinTalepRepository.Where(p => p.PersonelId == personel.Id && p.BitisTarihi.Year == DateTime.Now.Year && p.IzinTur.Ad == "Yillik Izin" && !p.IsDeleted);


//        var response = izinKurallar
//                .Join(personelAtamaRepository.GetAll(),
//                    izinKural => izinKural.SirketId,
//                    personelAtama => personelAtama.SirketId,
//                    (izinKural, personelAtama) => new { izinKural, personelAtama })
//                .Where(ip => ip.personelAtama.PersonelId == personel.Id)
//                .Select(p => new IzinlerGetKalanQueryResponse
//                    {
//                    //IzinTurAd = p.izinKural.IzinTurler.FirstOrDefault()!.IzinTur.Ad,
//                    Kullanilan = izinTalepler.Where(p => p.DegerlendirmeDurumu != DegerlendirmeDurumEnum.Reddedildi).Sum(p => p.ToplamSure),
//                    Kalan = (((DateTimeOffset.Now.Year - p.personelAtama.PozisyonBaslamaTarihi.Year) == 0 ? 1 : (DateTimeOffset.Now.Year - p.personelAtama.PozisyonBaslamaTarihi.Year)) * p.izinKural.IzinTurler.Where(p => p.IzinTur.Ad == "Yillik Izin").FirstOrDefault()!.IzinTur.LimitGunSayisi) - izinTalepler.Where(p =>p.DegerlendirmeDurumu != DegerlendirmeDurumEnum.Reddedildi).Sum(p => p.ToplamSure),
//                    GuncelHakEdisDonem = $"{(DateTimeOffset.Now < new DateTimeOffset(DateTimeOffset.Now.Year, p.personelAtama.PozisyonBaslamaTarihi.Month, p.personelAtama.PozisyonBaslamaTarihi.Day, 0, 0, 0, DateTimeOffset.Now.Offset)
//                    ? new DateTimeOffset(DateTimeOffset.Now.Year - 1, p.personelAtama.PozisyonBaslamaTarihi.Month, p.personelAtama.PozisyonBaslamaTarihi.Day, 0, 0, 0, DateTimeOffset.Now.Offset)
//                    : new DateTimeOffset(DateTimeOffset.Now.Year, p.personelAtama.PozisyonBaslamaTarihi.Month, p.personelAtama.PozisyonBaslamaTarihi.Day, 0, 0, 0, DateTimeOffset.Now.Offset)):dd-MM-yyyy} - " +
//                    $"{(DateTimeOffset.Now < new DateTimeOffset(DateTimeOffset.Now.Year, p.personelAtama.PozisyonBaslamaTarihi.Month, p.personelAtama.PozisyonBaslamaTarihi.Day, 0, 0, 0, DateTimeOffset.Now.Offset)
//                    ? new DateTimeOffset(DateTimeOffset.Now.Year, p.personelAtama.PozisyonBaslamaTarihi.Month, p.personelAtama.PozisyonBaslamaTarihi.Day, 0, 0, 0, DateTimeOffset.Now.Offset)
//                    : new DateTimeOffset(DateTimeOffset.Now.Year + 1, p.personelAtama.PozisyonBaslamaTarihi.Month, p.personelAtama.PozisyonBaslamaTarihi.Day, 0, 0, 0, DateTimeOffset.Now.Offset)):dd-MM-yyyy}",
//                }).FirstOrDefault();

//        return Task.FromResult(response!);
//    }
//}
