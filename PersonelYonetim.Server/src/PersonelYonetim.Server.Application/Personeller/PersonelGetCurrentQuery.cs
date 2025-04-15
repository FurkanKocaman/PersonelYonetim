using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.Personeller;

public sealed record PersonelGetCurrentQuery() : IRequest<IQueryable<PersonelGetCurrentQueryResponse>>;

public sealed class PersonelGetCurrentQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string Soyad { get; set; } = default!;
    public DateTimeOffset DogumTarihi { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Cinsiyet { get; set; }
    public string KurumsalBirimAd { get; set; } = string.Empty;
    public string PozisyonAd { get; set; } = string.Empty;
    public string? YoneticiAd { get; set; }
    public string? YoneticiPozisyon { get; set; }
    public string Role { get; set; } = default!;
}
public sealed class PersonelGetCurrentQueryHandler(
    IPersonelRepository personelRepository,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    UserManager<AppUser> userManager,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<PersonelGetCurrentQuery, IQueryable<PersonelGetCurrentQueryResponse>>
{
    public Task<IQueryable<PersonelGetCurrentQueryResponse>> Handle(PersonelGetCurrentQuery request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }

        var personel = personelRepository
            .Where(p => p.UserId == Guid.Parse(userIdString) && !p.IsDeleted).FirstOrDefault();

        if (personel == null)
        {
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
        }

        var response = personelGorevlendirmeRepository.Where(p => p.PersonelId == personel.Id && p.IsActive && !p.IsDeleted)
                    .GroupJoin(userManager.Users,
                    personelGorevlendirme => personel.CreateUserId,
                    createUser => createUser.Id,
                    (personelGorevlendirme, createUsers) => new { personelGorevlendirme, createUsers })
                    .SelectMany(
                    pu => pu.createUsers.DefaultIfEmpty(),
                    (pu, createUser) => new PersonelGetCurrentQueryResponse
                    {
                        Id = personel.Id,
                        Ad = personel.Ad,
                        Soyad = personel.Soyad,
                        DogumTarihi = personel.DogumTarihi,
                        AvatarUrl = personel.AvatarUrl,
                        Cinsiyet = personel.Cinsiyet != null ? personel.Cinsiyet.Value ? "Erkek" : "Kadın" : "Bilinmiyor",
                        KurumsalBirimAd = pu.personelGorevlendirme.KurumsalBirim != null ? pu.personelGorevlendirme.KurumsalBirim.Ad : "Bilinmiyor",
                        PozisyonAd = pu.personelGorevlendirme.Pozisyon != null ? pu.personelGorevlendirme.Pozisyon.Ad : "Bilinmiyor",
                        YoneticiAd = pu.personelGorevlendirme.RaporlananGorevlendirme != null ? pu.personelGorevlendirme.RaporlananGorevlendirme.Personel.FullName : "Bilinmiyor",
                        YoneticiPozisyon = pu.personelGorevlendirme.RaporlananGorevlendirme != null ? pu.personelGorevlendirme.RaporlananGorevlendirme.Pozisyon.Ad : "Bilinmiyor",
                        IsActive = personel.IsActive,
                        CreatedAt = personel.CreatedAt,
                        CreateUserId = createUser != null ? createUser.Id : Guid.Empty,
                        CreateUserName = createUser != null ? createUser.FirstName + " " + createUser.LastName + " (" + createUser.Email + ")" : "Bilinmiyor",
                        UpdateAt = personel.UpdateAt,
                        UpdateUserId = null,
                        UpdateUserName = null,
                        IsDeleted = personel.IsDeleted,
                        DeleteAt = personel.DeleteAt
                    });

        return Task.FromResult(response);
    }
}
