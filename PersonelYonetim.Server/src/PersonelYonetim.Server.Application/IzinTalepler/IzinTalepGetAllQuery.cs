using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.IzinTalepler;

public sealed record IzinTalepGetAllQuery() : IRequest<IQueryable<IzinTalepGetAllQueryResponse>>;

public sealed class IzinTalepGetAllQueryResponse : EntityDto
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
internal sealed class IzinTalepGetAllQueryHandler(
    IIzinTalepRepository izinTalepRepository,
    UserManager<AppUser> userManager,
    IPersonelRepository personelRepository,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<IzinTalepGetAllQuery, IQueryable<IzinTalepGetAllQueryResponse>>
{
    public Task<IQueryable<IzinTalepGetAllQueryResponse>> Handle(IzinTalepGetAllQuery request, CancellationToken cancellationToken)
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

        var izinler = izinTalepRepository
            .Where(i => i.Personel.PersonelAtamalar.Any(p => p.YoneticiId == personel.Id) && !i.IsDeleted)
            .Include(i => i.Personel)
            .Include(i => i.IzinTur)
            //.Include(i => i.Degerlendiren)
            .AsQueryable();

        var users = userManager.Users.AsQueryable();

        var response = from entity in izinler
                       join onay_user in users on entity.Id equals onay_user.Id into onay_user_join
                       from onay_users in onay_user_join.DefaultIfEmpty()
                       join create_user in users on entity.CreateUserId equals create_user.Id
                       join update_user in users on entity.UpdateUserId equals update_user.Id into update_user_join
                       from update_users in update_user_join.DefaultIfEmpty()
                       select new IzinTalepGetAllQueryResponse
                       {
                           Id = entity.Id,
                           PersonelId = entity.Personel.Id,
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
                           CreateUserName = $"{create_user.FirstName} {create_user.LastName} ({create_user.Email})",
                           UpdateAt = entity.UpdateAt,
                           UpdateUserId = entity.UpdateUserId,
                           UpdateUserName = entity.UpdateUserId == null ? null : $"{update_users.FirstName} {update_users.LastName} ({update_users.Email})",
                           IsDeleted = entity.IsDeleted,
                           DeleteAt = entity.DeleteAt,
                       };

        return Task.FromResult(response);
    }
}

