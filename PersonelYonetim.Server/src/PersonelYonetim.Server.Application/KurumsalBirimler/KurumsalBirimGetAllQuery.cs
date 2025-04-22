using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.KurumsalBirimler;
public sealed record KurumsalBirimGetAllQuery(
    ) : IRequest<IQueryable<KurumsalBirimGetAllQueryResponse>>;

public sealed class KurumsalBirimGetAllQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string? Kod { get; set; }
    public Guid BirimTipiId { get; set; }
    public string BirimTipiAd { get; set; } = default!;
}

internal sealed class KurumsalBirimGetAllQueryhandler(
    IKurumsalBirimRepository kurumsalBirimRepository,
    UserManager<AppUser> userManager,
    IHttpContextAccessor httpContextAccessor,
    IPersonelRepository personelRepository

    ) : IRequestHandler<KurumsalBirimGetAllQuery, IQueryable<KurumsalBirimGetAllQueryResponse>>
{
    public Task<IQueryable<KurumsalBirimGetAllQueryResponse>> Handle(KurumsalBirimGetAllQuery request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }

        var personel = personelRepository
            .Where(p => p.UserId == Guid.Parse(userIdString) && !p.IsDeleted).Select(p => new { p.TenantId }).FirstOrDefault();

        if (personel == null)
        {
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
        }


        var response = kurumsalBirimRepository.Where(p => p.TenantId == personel.TenantId && !p.IsDeleted).OrderBy(p => p.BirimTipi.HiyerarsiSeviyesi)
                .GroupJoin(userManager.Users,
                    birim => birim.CreateUserId,
                    createUser => createUser.Id,
                    (birim, createUsers) => new { birim, createUsers })
                  .SelectMany(
                    ku => ku.createUsers.DefaultIfEmpty(),
                    (ku, createUser) => new { ku.birim, createUser })
                  .GroupJoin(userManager.Users,
                    ku => ku.birim.UpdateUserId,
                    updateUser => updateUser.Id,
                    (ku, updateUsers) => new { ku.birim, ku.createUser, updateUsers })
                  .SelectMany(
                    kuu => kuu.updateUsers.DefaultIfEmpty(),
                    (kuu, updateUser) => new KurumsalBirimGetAllQueryResponse
                    {
                        Id = kuu.birim.Id,
                        Ad = kuu.birim.Ad,
                        Kod = kuu.birim.Kod,
                        BirimTipiId = kuu.birim.BirimTipiId,
                        BirimTipiAd = kuu.birim.BirimTipi != null ? kuu.birim.BirimTipi.Ad : "Bilinmiyor",
                        IsActive = kuu.birim.IsActive,
                        CreatedAt = kuu.birim.CreatedAt,
                        CreateUserId = kuu.createUser != null ? kuu.createUser.Id : Guid.Empty,
                        CreateUserName = kuu.createUser != null ? kuu.createUser.FirstName + " " + kuu.createUser.LastName + " (" + kuu.createUser.Email + ")" : "Bilinmiyor",
                        UpdateAt = kuu.birim.UpdateAt,
                        UpdateUserId = null,
                        UpdateUserName = null,
                        IsDeleted = kuu.birim.IsDeleted,
                        DeleteAt = kuu.birim.DeleteAt
                    });
        return Task.FromResult(response);
    }
}
