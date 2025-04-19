using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.ZamanYonetimler;

namespace PersonelYonetim.Server.Application.CalismaCizelgeleri;
public sealed record CalismaCizelgelerGetAllQuery(
    int Yil,
    int Ay
    ) : IRequest<IQueryable<CalismaCizelgeGetAllQueryResponse>>;

public sealed class CalismaCizelgeGetAllQueryResponse
{
    public Guid Id { get; set; }
    public string PersonelAd { get; set; } = default!;
    public decimal PlanlanmisSaat { get; set; }
    public decimal ToplamSaat { get; set; }
    public IEnumerable<Olay> Olaylar { get; set; } = new List<Olay>();
}

public sealed class Olay
{
    public DateTimeOffset Tarih { get; set; }
    public TimeOnly? Baslangic { get; set; }
    public TimeOnly? Bitis { get; set; }
    public decimal ToplamSure { get; set; }
    public OlayTipi OlayTipi { get; set; } = default!;
}

public enum OlayTipi
{
    Calisma,
    Mola,
    Izin,
    FazlaMesai
}

internal sealed class CalismaCizelgeGetAllQueryHandler(
    ICurrentUserService currentUserService,
    ICalismaCizelgeRepository calismaCizelgeRepository,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository
    ) : IRequestHandler<CalismaCizelgelerGetAllQuery, IQueryable<CalismaCizelgeGetAllQueryResponse>>
{
    public Task<IQueryable<CalismaCizelgeGetAllQueryResponse>> Handle(CalismaCizelgelerGetAllQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue || !userId.HasValue)
            throw new Exception("User veya tenantId bulunamamdı");

        var calismaCizelgeler = calismaCizelgeRepository.Where(p =>
            p.Yil == request.Yil &&
            p.Ay == request.Ay &&
            p.TenantId == tenantId.Value)
            .Include(p => p.GunlukCalismalar).ThenInclude(p => p.IzinPeriyotlari)
            .Include(p => p.GunlukCalismalar).ThenInclude(p => p.FazlaMesaiPeriyotlari);

        var response = calismaCizelgeler
                .Join(personelGorevlendirmeRepository.GetAll(),
                calismaCizelge => calismaCizelge.PersonelId,
                personelGorevlendirme => personelGorevlendirme.PersonelId,
                (calismaCizelge, personelGorevlendirme) => new { calismaCizelge, personelGorevlendirme })
                .Include(p => p.personelGorevlendirme.CalismaTakvimi).ThenInclude(p => p!.CalismaGunler)
                .Include(p => p.personelGorevlendirme.Personel)
                .Select(cp => new CalismaCizelgeGetAllQueryResponse
                {
                    Id = cp.calismaCizelge.Id,
                    PersonelAd = cp.personelGorevlendirme.Personel.FullName,
                    PlanlanmisSaat = 0,
                    ToplamSaat = 0,
                    Olaylar = cp.calismaCizelge.GunlukCalismalar
                        .Select(gunlukCalisma => new Olay
                        {
                            Tarih = gunlukCalisma.Tarih,
                            Baslangic = cp.personelGorevlendirme.CalismaTakvimi!.CalismaGunler.Where(p => p.Gun == gunlukCalisma.Tarih.DayOfWeek).FirstOrDefault()!.CalismaBaslangic,
                            Bitis = cp.personelGorevlendirme.CalismaTakvimi.CalismaGunler.Where(p => p.Gun == gunlukCalisma.Tarih.DayOfWeek).FirstOrDefault()!.CalismaBitis,
                            ToplamSure = cp.personelGorevlendirme.CalismaTakvimi.CalismaGunler.Where(p => p.Gun == gunlukCalisma.Tarih.DayOfWeek).FirstOrDefault()!.ToplamCalisma,
                            OlayTipi = OlayTipi.Calisma
                        }),
                });


        return Task.FromResult(response);

    }
}


