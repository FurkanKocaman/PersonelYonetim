using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.TakvimEtkinlikler;
using PersonelYonetim.Server.Domain.Users;

namespace PersonelYonetim.Server.Application.TakvimEtkinlikler;
public sealed record TakvimEtkinlikGetAllQuery () : IRequest<IQueryable<TakvimEtkinlikGetAllQueryResponse>>;

public sealed class TakvimEtkinlikGetAllQueryResponse : EntityDto
{
    public string Baslik { get; set; } = default!;
    public string? Aciklama { get; set; }
    public DateTimeOffset BaslangicTarihi { get; set; } = default!;
    public DateTimeOffset? BitisTarihi { get; set; }
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
    ICurrentUserService currentUserService) : IRequestHandler<TakvimEtkinlikGetAllQuery, IQueryable<TakvimEtkinlikGetAllQueryResponse>>
{
    public Task<IQueryable<TakvimEtkinlikGetAllQueryResponse>> Handle(TakvimEtkinlikGetAllQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        Guid? userId = currentUserService.UserId;

        if(!userId.HasValue || !tenantId.HasValue)
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği veya kiracı kimliği bulunamadı.");
        }


        var personel = personelRepository.GetAll()
            .Where(p => p.UserId == userId && !p.IsDeleted)
            .Select(p => new { p.Id })
            .FirstOrDefault();

        if (personel == null)
        {
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
        }

        var etkinlikler = takvimEtkinlikRepository
            .Where(p => !p.IsDeleted && p.TenantId == tenantId)
            .Where(p => p.PersonelIdler != null && p.PersonelIdler.Contains(personel.Id));

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
