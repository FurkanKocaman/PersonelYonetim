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
    //public Guid? DegerlendirenId { get; set; }
    //public string? DegerlendirenAd { get; set; }
}
internal sealed class IzinTalepGetQueryHandler(
    IIzinTalepRepository izinTalepRepository,
    UserManager<AppUser> userManager,
    IPersonelRepository personelRepository,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<IzinTalepGetQuery, IQueryable<IzinTalepGetQueryResponse>>
{
    public Task<IQueryable<IzinTalepGetQueryResponse>> Handle(IzinTalepGetQuery request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");

        var personel = personelRepository.GetAll()
            .Where(p => p.UserId == Guid.Parse(userIdString) && !p.IsDeleted)
            .Select(p => new { p.Id })
            .FirstOrDefault();

        if (personel is null)
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");

        var users = userManager.Users.AsQueryable();

        var response = from entity in izinTalepRepository.Where(i => i.PersonelId == personel.Id && !i.IsDeleted)
                       join createUser in users on entity.CreateUserId equals createUser.Id into createGroup
                       from createUser in createGroup.DefaultIfEmpty()

                       join updateUser in users on entity.UpdateUserId equals updateUser.Id into updateGroup
                       from updateUser in updateGroup.DefaultIfEmpty()

                       //join degerlendirenUser in users on entity.DegerlendirenId equals degerlendirenUser.Id into degerlendirenGroup
                       //from degerlendirenUser in degerlendirenGroup.DefaultIfEmpty()

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
                           Aciklama = entity.Aciklama,
                           //DegerlendirmeDurumu = entity.DegerlendirmeDurumu.Name,
                           //DegerlendirenId = entity.DegerlendirenId,
                           //DegerlendirenAd = degerlendirenUser != null
                           //    ? $"{degerlendirenUser.FirstName} {degerlendirenUser.LastName} ({degerlendirenUser.Email})"
                           //    : null,
                           IsActive = entity.IsActive,
                           CreatedAt = entity.CreatedAt,
                           CreateUserId = createUser.Id,
                           CreateUserName = createUser != null
                               ? $"{createUser.FirstName} {createUser.LastName} ({createUser.Email})"
                               : null,
                           UpdateAt = entity.UpdateAt,
                           UpdateUserId = updateUser.Id,
                           UpdateUserName = updateUser != null
                               ? $"{updateUser.FirstName} {updateUser.LastName} ({updateUser.Email})"
                               : null,
                           IsDeleted = entity.IsDeleted,
                           DeleteAt = entity.DeleteAt,
                       };

        return Task.FromResult(response);
    }
}
