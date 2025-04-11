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
using System.Runtime;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.Personeller;

public sealed record PersonelGetCurrentQuery() : IRequest<IQueryable<PersonelGetCurrentQueryResponse>>;

public sealed class PersonelGetCurrentQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string Soyad { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public DateTimeOffset DogumTarihi { get; set; }
    public DateTimeOffset PozisyonBaslangicTarih { get; set; }
    public string? ProfilResimUrl { get; set; }
    public string? Cinsiyet { get; set; }
    public string SirketAd { get; set; } = default!;
    public Guid SirketId { get; set; } = default!;
    public string? SubeAd { get; set; }
    public Guid? SubeId { get; set; }
    public string? DepartmanAd { get; set; }
    public Guid? DepartmanId { get; set; }
    public string? PozisyonAd { get; set; }
    public Guid? PozisyonId { get; set; }
    public Iletisim Iletisim { get; set; } = default!;
    public Adres Adres { get; set; } = default!;
    public string? Yonetici { get; set; }
    public string? YoneticiPozisyon { get; set; }
    public int Role { get; set; } = default!;
    public Guid UserId { get; set; } = default!;
    public Guid? CalismaTakvimiId { get; set; }
    public Guid? IzinKuralId { get; set; }
    public int SozlesmeTuruValue { get; set; }
    public DateTimeOffset? SozlesmeBitisTarihi { get; set; }

}
public sealed class PersonelGetCurrentQueryHandler(
    IPersonelRepository personelRepository,
    IPersonelAtamaRepository personelAtamaRepository,
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

        var response = personelAtamaRepository.GetAll()
                .Where(p => p.PersonelId == personel.Id && p.IsDeleted == false)
                .Join(userManager.Users,
                personelAtama => personelAtama.Personel!.CreateUserId,
                createUser => createUser.Id,
                (personelAtama, createUser) => new { personelAtama, createUser })
                .Select(pu => new PersonelGetCurrentQueryResponse
                {
                    Id = personel.Id,
                    Ad = personel.Ad,
                    Soyad = personel.Soyad,
                    FullName = personel.FullName,
                    DogumTarihi = personel.DogumTarihi,
                    PozisyonBaslangicTarih = pu.personelAtama.PozisyonBaslamaTarihi,
                    ProfilResimUrl = personel.AvatarUrl,
                    Cinsiyet = personel.Cinsiyet == null ? null : pu.personelAtama.Personel!.Cinsiyet == true ? "Erkek" : "Kadın",
                    SirketId = pu.personelAtama.SirketId,
                    SirketAd = pu.personelAtama.Sirket!.Ad,
                    SubeId = pu.personelAtama.SubeId,
                    SubeAd = pu.personelAtama.Sube != null ? pu.personelAtama.Sube.Ad : null,
                    DepartmanId = pu.personelAtama.DepartmanId,
                    DepartmanAd = pu.personelAtama.Departman != null ? pu.personelAtama.Departman.Ad : null,
                    PozisyonId = pu.personelAtama.PozisyonId,
                    PozisyonAd = pu.personelAtama.Pozisyon != null ? pu.personelAtama.Pozisyon.Ad : null,
                    Iletisim = pu.personelAtama.Personel!.Iletisim,
                    Adres = pu.personelAtama.Personel.Adres,
                    Yonetici = pu.personelAtama.Yonetici != null ? $"{pu.personelAtama.Yonetici.FullName}" : null,
                    YoneticiPozisyon = "",
                    Role = pu.personelAtama.RolTipi.Value,
                    UserId = pu.personelAtama.Personel.UserId,
                    CalismaTakvimiId = pu.personelAtama.CalismaTakvimId,
                    //IzinKuralId = pu.personelAtama.IzinKuralId!,
                    SozlesmeTuruValue = pu.personelAtama.SozlesmeTuru.Value,
                    SozlesmeBitisTarihi = pu.personelAtama.SozlesmeBitisTarihi,
                    IsActive = pu.personelAtama.Personel.IsActive,
                    CreatedAt = pu.personelAtama.Personel.CreatedAt,
                    CreateUserId = pu.createUser.Id,
                    CreateUserName = pu.createUser.FirstName + " " + pu.createUser.LastName + " (" + pu.createUser.Email + ")",
                    UpdateAt = pu.personelAtama.Personel.UpdateAt,
                    UpdateUserId = null,
                    UpdateUserName = null,
                    IsDeleted = pu.personelAtama.Personel.IsDeleted,
                    DeleteAt = pu.personelAtama.Personel.DeleteAt
                });

        return Task.FromResult(response);
    }
}
