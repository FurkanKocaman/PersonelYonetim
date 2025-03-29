using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.IzinTalepler;

public sealed record IzinTalepGetAllQuery() : IRequest<IQueryable<IzinTalepGetAllQueryResponse>>;

public sealed class IzinTalepGetAllQueryResponse
{
    public Guid Id { get; set; }
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
            .Where(p => p.UserId == Guid.Parse(userIdString))
            .Select(p => new { p.Id })
            .FirstOrDefault();

        if (personel is null)
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");


        var response = (from entity in izinTalepRepository.Where(i => i.Personel.PersonelAtamalar.Any(p => p.YoneticiId == personel.Id))
                        join onay_user in userManager.Users.AsQueryable() on entity.DegerlendirenId equals onay_user.Id
                        into onay_user
                        from onay_users in onay_user.DefaultIfEmpty()
                        select new IzinTalepGetAllQueryResponse
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
                            DegerlendirenId = entity.DegerlendirenId,
                            DegerlendirenAd = entity.Degerlendiren != null ? entity.Degerlendiren.Ad : null,
                        });

        return Task.FromResult(response);
    }
}
