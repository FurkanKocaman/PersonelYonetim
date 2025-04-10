using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Pozisyonlar;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.PersonelAtamalar;
public sealed record PersonelAtamaGetQuery(
    ) : IRequest<IQueryable<PersonelAtamaGetQueryResponse>>;

public sealed class PersonelAtamaGetQueryResponse : EntityDto
{
    public string FullName { get; set; } = default!;
    public string SirketAd { get; set; } = default!;
    public Guid SirketId { get; set; } = default!;
    public string? SubeAd { get; set; }
    public Guid? SubeId { get; set; }
    public string? DepartmanAd { get; set; }
    public Guid? DepartmanId { get; set; }
    public string? PozisyonAd { get; set; }
    public Guid? PozisyonId { get; set; }
    public string RolAd { get; set; } = default!;
    public Guid? YoneticiId { get; set; }
    public string? YoneticiAd { get; set; } 
    public int SozlesmeTuruValue { get; set; }
    public DateTimeOffset PozisyonBaslangicTarih { get; set; }
    public DateTimeOffset? PozisyonBitisTarih { get; set; }
    public string CalismaTakvimAd { get; set; } = "";
    public string IzinKuralAd { get; set; } = "";
}


internal sealed class PersonelAtamaGetQueryHandler(
    IPersonelAtamaRepository personelAtamaRepository,
    IHttpContextAccessor httpContextAccessor,
    IPersonelRepository personelRepository) : IRequestHandler<PersonelAtamaGetQuery, IQueryable<PersonelAtamaGetQueryResponse>>
{
    public Task<IQueryable<PersonelAtamaGetQueryResponse>> Handle(PersonelAtamaGetQuery request, CancellationToken cancellationToken)
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

        if (personel == null)
        {
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
        }

        var personelAtamalar = personelAtamaRepository.Where(p => p.PersonelId == personel.Id);

        var response = personelAtamalar
            .Select(p => new PersonelAtamaGetQueryResponse
            {
                Id = p.Id,
                FullName = p.Personel!.FullName,
                SirketId = p.SirketId,
                SirketAd = p.Sirket!.Ad,
                SubeId = p.SubeId,
                SubeAd = p.Sube != null ? p.Sube.Ad : null,
                DepartmanId = p.DepartmanId,
                DepartmanAd = p.Departman != null ? p.Departman.Ad : null,
                PozisyonId = p.PozisyonId,
                PozisyonAd = p.Pozisyon != null ? p.Pozisyon.Ad : null,
                RolAd = p.RolTipi.Name,
                YoneticiId = p.YoneticiId,
                YoneticiAd = p.Yonetici != null ? p.Yonetici.Ad : null,
                SozlesmeTuruValue = p.SozlesmeTuru.Value,
                PozisyonBaslangicTarih = p.PozisyonBaslamaTarihi,
                PozisyonBitisTarih = p.SozlesmeBitisTarihi,
                CalismaTakvimAd = p.CalismaTakvimi != null ? p.CalismaTakvimi.Ad : "",
                //IzinKuralAd = p.IzinKural != null ? p.IzinKural!.Ad : "",
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                CreateUserId = p.CreateUserId,
                CreateUserName = "",
                UpdateAt = p.UpdateAt,
                UpdateUserName = "",
                IsDeleted = p.IsDeleted,
                DeleteAt = p.DeleteAt
            });
        return Task.FromResult(response);

    }
}
