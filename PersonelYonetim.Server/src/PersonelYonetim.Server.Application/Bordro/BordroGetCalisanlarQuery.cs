using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bordro;
using PersonelYonetim.Server.Domain.PersonelDetaylar;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Tenants;

namespace PersonelYonetim.Server.Application.Bordro;
public sealed record BordroGetCalisanlarQuery(
    ) : IRequest<IQueryable<BordroGetCalisanlarQueryResponse>>;

public sealed class BordroGetCalisanlarQueryResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string? AvatarUrl { get; set; }
    public string? TCKN { get; set; } 
    public DateTimeOffset? IseBaslangicTarihi { get; set; }
    public DateTimeOffset? IstenCikisTarihi { get; set; }
    public int EngelDerecesi { get; set; }
    public string? TabiOlduguKanun { get; set; }
    public string? SGKIsyeri { get; set; }
    public string? VergiDairesiAdi { get; set; }
    public decimal KumulatifVergiMatrahi { get; set; }
    public string? BirimAdi { get; set; }
    public string? PozisyonAd { get; set; }
    public string? MeslekKodu { get; set; }
}

internal sealed class BordroGetCalisanlarQueryHandler(
    IPersonelDetayRepository personelDetayRepository,
    ITenantRepository tenantRepository,
    ICurrentUserService currentUserService,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    IMaasPusulaRepository maasPusulaRepository
    ) : IRequestHandler<BordroGetCalisanlarQuery, IQueryable<BordroGetCalisanlarQueryResponse>>
{
    public Task<IQueryable<BordroGetCalisanlarQueryResponse>> Handle(BordroGetCalisanlarQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if(!tenantId.HasValue)
            return Task.FromResult(Enumerable.Empty<BordroGetCalisanlarQueryResponse>().AsQueryable());

        var personelGorevlendirmeler = personelGorevlendirmeRepository.Where(p => p.TenantId == tenantId && !p.IsDeleted).Include(p => p.Personel).Include(p => p.Pozisyon).Include(p => p.KurumsalBirim);
        if(!personelGorevlendirmeler.Any())
            return Task.FromResult(Enumerable.Empty<BordroGetCalisanlarQueryResponse>().AsQueryable());

        var maasPusulalar = maasPusulaRepository.Where(p => p.TenantId == tenantId && p.BordroDonem!.Yil == DateTimeOffset.Now.Year).OrderBy(p => p.BordroDonem!.Ay).Include(p => p.BordroDonem);

        var tenant = tenantRepository.FirstOrDefault(p => p.Id == tenantId);
        if(tenant is null)
            return Task.FromResult(Enumerable.Empty<BordroGetCalisanlarQueryResponse>().AsQueryable());


        var response = personelGorevlendirmeler
            .GroupJoin(personelDetayRepository.GetAll(),
                personelGorevlendirme => personelGorevlendirme.PersonelId,
                personelDetays => personelDetays.PersonelId,
                (personelGorevlendirme, personelDetays) => new { personelGorevlendirme, personelDetays })
            .SelectMany(
            pp => pp.personelDetays.DefaultIfEmpty(),
            (pp, personelDetay) => new BordroGetCalisanlarQueryResponse
            {
                Id = pp.personelGorevlendirme.Id,
                FullName = pp.personelGorevlendirme.Personel.FullName,
                AvatarUrl = pp.personelGorevlendirme.Personel.AvatarUrl,
                TCKN = personelDetay != null ? personelDetay.TCKN : "",
                IseBaslangicTarihi = pp.personelGorevlendirme.IseGirisTarihi,
                IstenCikisTarihi = pp.personelGorevlendirme.IstenCikisTarihi,
                EngelDerecesi = personelDetay != null ? personelDetay.EngelliMi ? personelDetay.EngelOrani!.Value : 0 : 0,
                TabiOlduguKanun = pp.personelGorevlendirme.TabiOlduguKanun ?? tenant.TabiOlduguKanun,
                SGKIsyeri = pp.personelGorevlendirme.SGKIsyeri ?? tenant.SGKIsyeri,
                VergiDairesiAdi = pp.personelGorevlendirme.VergiDairesiAdi ?? tenant.VergiDairesiAdi,
                KumulatifVergiMatrahi = maasPusulalar.Where(p => p.PersonelId == pp.personelGorevlendirme.PersonelId).FirstOrDefault() != null ? maasPusulalar.Where(p => p.PersonelId == pp.personelGorevlendirme.PersonelId).FirstOrDefault()!.KumulatifGelirVergisiMatrahiDonemSonu : 0,
                BirimAdi = pp.personelGorevlendirme.KurumsalBirim != null ? pp.personelGorevlendirme.KurumsalBirim.Ad : "",
                PozisyonAd = pp.personelGorevlendirme.Pozisyon != null ? pp.personelGorevlendirme.Pozisyon.Ad : "",
                MeslekKodu = pp.personelGorevlendirme.MeslekKodu,
            });

        return Task.FromResult(response);

    }
}
