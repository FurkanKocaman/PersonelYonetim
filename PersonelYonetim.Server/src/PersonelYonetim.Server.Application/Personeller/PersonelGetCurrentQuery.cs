using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public string Role { get; set;} = default!;
}

public sealed class PersonelGetCurrentQueryHandler(
    IPersonelRepository personelRepository,
    IDepartmanRepository departmanRepository,
    IPozisyonRepository pozisyonRepository,
    IPersonelAtamaRepository personelAtamaRepository,
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

        var response = (from entity in personelRepository.GetAll()
                        where entity.UserId == Guid.Parse(userIdString)
                        join personel_atama in personelAtamaRepository.GetAll() on entity.Id equals personel_atama.PersonelId
                        join departman in departmanRepository.GetAll() on personel_atama.DepartmanId equals departman.Id
                        into departman from departmans in departman.DefaultIfEmpty()
                        join pozisyon in pozisyonRepository.GetAll() on personel_atama.PozisyonId equals pozisyon.Id
                        into pozisyon from pozisyons in pozisyon.DefaultIfEmpty()
                        join create_user in userManager.Users.AsQueryable() on entity.CreateUserId equals create_user.Id
                        into create_user from create_users in create_user.DefaultIfEmpty()
                        join update_user in userManager.Users.AsQueryable() on entity.UpdateUserId equals update_user.Id
                        into update_user from update_users in update_user.DefaultIfEmpty()
                        join userRole in userRoleRepository.GetAll() on entity.UserId equals userRole.UserId
                        join rol in roleManager.Roles.AsQueryable() on userRole.RoleId equals rol.Id
                        select new PersonelGetCurrentQueryResponse
                        {
                            Id = entity.Id,
                            FullName = entity.FullName,
                            Cinsiyet = entity.Cinsiyet == null ? null : entity.Cinsiyet == true ? "Erkek" : "Kadın",
                            Eposta = entity.Iletisim.Eposta,
                            Telefon = entity.Iletisim.Telefon,
                            DogumTarihi = entity.DogumTarihi,
                            IseGirisTarihi = entity.IseGirisTarihi,
                            ProfilResimUrl = entity.ProfilResimUrl,
                            Ulke = entity.Adres.Ulke,
                            Sehir = entity.Adres.Sehir,
                            Ilce = entity.Adres.Ilce,
                            TamAdres = entity.Adres.TamAdres,
                            DepartmanAd = departmans != null ? departmans.Ad : "",
                            PozisyonAd = pozisyons != null ? pozisyons.Ad : "",
                            Yonetici = entity.Yonetici != null ? entity.Yonetici!.FullName : "",
                            IsActive = entity.IsActive,
                            CreatedAt = entity.CreatedAt,
                            CreateUserId = create_users != null ? create_users.Id : entity.Id,
                            CreateUserName = create_users != null ? $"{create_users.FirstName} {create_users.LastName} ({create_users.Email})" : "",
                            UpdateAt = entity.UpdateAt,
                            UpdateUserId = update_users != null ? update_users.Id : null,
                            UpdateUserName = update_users != null ? $"{update_users.FirstName} {update_users.LastName} ({update_users.Email})" : null,
                            IsDeleted = entity.IsDeleted,
                            DeleteAt = entity.DeleteAt,
                            Role = rol.Name!,
                        });

        return Task.FromResult(response);
    }
}
