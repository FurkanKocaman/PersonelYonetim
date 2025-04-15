using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.IzinKurallar;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.IzinTurler;
public sealed record IzinTurGetAllQuery () : IRequest<IQueryable<IzinTurGetAllQueryResponse>>;

public sealed class IzinTurGetAllQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public bool UcretliMi { get; set; }
    public string LimitTipi { get; set; } = default!;
    public Guid SirketId { get; set; }
    public string SirketAd { get; set; } = default!;
}

internal sealed class IzinTurGetAllQueryHandler(
    IIzinTurRepository izinTurRepository,
    IPersonelRepository personelRepository,
    IPersonelAtamaRepository personelAtamaRepository,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<IzinTurGetAllQuery, IQueryable<IzinTurGetAllQueryResponse>>
{
    public Task<IQueryable<IzinTurGetAllQueryResponse>> Handle(IzinTurGetAllQuery request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }

        var personel = personelRepository.GetAll()
            .Where(p => p.UserId == Guid.Parse(userIdString) && !p.IsDeleted)
            .Select(p => new { p.Id })
            .FirstOrDefault();

        if (personel is null)
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");

        var response = izinTurRepository.GetAll()
                .Where(p => !p.IsDeleted)
                .Join(personelAtamaRepository.GetAll(),
                    izinTur => izinTur.TenantId,
                    personelAtama => personelAtama.SirketId,
                    (izinTur, personelAtama) => new { izinTur, personelAtama })
                .Where(ip => ip.personelAtama.PersonelId == personel.Id && !ip.personelAtama.IsDeleted)
                .Select(ip => new IzinTurGetAllQueryResponse
                {
                    Id = ip.izinTur.Id,
                    Ad = ip.izinTur.Ad,
                    Aciklama = ip.izinTur.Aciklama,
                    UcretliMi = ip.izinTur.UcretliMi,
                    LimitTipi = ip.izinTur.LimitTipi == LimitTipiEnum.Limitsiz ? ip.izinTur.LimitTipi.Name : ip.izinTur.LimitTipi.Name + " " + ip.izinTur.LimitGunSayisi +" gün",
                    SirketId = ip.izinTur.TenantId,
                    //SirketAd = ip.izinTur.t.Ad,
                    IsActive = ip.izinTur.IsActive,
                    CreatedAt = ip.izinTur.CreatedAt,
                    IsDeleted = ip.izinTur.IsDeleted,
                });

        return Task.FromResult(response);
    }
}
