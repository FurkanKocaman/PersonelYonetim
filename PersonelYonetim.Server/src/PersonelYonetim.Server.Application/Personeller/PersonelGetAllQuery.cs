using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.Personeller;

public sealed record PersonelGetAllQuery(
    Guid? KurumsalBirimId) : IRequest<IQueryable<PersonelGetAllQueryResponse>> ;

public sealed class PersonelGetAllQueryResponse : EntityDto
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
    public string?[] Roller { get; set; } = default!;
}

internal sealed class PersonelGetAllQueryHandler(
    IPersonelRepository personelRepository,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    IHttpContextAccessor httpContextAccessor,
    UserManager<AppUser> userManager
    ) : IRequestHandler<PersonelGetAllQuery, IQueryable<PersonelGetAllQueryResponse>>
{
    public Task<IQueryable<PersonelGetAllQueryResponse>> Handle(PersonelGetAllQuery request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }

        var personel = personelRepository
            .Where(p => p.UserId == Guid.Parse(userIdString) && !p.IsDeleted).Select(p => new {p.TenantId}).FirstOrDefault();

        if (personel == null)
        {
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
        }

        var response = personelRepository
            .Where(p => p.TenantId == personel.TenantId && !p.IsDeleted)
            .Join(personelGorevlendirmeRepository.GetAll(),
                    personel => personel.Id,
                    personelGorevlendirme => personelGorevlendirme.PersonelId,
                    (personel, personelGorevlendirme) => new { personel, personelGorevlendirme })
            .GroupJoin(userManager.Users,
                    pp => pp.personel.CreateUserId,
                    createUser => createUser.Id,
                    (pp, createUsers) => new { pp.personel, pp.personelGorevlendirme, createUsers })
             .SelectMany(
                    ppu => ppu.createUsers.DefaultIfEmpty(),
                    (ppu, createUser) => new { ppu.personel, ppu.personelGorevlendirme, createUser })
             .GroupJoin(userManager.Users,
                    ppu => ppu.personel.UpdateUserId,
                    updateUser => updateUser.Id,
                    (ppu, updateUsers) => new { ppu.personel, ppu.personelGorevlendirme, ppu.createUser, updateUsers })
             .SelectMany(
                    ppuu => ppuu.updateUsers.DefaultIfEmpty(),
                    (ppuu, updateUser) => new PersonelGetAllQueryResponse
                    {
                        Id = ppuu.personel.Id,
                        Ad = ppuu.personel.Ad,
                        Soyad = ppuu.personel.Soyad,
                        DogumTarihi = ppuu.personel.DogumTarihi,
                        AvatarUrl = ppuu.personel.AvatarUrl,
                        Cinsiyet = ppuu.personel.Cinsiyet != null ? ppuu.personel.Cinsiyet.Value ? "Erkek" : "Kadın" : "Bilinmiyor",
                        Iletisim = ppuu.personel.Iletisim,
                        Adres = ppuu.personel.Adres,
                        KurumsalBirimAd = ppuu.personelGorevlendirme.KurumsalBirim != null ? ppuu.personelGorevlendirme.KurumsalBirim.Ad : "Bilinmiyor",
                        PozisyonAd = ppuu.personelGorevlendirme.Pozisyon != null ? ppuu.personelGorevlendirme.Pozisyon.Ad : "Bilinmiyor",
                        YoneticiAd = ppuu.personelGorevlendirme.RaporlananGorevlendirme != null ? ppuu.personelGorevlendirme.RaporlananGorevlendirme.Personel.FullName : "Bilinmiyor",
                        YoneticiPozisyon = ppuu.personelGorevlendirme.RaporlananGorevlendirme != null ? ppuu.personelGorevlendirme.RaporlananGorevlendirme.Pozisyon.Ad : "Bilinmiyor",
                        Roller = ppuu.personelGorevlendirme.GorevlendirmeRolleri.Select(r => r.Rol.Name).ToArray(),
                        IsActive = ppuu.personel.IsActive,
                        CreatedAt = ppuu.personel.CreatedAt,
                        CreateUserId = ppuu.createUser != null ? ppuu.createUser.Id : Guid.Empty,
                        CreateUserName = ppuu.createUser != null ? ppuu.createUser.FirstName + " " + ppuu.createUser.LastName + " (" + ppuu.createUser.Email + ")" : "Bilinmiyor",
                        UpdateAt = ppuu.personel.UpdateAt,
                        UpdateUserId = updateUser != null ? updateUser.Id : Guid.Empty,
                        UpdateUserName = updateUser != null ? updateUser.FirstName + " " + updateUser.LastName + " (" + updateUser.Email + ")" : "Bilinmiyor",
                        IsDeleted = ppuu.personel.IsDeleted,
                        DeleteAt = ppuu.personel.DeleteAt
                    });

        return Task.FromResult(response);
    }
}
