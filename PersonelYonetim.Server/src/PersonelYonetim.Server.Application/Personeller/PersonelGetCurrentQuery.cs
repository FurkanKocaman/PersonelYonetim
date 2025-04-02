using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Roller;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.Personeller;

public sealed record PersonelGetCurrentQuery() : IRequest<IQueryable<PersonelGetCurrentQueryResponse>>;

public sealed class PersonelGetCurrentQueryResponse : EntityDto
{
    public string FullName { get; set; } = default!;
    public DateTimeOffset DogumTarihi { get; set; }
    public DateTimeOffset IseGirisTarihi { get; set; }
    public string? ProfilResimUrl { get; set; }
    public string? Cinsiyet { get; set; }
    public string DepartmanAd { get; set; } = default!;
    public string PozisyonAd { get; set; } = default!;
    public string Eposta { get; set; } = default!;
    public string Telefon { get; set; } = default!;
    public string Ulke { get; set; } = default!;
    public string Sehir { get; set; } = default!;
    public string Ilce { get; set; } = default!;
    public string TamAdres { get; set; } = default!;
    public string? Yonetici { get; set; }
    public string Role { get; set; } = default!;
    public Guid UserId { get; set; } = default!;
}
public sealed class PersonelGetCurrentQueryHandler(
    IPersonelRepository personelRepository,
    //IDepartmanRepository departmanRepository,
    //IPozisyonRepository pozisyonRepository,
    //IPersonelAtamaRepository personelAtamaRepository,
    IUserRoleRepository userRoleRepository,
    RoleManager<AppRole> roleManager,
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

        var userId = Guid.Parse(userIdString);

        var response = personelRepository
            .GetAll()
            .Where(p => p.UserId == userId && !p.IsDeleted)
            .Select(entity => new PersonelGetCurrentQueryResponse
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Cinsiyet = entity.Cinsiyet.HasValue ? (entity.Cinsiyet.Value ? "Erkek" : "Kadın") : null,
                Eposta = entity.Iletisim.Eposta,
                Telefon = entity.Iletisim.Telefon,
                DogumTarihi = entity.DogumTarihi,
                IseGirisTarihi = DateTimeOffset.Now,
                ProfilResimUrl = entity.ProfilResimUrl,
                Ulke = entity.Adres.Ulke,
                Sehir = entity.Adres.Sehir,
                Ilce = entity.Adres.Ilce,
                TamAdres = entity.Adres.TamAdres,
                DepartmanAd = entity.PersonelAtamalar
                    .Select(pa => pa.Departman!.Ad)
                    .FirstOrDefault() ?? "",
                PozisyonAd = entity.PersonelAtamalar
                    .Select(pa => pa.Pozisyon!.Ad)
                    .FirstOrDefault() ?? "",
                UserId = entity.UserId,
                //Yonetici = entity.Yonetici != null ? entity.Yonetici.FullName : "",
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreateUserId = entity.CreateUserId,
                CreateUserName = userManager.Users
                    .Where(u => u.Id == entity.CreateUserId)
                    .Select(u => $"{u.FirstName} {u.LastName} ({u.Email})")
                    .FirstOrDefault() ?? "",
                UpdateAt = entity.UpdateAt,
                UpdateUserId = entity.UpdateUserId,
                UpdateUserName = entity.UpdateUserId.HasValue
                    ? userManager.Users
                        .Where(u => u.Id == entity.UpdateUserId)
                        .Select(u => $"{u.FirstName} {u.LastName} ({u.Email})")
                        .FirstOrDefault()
                    : null,
                IsDeleted = entity.IsDeleted,
                DeleteAt = entity.DeleteAt,
                Role = userRoleRepository
                    .GetAll()
                    .Where(ur => ur.UserId == entity.UserId)
                    .Join(roleManager.Roles,
                        ur => ur.RoleId,
                        role => role.Id,
                        (ur, role) => role.Name)
                    .FirstOrDefault()!,
            });

        return Task.FromResult(response);
    }
}
