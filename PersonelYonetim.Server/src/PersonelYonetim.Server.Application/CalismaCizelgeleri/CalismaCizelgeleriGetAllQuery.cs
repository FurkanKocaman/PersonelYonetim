using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Dtos;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.ZamanYonetimler;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.CalismaCizelgeleri;
public sealed record CalismaCizelgeleriGetAllQuery(
    Guid? tenantId = null) : IRequest<List<CalismaCizelgeleriGetAllQueryResponse>>;

public sealed class CalismaCizelgeleriGetAllQueryResponse : EntityDto
{
    public string PersonelAd { get; set; } = string.Empty;
    public int Yil { get; set; }
    public int Ay { get; set; }
    public List<GunlukCalismaResponse> GunlukCalismalar { get; set; } = new List<GunlukCalismaResponse>();

}
public sealed class GunlukCalismaResponse
{
    public DateTimeOffset Tarih { get; set; }
    public List<CalismaPeriyotDto> CalismaPeriyotlari { get; set; } = new List<CalismaPeriyotDto>();
    public List<MolaPeriyotDto> MolaPeriyotlari { get; set; } = new List<MolaPeriyotDto>();
    public List<IzinPeriyotDto> IzinPeriyotlari { get; set; } = new List<IzinPeriyotDto>();
    public List<FazlaMesaiPeriyotDto> FazlaMesaiPeriyotlari { get; set; } = new List<FazlaMesaiPeriyotDto>();

}

internal sealed class CalismaCizelgeleriGetAllQueryResponseHandler(
    IHttpContextAccessor httpContextAccessor,
    IPersonelRepository personelRepository,
    ICalismaCizelgeRepository calismaCizelgeRepository,
    IGunlukCalismaRepository gunlukCalismaRepository
    ) : IRequestHandler<CalismaCizelgeleriGetAllQuery, List<CalismaCizelgeleriGetAllQueryResponse>>
{
    public Task<List<CalismaCizelgeleriGetAllQueryResponse>> Handle(CalismaCizelgeleriGetAllQuery request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }

        var personel = personelRepository.GetAll()
            .Where(p => p.UserId == Guid.Parse(userIdString))
            .Include(p => p.PersonelGorevlendirmeler)
            .Select(p => new { p.Id, p.FullName, p.PersonelGorevlendirmeler })
            .FirstOrDefault();

        if (personel is null)
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");

        var personelAtama = personel.PersonelGorevlendirmeler.Where(p => request.tenantId != null ? p.TenantId == request.tenantId : true && p.IsActive == true && p.IsDeleted == false).FirstOrDefault();
       
        if (personelAtama is null)
            throw new UnauthorizedAccessException("Personelatama bilgisi bulunamadı.");

        var calismaCizelgeleri = calismaCizelgeRepository
          .Where(p => p.TenantId == personelAtama.TenantId && p.IsDeleted == false)
          .Include(p => p.Personel)
          .OrderBy(p => p.PersonelId)
          .ToList();

        var responseList = new List<CalismaCizelgeleriGetAllQueryResponse>();

        foreach (var calismaCizelge in calismaCizelgeleri)
        {
            var gunlukCalismalar = gunlukCalismaRepository
                .Where(g => g.CalismaCizelgesiId == calismaCizelge.Id)
                .Include(g => g.CalismaPeriyotlari)
                .Include(g => g.IzinPeriyotlari)
                .Include(g => g.MolaPeriyotlari)
                .Include(g => g.FazlaMesaiPeriyotlari)
                .OrderBy(g => g.Tarih)
                .ToList();

            var response = new CalismaCizelgeleriGetAllQueryResponse
            {
                Id = calismaCizelge.Id,
                PersonelAd = calismaCizelge.Personel.FullName,
                Yil = calismaCizelge.Yil,
                Ay = calismaCizelge.Ay,
                GunlukCalismalar = gunlukCalismalar.Select(g => new GunlukCalismaResponse
                {
                    Tarih = g.Tarih,
                    CalismaPeriyotlari = g.CalismaPeriyotlari.Select(cp => new CalismaPeriyotDto
                    {
                        Id = cp.Id,
                        GunlukCalismaId = cp.GunlukCalismaId,
                        BaslangicSaati = cp.BaslangicSaati,
                        BitisSaati = cp.BitisSaati,
                        CalismaTipi = cp.CalismaPeriyoduTipi == 0 ? "Normal" : "Fazla Mesai"

                    }).ToList(),
                    MolaPeriyotlari = g.MolaPeriyotlari.Select(mp => mp.Adapt<MolaPeriyotDto>()).ToList(),
                    IzinPeriyotlari = g.IzinPeriyotlari.Select(ip => ip.Adapt<IzinPeriyotDto>()).ToList(),
                    FazlaMesaiPeriyotlari = g.FazlaMesaiPeriyotlari.Select(fp => fp.Adapt<FazlaMesaiPeriyotDto>()).ToList()
                }).OrderBy(p => p.Tarih).ToList(),
                IsActive = calismaCizelge.IsActive,
                CreatedAt = calismaCizelge.CreatedAt,
                CreateUserId = calismaCizelge.CreateUserId,
                CreateUserName = "", // Gerekirse doldurun
                UpdateAt = calismaCizelge.UpdateAt,
                UpdateUserId = calismaCizelge.UpdateUserId,
                UpdateUserName = calismaCizelge.UpdateuserName,
                IsDeleted = calismaCizelge.IsDeleted,
                DeleteAt = calismaCizelge.DeleteAt,
            };

            responseList.Add(response);
        }

        return Task.FromResult(responseList);
    }
}
