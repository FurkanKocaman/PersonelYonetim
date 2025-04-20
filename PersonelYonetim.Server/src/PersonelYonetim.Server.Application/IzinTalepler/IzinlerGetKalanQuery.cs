using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Personeller;

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
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    IGorevlendirmeIzinKuraliRepository gorevlendirmeIzinKuraliRepository,
    IPersonelRepository personelRepository,
    IIzinTalepRepository izinTalepRepository,
    ICurrentUserService currentUserService
) : IRequestHandler<IzinlerGetKalanQuery, IzinlerGetKalanQueryResponse>
{
    public Task<IzinlerGetKalanQueryResponse> Handle(IzinlerGetKalanQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        Guid? tenantId = currentUserService.TenantId;

        if (!userId.HasValue || !tenantId.HasValue)
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }

        var personelId = personelRepository.FirstOrDefault(p => p.UserId == userId).Id;

        if(personelId == Guid.Empty)
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");

        var personelGorevlendirme = personelGorevlendirmeRepository.FirstOrDefault(p => p.PersonelId == personelId && p.IsDeleted == false && p.IsActive && p.TenantId == tenantId);
        if (personelGorevlendirme is null)
            throw new UnauthorizedAccessException("Kullanıcı görevlendirme bulunamadı.");

        var gorevlendirmeIzinKurali = gorevlendirmeIzinKuraliRepository.Where(p => p.PersonelGorevlendirmeId == personelGorevlendirme.Id && p.IsActive && !p.IsDeleted)
                                        .Include(p => p.IzinKural).ThenInclude(p => p.IzinTurler).FirstOrDefault();

        if (gorevlendirmeIzinKurali is null || gorevlendirmeIzinKurali.IzinKural is null || !gorevlendirmeIzinKurali.IzinKural.IzinTurler.Any())
            throw new UnauthorizedAccessException("Kullanıcı görevlendirme izin kuralı bulunamadı.");

        var izinTur = gorevlendirmeIzinKurali.IzinKural.IzinTurler.FirstOrDefault(p => p.Ad == "Yillik Izin");
        if (izinTur is null)
            throw new UnauthorizedAccessException("İzin türü bulunamadı.");

        decimal izinTaleplerToplamGun = izinTalepRepository.Where(p => p.IzinTurId == izinTur.Id && p.PersonelId == personelId).Sum(p => p.ToplamSure);

        var kidemYil = (DateTimeOffset.Now.Year - personelGorevlendirme.BaslangicTarihi.Year);

        var toplamHak = kidemYil == 1 || kidemYil == 0 ? izinTur.LimitGunSayisi : izinTur.LimitGunSayisi + (4 * kidemYil);

        var donemBaslangic = new DateTimeOffset(DateTimeOffset.Now.Year, personelGorevlendirme.BaslangicTarihi.Month, personelGorevlendirme.BaslangicTarihi.Day, 0, 0, 0, DateTimeOffset.Now.Offset);
        if (DateTimeOffset.Now < donemBaslangic)
            donemBaslangic = donemBaslangic.AddYears(-1);

        var donemBitis = donemBaslangic.AddYears(1);

        var response = new IzinlerGetKalanQueryResponse
        {
            IzinTurAd = izinTur.Ad,
            Kullanilan = izinTaleplerToplamGun,
            Kalan = toplamHak - izinTaleplerToplamGun,
            GuncelHakEdisDonem = $"{donemBaslangic:dd-MM-yyyy} - {donemBitis:dd-MM-yyyy}"
        };

        return Task.FromResult(response);
    }
}
