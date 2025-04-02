using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Subeler;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.TakvimEtkinlikler;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.TakvimEtkinlikler;
public sealed record TakvimEtkinlikGetAllQuery () : IRequest<IQueryable<TakvimEtkinlikGetAllQueryResponse>>;

public sealed class TakvimEtkinlikGetAllQueryResponse : EntityDto
{
    public string Baslik { get; set; } = default!;
    public string? Aciklama { get; set; }
    public DateTimeOffset BaslangicTarihi { get; set; } = default!;
    public DateTimeOffset? BitisTarihi { get; set; }
    public Guid SirketId { get; set; } = default!;
    public string SirketAd { get; set; } = default!;
    public bool IsPublic { get; set; } = true;
    public List<KatilimciDto> Katilimcilar { get; set; } = new List<KatilimciDto>();

}
public class KatilimciDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
}

internal sealed class TakvimEtkinlikGetAllQueryHandler(
    IPersonelRepository personelRepository,
    ITakvimEtkinlikRepository takvimEtkinlikRepository,
    UserManager<AppUser> userManager,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<TakvimEtkinlikGetAllQuery, IQueryable<TakvimEtkinlikGetAllQueryResponse>>
{
    public Task<IQueryable<TakvimEtkinlikGetAllQueryResponse>> Handle(TakvimEtkinlikGetAllQuery request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }

        var personel = personelRepository.GetAll()
            .Where(p => p.UserId == Guid.Parse(userIdString) && !p.IsDeleted)
            .Select(p => new { p.Id, p.PersonelAtamalar })
            .FirstOrDefault();

        if (personel == null)
        {
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
        }

        var etkinlikler = takvimEtkinlikRepository.Where(p =>!p.IsDeleted && p.SirketId == personel.PersonelAtamalar.Select(p => new { p.SirketId }).FirstOrDefault()!.SirketId);

        var response = etkinlikler
                .Join(userManager.Users,
                      etkinlik => etkinlik.CreateUserId,
                      createUser => createUser.Id,
                      (etkinlik, createUser) => new { etkinlik, createUser })
                .GroupJoin(userManager.Users,
                      eu => eu.etkinlik.UpdateUserId,
                      updateUser => updateUser.Id,
                      (eu, updateUsers) => new { eu.etkinlik, eu.createUser, updateUsers })
                .SelectMany(
                      euu => euu.updateUsers.DefaultIfEmpty(),
                      (euu, updateUser) => new TakvimEtkinlikGetAllQueryResponse
                      {
                          Id = euu.etkinlik.Id,
                          Baslik = euu.etkinlik.Baslik,
                          Aciklama = euu.etkinlik.Aciklama,
                          SirketId = euu.etkinlik.Sirket.Id,
                          SirketAd = euu.etkinlik.Sirket.Ad,
                          BaslangicTarihi = euu.etkinlik.BaslangicTarihi,
                          BitisTarihi = euu.etkinlik.BitisTarihi,
                          IsPublic = euu.etkinlik.IsPublic,
                          //Katilimcilar = euu.etkinlik.PersonelIdler != null? personelRepository.WhereWithTracking(p => euu.etkinlik.PersonelIdler.Contains(p.Id)).Select(p => new KatilimciDto { Id = p.Id,FullName = p.FullName}).ToList() : new List<KatilimciDto>(),
                          IsActive = euu.etkinlik.IsActive,
                          CreatedAt = euu.etkinlik.CreatedAt,
                          CreateUserId = euu.createUser.Id,
                          CreateUserName = euu.createUser.FirstName + " " + euu.createUser.LastName + " (" + euu.createUser.Email + ")",
                          UpdateAt = euu.etkinlik.UpdateAt,
                          UpdateUserId = updateUser != null ? updateUser.Id : null,
                          UpdateUserName = updateUser != null
                              ? updateUser.FirstName + " " + updateUser.LastName + " (" + updateUser.Email + ")"
                              : null,
                          IsDeleted = euu.etkinlik.IsDeleted,
                          DeleteAt = euu.etkinlik.DeleteAt
                      });

        return Task.FromResult(response);
    }
}
