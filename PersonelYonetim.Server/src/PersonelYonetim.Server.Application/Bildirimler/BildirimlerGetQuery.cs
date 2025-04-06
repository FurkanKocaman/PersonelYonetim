using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.Bildirimler;
public sealed record BildirimlerGetQuery(
    ) : IRequest<IQueryable<BildirimlerGetQueryResponse>>;

public sealed class BildirimlerGetQueryResponse: EntityDto
{
    public string Baslik { get; set; } = string.Empty;
    public string Aciklama { get; set; } = string.Empty;
    public string BildirimTipi { get; set; } = default!;
    public string AliciTipi { get; set; } = default!;
    public string? Url { get; set; }
    public bool OkunduMu { get; set; } = false;
    public DateTimeOffset? OkunmaTarihi { get; set; }
}

internal sealed class BildirimlerGetQueryHandler(
    IPersonelRepository personelRepository,
    IHttpContextAccessor httpContextAccessor,
    IPersonelBildirimRepository personelBildirimRepository,
    UserManager<AppUser> userManager
    ) : IRequestHandler<BildirimlerGetQuery, IQueryable<BildirimlerGetQueryResponse>>
{
    public Task<IQueryable<BildirimlerGetQueryResponse>> Handle(BildirimlerGetQuery request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }

        var personel = personelRepository.GetAll()
            .Where(p => p.UserId == Guid.Parse(userIdString) && !p.IsDeleted)
            .Select(p => new { p.Id })
            .FirstOrDefault();

        if (personel == null)
        {
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
        }

        var bildirimler = personelBildirimRepository.Where(p => p.PersonelId == personel.Id).OrderBy(p => p.CreatedAt).Include(p => p.Bildirim);

        var response = bildirimler
                    .Join(userManager.Users,
                    bildirim => bildirim.CreateUserId,
                    createUser => createUser.Id,
                    (bildirim, createUser) => new { bildirim, createUser })
                    .Select(p => new BildirimlerGetQueryResponse
                    {
                        Id = p.bildirim.Bildirim.Id,
                        Baslik = p.bildirim.Bildirim.Baslik,
                        Aciklama = p.bildirim.Bildirim.Aciklama,
                        BildirimTipi = p.bildirim.Bildirim.BildirimTipi.Name,
                        AliciTipi = p.bildirim.Bildirim.AliciTipi.Name,
                        Url = p.bildirim.Bildirim.Url,
                        OkunduMu = p.bildirim.OkunduMu,
                        OkunmaTarihi = p.bildirim.OkunmaTarihi,
                        IsActive = p.bildirim.IsActive,
                        CreatedAt = p.bildirim.CreatedAt,
                        CreateUserId = p.bildirim.CreateUserId,
                        CreateUserName = p.createUser.FirstName + " " + p.createUser.LastName + " (" + p.createUser.Email + ")",
                        IsDeleted = p.bildirim.IsDeleted,
                        DeleteAt = p.bildirim.DeleteAt
                    });

        return Task.FromResult(response);
    }
}
