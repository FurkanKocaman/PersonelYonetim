using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Users;

namespace PersonelYonetim.Server.Application.PersonelGorevlendirmeler;
public sealed record PersonelGorevlendirmeGetAllQuery(
    Guid? TenantId
    ):IRequest<IQueryable<PersonelGorevlendirmeGetAllQueryResponse>>;

public sealed class PersonelGorevlendirmeGetAllQueryResponse : EntityDto
{
    public Guid PersonelId { get; set; }
    public string PersonelFullName { get; set; } = string.Empty;
    public Guid? KurumsalBirimId { get; set; }
    public string? KurumsalBirimAd { get; set; }
    public Guid? PozisyonId { get; set; }
    public string? PozisyonAd { get; set; }

    public DateTimeOffset IseGirisTarihi { get; set; }
    public DateTimeOffset? IstenCikisTarihi { get; set; }

    public DateTimeOffset PozisyonBaslangicTarihi { get; set; }
    public DateTimeOffset? PozisyonBitisTarihi { get; set; }

    public string BirincilGorevMi { get; set; } = string.Empty;
    public string? GorevlendirmeTipiAd { get; set; }
    public string? CalismaSekliAd { get; set; }

    public List<string?> GorevlendirmeRolleriAd { get; set; } = new List<string?>();
    public string? IzinKuralAd { get; set; }

    public Guid? RaporlananPersonelId { get; set; }
    public string? RaporlananPersonelAd { get; set; }

    public string? MesaiOnaySurecAd { get; set; }

    public string? CalismaTakvimiAd { get; set; }

    public decimal BrutUcret { get; set; }
    public string? SGKIsyeri { get; set; }
    public string? SGKNumarasi { get; set; }
    public string? VergiDairesiAdi { get; set; }
    public string? VergiNumarasi { get; set; }
    public string? TabiOlduguKanun { get; set; }
    public string? MeslekKodu { get; set; }
}

internal sealed class PersonelGorevlendirmeGetAllQueryHandler(
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    UserManager<AppUser> userManager,
    ICurrentUserService currentUserService
    ) : IRequestHandler<PersonelGorevlendirmeGetAllQuery, IQueryable<PersonelGorevlendirmeGetAllQueryResponse>>
{
    public  Task<IQueryable<PersonelGorevlendirmeGetAllQueryResponse>> Handle(PersonelGorevlendirmeGetAllQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        Guid? userId = currentUserService.UserId;

        if (!tenantId.HasValue || !userId.HasValue)
            return Task.FromResult(Enumerable.Empty<PersonelGorevlendirmeGetAllQueryResponse>().AsQueryable());

        var personelGorevlendirme = personelGorevlendirmeRepository.Where(p => p.Personel.UserId == userId)
            .Include(p => p.Personel)
            .Include(p => p.GorevlendirmeRolleri).ThenInclude(p => p.Rol)
            .Include(p => p.GorevlendirmeIzinKurali).ThenInclude(p => p!.IzinKural)
            .Include(p => p.KurumsalBirim)
            .Include(p => p.Pozisyon);

        var response = personelGorevlendirme
                    .GroupJoin(userManager.Users,
                        personelGorevlendirme => personelGorevlendirme.CreateUserId,
                        createUser => createUser.Id,
                        (personelGorevlendirme, createUsers) => new { personelGorevlendirme, createUsers })
                    .SelectMany(
                        pu => pu.createUsers.DefaultIfEmpty(),
                        (pu, createUser) => new { pu.personelGorevlendirme, createUser })
                    .GroupJoin(userManager.Users,
                        pu => pu.personelGorevlendirme.UpdateUserId,
                        updateUser => updateUser.Id,
                        (pu, updateUsers) => new { pu.personelGorevlendirme, pu.createUser, updateUsers })
                    .SelectMany(
                        pu => pu.updateUsers.DefaultIfEmpty(),
                        (pu, updateUser) => new PersonelGorevlendirmeGetAllQueryResponse
                        {
                            Id = pu.personelGorevlendirme.Id,
                            PersonelId = pu.personelGorevlendirme.Personel.Id,
                            PersonelFullName = pu.personelGorevlendirme.Personel.FullName,
                            KurumsalBirimId = pu.personelGorevlendirme.KurumsalBirimId,
                            KurumsalBirimAd = pu.personelGorevlendirme.KurumsalBirim != null ? pu.personelGorevlendirme.KurumsalBirim.Ad : "Bulunamamdı",
                            PozisyonId = pu.personelGorevlendirme.PozisyonId,
                            PozisyonAd = pu.personelGorevlendirme.Pozisyon != null ? pu.personelGorevlendirme.Pozisyon.Ad : "Bulunamamdı",

                            IseGirisTarihi = pu.personelGorevlendirme.IseGirisTarihi,
                            IstenCikisTarihi = pu.personelGorevlendirme.IstenCikisTarihi,
                            PozisyonBaslangicTarihi = pu.personelGorevlendirme.PozisyonBaslangicTarihi,
                            PozisyonBitisTarihi = pu.personelGorevlendirme.PozisyonBitisTarihi,

                            BirincilGorevMi = pu.personelGorevlendirme.BirincilGorevMi ? "Evet" : "Hayır",
                            GorevlendirmeTipiAd = pu.personelGorevlendirme.GorevlendirmeTipi != null ? pu.personelGorevlendirme.GorevlendirmeTipi.Name : "Bulunammadı",
                            CalismaSekliAd = pu.personelGorevlendirme.CalismaSekli != null ? pu.personelGorevlendirme.CalismaSekli.Name : "Bulunamadı",
                            
                            GorevlendirmeRolleriAd = pu.personelGorevlendirme.GorevlendirmeRolleri.Select(p => p.Rol.Name).ToList(),
                            IzinKuralAd = pu.personelGorevlendirme.GorevlendirmeIzinKurali != null ? pu.personelGorevlendirme.GorevlendirmeIzinKurali.IzinKural.Ad:"Bulunammadı",

                            RaporlananPersonelId = pu.personelGorevlendirme.Id,
                            RaporlananPersonelAd = pu.personelGorevlendirme.Personel.Ad,
                           
                            MesaiOnaySurecAd = pu.personelGorevlendirme.MesaiOnaySurec != null ? pu.personelGorevlendirme.MesaiOnaySurec.Ad : "Bulunamamdı",
                            CalismaTakvimiAd = pu.personelGorevlendirme.CalismaTakvimi != null ? pu.personelGorevlendirme.CalismaTakvimi.Ad :"Bulunammadı",

                            BrutUcret = pu.personelGorevlendirme.BrutUcret,
                            SGKIsyeri = pu.personelGorevlendirme.SGKIsyeri,
                            SGKNumarasi = pu.personelGorevlendirme.SGKNumarasi,
                            VergiDairesiAdi = pu.personelGorevlendirme.VergiDairesiAdi,
                            VergiNumarasi = pu.personelGorevlendirme.VergiNumarasi,
                            TabiOlduguKanun = pu.personelGorevlendirme.TabiOlduguKanun,
                            MeslekKodu = pu.personelGorevlendirme.MeslekKodu,

                            IsActive = pu.personelGorevlendirme.IsActive,
                            CreatedAt = pu.personelGorevlendirme.CreatedAt,
                            CreateUserId = pu.createUser != null ? pu.createUser.Id : null,
                            CreateUserName = pu.createUser != null ? pu.createUser.FirstName + " " + pu.createUser.LastName + " (" + pu.createUser.Email + ")" : null,
                            UpdateAt = pu.personelGorevlendirme.UpdateAt,
                            UpdateUserId = updateUser != null ? updateUser.Id : null,
                            UpdateUserName = updateUser != null ? updateUser.FirstName + " " + updateUser.LastName + " (" + updateUser.Email + ")" : null,
                            IsDeleted = pu.personelGorevlendirme.IsDeleted,
                            DeleteAt =pu.personelGorevlendirme.DeleteAt,

                        }).AsQueryable();

        return Task.FromResult(response);
    }
}
