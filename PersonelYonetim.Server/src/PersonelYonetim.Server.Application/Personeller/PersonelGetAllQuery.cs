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
    public Guid? PersonelGorevlendirmeId { get; set; }
    public string Ad { get; set; } = default!;
    public string Soyad { get; set; } = default!;
    public DateTimeOffset DogumTarihi { get; set; }
    public string? AvatarUrl { get; set; }
    public bool? Cinsiyet { get; set; }
    public Iletisim Iletisim { get; set; } = default!;
    public Adres? Adres { get; set; }

    public DateTimeOffset IseGirisTarihi { get; set; }
    public DateTimeOffset? IstenCikisTarihi { get; set; }
    public DateTimeOffset PozisyonBaslangicTarihi { get; set; }
    public DateTimeOffset? PozisyonBitisTarihi { get; set; }

    public Guid? RaporlananGorevlendirmeId { get; set; }
    public string? YoneticiAd { get; set; }
    public string? YoneticiPozisyon { get; set; }

    public Guid? KurumsalBirimId { get; set; }
    public string? KurumsalBirimAd { get; set; } 

    public Guid? PozisyonId { get; set; }
    public string? PozisyonAd { get; set; }

    public decimal BrutUcret { get; set; }
    
    public Guid CalismaTakvimiId { get; set; }

    public int? GorevlendirmeTipiValue { get; set; }
    public int? CalismaSekliValue { get; set; }

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
            .Where(pg => request.KurumsalBirimId != null ? pg.personelGorevlendirme.KurumsalBirimId == request.KurumsalBirimId : true)
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
                        PersonelGorevlendirmeId = ppuu.personelGorevlendirme.Id,

                        Ad = ppuu.personel.Ad,
                        Soyad = ppuu.personel.Soyad,
                        DogumTarihi = ppuu.personel.DogumTarihi,
                        AvatarUrl = ppuu.personel.AvatarUrl,
                        Cinsiyet = ppuu.personel.Cinsiyet != null ? ppuu.personel.Cinsiyet.Value : null,
                        Iletisim = ppuu.personel.Iletisim,
                        Adres = ppuu.personel.Adres,

                        IseGirisTarihi = ppuu.personelGorevlendirme.IseGirisTarihi,
                        IstenCikisTarihi = ppuu.personelGorevlendirme.IstenCikisTarihi,
                        PozisyonBaslangicTarihi = ppuu.personelGorevlendirme.PozisyonBaslangicTarihi,
                        PozisyonBitisTarihi = ppuu.personelGorevlendirme.PozisyonBitisTarihi,

                        RaporlananGorevlendirmeId = ppuu.personelGorevlendirme.RaporlananGorevlendirmeId,
                        YoneticiAd = ppuu.personelGorevlendirme.RaporlananGorevlendirme != null ? ppuu.personelGorevlendirme.RaporlananGorevlendirme.Personel.FullName : "Bilinmiyor",
                        YoneticiPozisyon = ppuu.personelGorevlendirme.RaporlananGorevlendirme != null ? ppuu.personelGorevlendirme.RaporlananGorevlendirme.Pozisyon!.Ad : "Bilinmiyor",

                        KurumsalBirimId = ppuu.personelGorevlendirme.KurumsalBirimId != null ? ppuu.personelGorevlendirme.KurumsalBirimId : null,
                        KurumsalBirimAd = ppuu.personelGorevlendirme.KurumsalBirim != null ? ppuu.personelGorevlendirme.KurumsalBirim.Ad : "Bilinmiyor",

                        PozisyonId = ppuu.personelGorevlendirme.PozisyonId,
                        PozisyonAd = ppuu.personelGorevlendirme.Pozisyon != null ? ppuu.personelGorevlendirme.Pozisyon.Ad : "Bilinmiyor",
                        
                        Roller = ppuu.personelGorevlendirme.GorevlendirmeRolleri.Select(r => r.Rol.Name).ToArray(),

                        BrutUcret = ppuu.personelGorevlendirme.BrutUcret,

                        CalismaTakvimiId = ppuu.personelGorevlendirme.CalismaTakvimId,

                        CalismaSekliValue = ppuu.personelGorevlendirme.CalismaSekli != null ? ppuu.personelGorevlendirme.CalismaSekli.Value : null,
                        GorevlendirmeTipiValue = ppuu.personelGorevlendirme.GorevlendirmeTipi != null ? ppuu.personelGorevlendirme.GorevlendirmeTipi.Value : null,

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
