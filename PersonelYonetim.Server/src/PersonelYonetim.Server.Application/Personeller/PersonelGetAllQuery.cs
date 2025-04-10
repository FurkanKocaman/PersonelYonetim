using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.Personeller;

public sealed record PersonelGetAllQuery(
    Guid SirketId,
    Guid? SubeId,
    Guid? DepartmanId,
    Guid? PozisyonId) : IRequest<IQueryable<PersonelGetAllQueryResponse>> ;

public sealed class PersonelGetAllQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string Soyad { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public DateTimeOffset DogumTarihi { get; set; }
     public bool? Cinsiyet { get; set; }
    public string SirketAd { get; set; } = default!;
    public Guid SirketId { get; set; } = default!;
    public string? SubeAd { get; set; }
    public Guid? SubeId { get; set; }
    public string? DepartmanAd { get; set; }
    public Guid? DepartmanId { get; set; }
    public string? PozisyonAd { get; set; }
    public Guid? PozisyonId { get; set; }
    public int Role { get; set; }
    //public string ErisimSekli { get; set; } = "aa";
    public Guid? YoneticiId { get; set; }
    public int SozlesmeTuruValue { get; set; }
    public DateTimeOffset PozisyonBaslangicTarih {  get; set; }
    public Iletisim Iletisim { get; set; } = default!;
    public Adres Adres { get; set; } = default!;
}

internal sealed class PersonelGetAllQueryHandler(
    IPersonelRepository personelRepository,
    IPersonelAtamaRepository personelAtamaRepository,
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


        var response = personelRepository.GetAll()
            .Where(p => !p.IsDeleted)
            .Join(personelAtamaRepository.GetAll(),
                    personel => personel.Id,
                    personelAtama => personelAtama.PersonelId,
                    (personel, personelAtama) => new { personel, personelAtama })
            .Where(pp => !pp.personelAtama.IsDeleted &&
                    pp.personelAtama.SirketId == request.SirketId &&
                    (request.SubeId == null || pp.personelAtama.SubeId == request.SubeId) &&
                    (request.DepartmanId == null || pp.personelAtama.DepartmanId == request.DepartmanId) &&
                    (request.PozisyonId == null || pp.personelAtama.PozisyonId == request.PozisyonId))
            .Join(userManager.Users,
                  pp => pp.personel.CreateUserId,
                  createUser => createUser.Id,
                  (pp, createUser) => new { pp.personel, pp.personelAtama, createUser })
            .GroupJoin(userManager.Users,
                  ppu => ppu.personel.UpdateUserId,
                  updateUser => updateUser.Id,
                  (ppu, updateUsers) => new { ppu.personel, ppu.personelAtama, ppu.createUser, updateUsers })
            .SelectMany(
                  ppuu => ppuu.updateUsers.DefaultIfEmpty(),
                  (ppuu, updateUser) => new PersonelGetAllQueryResponse
                  {
                      Id = ppuu.personel.Id,
                      Ad = ppuu.personel.Ad,
                      Soyad = ppuu.personel.Soyad,
                      FullName = ppuu.personel.FullName,
                      DogumTarihi = ppuu.personel.DogumTarihi,
                      Cinsiyet = ppuu.personel.Cinsiyet,
                      SirketId = ppuu.personelAtama.SirketId,
                      SirketAd = ppuu.personelAtama.Sirket!.Ad,
                      SubeId = ppuu.personelAtama.SubeId,
                      SubeAd = ppuu.personelAtama.Sube!.Ad,
                      DepartmanId = ppuu.personelAtama.DepartmanId,
                      DepartmanAd = ppuu.personelAtama.Departman!.Ad,
                      PozisyonId = ppuu.personelAtama.PozisyonId,
                      PozisyonAd = ppuu.personelAtama.Pozisyon!.Ad,
                      Role = ppuu.personelAtama.RolTipi.Value,
                      YoneticiId = ppuu.personelAtama.YoneticiId,
                      SozlesmeTuruValue = ppuu.personelAtama.SozlesmeTuru.Value,
                      PozisyonBaslangicTarih = ppuu.personelAtama.PozisyonBaslamaTarihi,
                      Iletisim = ppuu.personel.Iletisim,
                      Adres = ppuu.personel.Adres,
                      IsActive = ppuu.personel.IsActive,
                      CreatedAt = ppuu.personel.CreatedAt,
                      CreateUserId = ppuu.createUser.Id,
                      CreateUserName = ppuu.createUser.FirstName + " " + ppuu.createUser.LastName + " (" + ppuu.createUser.Email + ")",
                      UpdateAt = ppuu.personel.UpdateAt,
                      UpdateUserId = updateUser != null ? updateUser.Id : null,
                      UpdateUserName = updateUser != null
                          ? updateUser.FirstName + " " + updateUser.LastName + " (" + updateUser.Email + ")"
                          : null,
                      IsDeleted = ppuu.personel.IsDeleted,
                      DeleteAt = ppuu.personel.DeleteAt
                  });


        return Task.FromResult(response);
    }
}
