using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.KurumsalBirimTipleri;
public sealed record KurumsalBirimTipiGetAllQuery(
    ) : IRequest<IQueryable<KurumsalBirimTipiGetAllQueryResponse>>;

public sealed class KurumsalBirimTipiGetAllQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public int HiyerarsiSeviyesi { get; set; }
    public bool YoneticisiOlabilirMi { get; set; } = false;
}

internal sealed class KurumsalBirimTipiGetAllQueryHandler(
    IKurumsalBirimTipiRepository kurumsalBirimTipiRepository,
    UserManager<AppUser> userManager,
    IHttpContextAccessor httpContextAccessor,
    IPersonelRepository personelRepository
    ) : IRequestHandler<KurumsalBirimTipiGetAllQuery, IQueryable<KurumsalBirimTipiGetAllQueryResponse>>
{
    public Task<IQueryable<KurumsalBirimTipiGetAllQueryResponse>> Handle(KurumsalBirimTipiGetAllQuery request, CancellationToken cancellationToken)
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

        var response = kurumsalBirimTipiRepository.Where(p => p.TenantId == personel.TenantId).OrderBy(p => p.HiyerarsiSeviyesi)
                 .GroupJoin(userManager.Users,
                    birimTipi => birimTipi.CreateUserId,
                    createUser => createUser.Id,
                    (birimTipi, createUsers) => new { birimTipi, createUsers })
                  .SelectMany(
                    ku => ku.createUsers.DefaultIfEmpty(),
                    (ku, createUser) => new { ku.birimTipi, createUser })
                  .GroupJoin(userManager.Users,
                    ku => ku.birimTipi.UpdateUserId,
                    updateUser => updateUser.Id,
                    (ku, updateUsers) => new { ku.birimTipi, ku.createUser, updateUsers })
                  .SelectMany(
                    kuu => kuu.updateUsers.DefaultIfEmpty(),
                    (kuu, updateUser) => new KurumsalBirimTipiGetAllQueryResponse
                    {
                        Id = kuu.birimTipi.Id,
                        Ad = kuu.birimTipi.Ad,
                        Aciklama = kuu.birimTipi.Aciklama,
                        HiyerarsiSeviyesi = kuu.birimTipi.HiyerarsiSeviyesi,
                        YoneticisiOlabilirMi = kuu.birimTipi.YoneticisiOlabilirMi,
                        IsActive = kuu.birimTipi.IsActive,
                        CreatedAt = kuu.birimTipi.CreatedAt,
                        CreateUserId = kuu.createUser != null ? kuu.createUser.Id : Guid.Empty,
                        CreateUserName = kuu.createUser != null ? kuu.createUser.FirstName + " " + kuu.createUser.LastName + " (" + kuu.createUser.Email + ")" : "Bilinmiyor",
                        UpdateAt = kuu.birimTipi.UpdateAt,
                        UpdateUserId = null,
                        UpdateUserName = null,
                        IsDeleted = kuu.birimTipi.IsDeleted,
                        DeleteAt = kuu.birimTipi.DeleteAt
                    });
        return Task.FromResult(response);
    }
}

