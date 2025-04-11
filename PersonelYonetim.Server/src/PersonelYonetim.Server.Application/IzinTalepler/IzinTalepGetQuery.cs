using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.IzinTalepler;

public sealed record IzinTalepGetQuery() : IRequest<IQueryable<IzinTalepGetQueryResponse>>;

public sealed class IzinTalepGetQueryResponse : EntityDto
{
    public Guid PersonelId { get; set; }
    public string PersonelFullName { get; set; } = default!;
    public DateTimeOffset BaslangicTarihi { get; set; }
    public DateTimeOffset BitisTarihi { get; set; }
    public DateTimeOffset MesaiBaslangicTarihi { get; set; }
    public decimal ToplamSure { get; set; }
    public string IzinTuru { get; set; } = default!;
    public string? Aciklama { get; set; }
    public string DegerlendirmeDurumu { get; set; } = default!;
    public Guid? DegerlendirenId { get; set; }
    public string? DegerlendirenAd { get; set; }
}

internal sealed class IzinTalepGetQueryHandler(
    IIzinTalepRepository izinTalepRepository,
    UserManager<AppUser> userManager,
    IPersonelRepository personelRepository,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<IzinTalepGetQuery, IQueryable<IzinTalepGetQueryResponse>>
{
    public Task<IQueryable<IzinTalepGetQueryResponse>> Handle(IzinTalepGetQuery request, CancellationToken cancellationToken)
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

        if (personel is null)
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");

        var response = (from entity in izinTalepRepository.Where(i => i.PersonelId == personel.Id && !i.IsDeleted)
                        join onay_user in userManager.Users.AsQueryable() on entity.Id equals onay_user.Id
                        into onay_user
                        from onay_users in onay_user.DefaultIfEmpty()
                        join create_user in userManager.Users.AsQueryable() on entity.CreateUserId equals create_user.Id
                        join update_user in userManager.Users.AsQueryable() on entity.UpdateUserId equals update_user.Id
                        into update_user
                        from update_users in update_user.DefaultIfEmpty()
                        select new IzinTalepGetQueryResponse
                        {
                            Id = entity.Id,
                            PersonelId = entity.PersonelId,
                            PersonelFullName = entity.Personel.FullName,
                            BaslangicTarihi = entity.BaslangicTarihi,
                            BitisTarihi = entity.BitisTarihi,
                            MesaiBaslangicTarihi = entity.MesaiBaslangicTarihi,
                            ToplamSure = entity.ToplamSure,
                            IzinTuru = entity.IzinTur.Ad,
                            Aciklama = entity.Aciklama!,
                            DegerlendirmeDurumu = entity.DegerlendirmeDurumu.Name!,
                            //DegerlendirenId = entity.DegerlendirenId,
                            //DegerlendirenAd = entity.Degerlendiren != null ? $"{entity.Degerlendiren!.Ad} {entity.Degerlendiren!.Soyad}" : null,
                            IsActive = entity.IsActive,
                            CreatedAt = entity.CreatedAt,
                            CreateUserId = create_user.Id,
                            CreateUserName = create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",
                            UpdateAt = entity.UpdateAt,
                            UpdateUserId = update_users.Id,
                            UpdateUserName = entity.UpdateUserId == null ? null : update_users.FirstName + " " + update_users.LastName + " (" + update_users.Email + ")",
                            IsDeleted = entity.IsDeleted,
                            DeleteAt = entity.DeleteAt,
                        });

        return Task.FromResult(response);
    }
}
