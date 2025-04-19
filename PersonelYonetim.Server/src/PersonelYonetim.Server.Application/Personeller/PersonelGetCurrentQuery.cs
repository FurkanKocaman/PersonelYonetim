using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Domain.Users;

namespace PersonelYonetim.Server.Application.Personeller;

public sealed record PersonelGetCurrentQuery() : IRequest<IQueryable<PersonelGetCurrentQueryResponse>>;

public sealed class PersonelGetCurrentQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string Soyad { get; set; } = default!;
    public DateTimeOffset DogumTarihi { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Cinsiyet { get; set; }
    public Iletisim Iletisim { get; set; } = default!;
    public Adres Adres { get; set; } = default!;
    public string KurumsalBirimAd { get; set; } = string.Empty;
    public string PozisyonAd { get; set; } = string.Empty;
    public string? YoneticiAd { get; set; }
    public string? YoneticiPozisyon { get; set; }
    public DateTimeOffset BaslangicTarih { get; set; }
    public List<string> RoleClaims { get; set; } = new List<string>();
}
public sealed class PersonelGetCurrentQueryHandler(
    IPersonelRepository personelRepository,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    ICurrentUserService currentUserService
    )
    : IRequestHandler<PersonelGetCurrentQuery, IQueryable<PersonelGetCurrentQueryResponse>>
{
    public async Task<IQueryable<PersonelGetCurrentQueryResponse>> Handle(PersonelGetCurrentQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        Guid? tenantId = currentUserService.TenantId;

        if (!userId.HasValue || !tenantId.HasValue)
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");

        var personel = await personelRepository
            .Where(p => p.UserId == userId && !p.IsDeleted)
            .Include(p => p.PersonelGorevlendirmeler)
                .ThenInclude(p => p.GorevlendirmeRolleri)
                    .ThenInclude(p => p.Rol)
            .FirstOrDefaultAsync(cancellationToken);

        if (personel == null)
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");

        var personelGorevlendirmeler = await personelGorevlendirmeRepository
            .Where(p => p.PersonelId == personel.Id && p.IsActive && !p.IsDeleted)
            .Include(p => p.GorevlendirmeRolleri).ThenInclude(p => p.Rol)
            .Include(p => p.KurumsalBirim)
            .Include(p => p.Pozisyon)
            .Include(p => p.RaporlananGorevlendirme).ThenInclude(r => r!.Personel)
            .Include(p => p.RaporlananGorevlendirme).ThenInclude(r => r!.Pozisyon)
            .ToListAsync(cancellationToken);

        var responseList = new List<PersonelGetCurrentQueryResponse>();

        foreach (var gorevlendirme in personelGorevlendirmeler)
        {
            var roleClaims = new List<string>();

            foreach (var gorevlendirmeRol in gorevlendirme.GorevlendirmeRolleri)
            {
                var claims = await roleManager.GetClaimsAsync(gorevlendirmeRol.Rol);
                roleClaims.AddRange(claims.Select(c => c.Value));
            }

            var createUser = await userManager.FindByIdAsync(personel.CreateUserId.ToString());

            responseList.Add(new PersonelGetCurrentQueryResponse
            {
                Id = personel.Id,
                Ad = personel.Ad,
                Soyad = personel.Soyad,
                DogumTarihi = personel.DogumTarihi,
                AvatarUrl = personel.AvatarUrl,
                Cinsiyet = personel.Cinsiyet != null ? personel.Cinsiyet.Value ? "Erkek" : "Kadın" : "Bilinmiyor",
                Adres = personel.Adres,
                Iletisim = personel.Iletisim,
                BaslangicTarih = gorevlendirme.BaslangicTarihi,
                RoleClaims = roleClaims.Distinct().ToList(),
                KurumsalBirimAd = gorevlendirme.KurumsalBirim?.Ad ?? "Bilinmiyor",
                PozisyonAd = gorevlendirme.Pozisyon?.Ad ?? "Bilinmiyor",
                YoneticiAd = gorevlendirme.RaporlananGorevlendirme?.Personel?.FullName ?? "Bilinmiyor",
                YoneticiPozisyon = gorevlendirme.RaporlananGorevlendirme?.Pozisyon?.Ad ?? "Bilinmiyor",
                IsActive = personel.IsActive,
                CreatedAt = personel.CreatedAt,
                CreateUserId = createUser?.Id ?? Guid.Empty,
                CreateUserName = createUser != null ? $"{createUser.FirstName} {createUser.LastName} ({createUser.Email})" : "Bilinmiyor",
                UpdateAt = personel.UpdateAt,
                UpdateUserId = null,
                UpdateUserName = null,
                IsDeleted = personel.IsDeleted,
                DeleteAt = personel.DeleteAt
            });
        }

        return responseList.AsQueryable();
    }

}
