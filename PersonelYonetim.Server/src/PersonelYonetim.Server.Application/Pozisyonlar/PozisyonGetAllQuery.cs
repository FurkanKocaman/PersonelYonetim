using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;
using System.Threading.Tasks.Dataflow;

namespace PersonelYonetim.Server.Application.Pozisyonlar;

public sealed record PozisyonGetAllQuery(
    Guid? SirketId) :IRequest<IQueryable<PozisyonGetAllQueryResponse>> ;

public sealed class PozisyonGetAllQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public Guid SirketId { get; set; } = default!;
    public string SirketAd { get; set; } = default!;
}

internal sealed class PozisyonGetAllQueryHandler(
    IPozisyonRepository pozisyonRepository,
    IHttpContextAccessor httpContextAccessor,
    IPersonelRepository personelRepository,
    IPersonelAtamaRepository personelAtamaRepository,
    UserManager<AppUser> userManager) : IRequestHandler<PozisyonGetAllQuery, IQueryable<PozisyonGetAllQueryResponse>>
{
    public Task<IQueryable<PozisyonGetAllQueryResponse>> Handle(PozisyonGetAllQuery request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }
        var personel = personelRepository.FirstOrDefault(p => p.UserId == Guid.Parse(userIdString));
        if (personel == null)
        {
        }

        var response = (from entity in pozisyonRepository.GetAll() where request.SirketId != null ? request.SirketId == entity.SirketId : true
                        join personelAtama in personelAtamaRepository.GetAll() on personel!.Id equals personelAtama.PersonelId
                        join create_user in userManager.Users.AsQueryable() on entity.CreateUserId equals create_user.Id
                        join update_user in userManager.Users.AsQueryable() on entity.UpdateUserId equals update_user.Id
                        into update_user
                        from update_users in update_user.DefaultIfEmpty()
                        select new PozisyonGetAllQueryResponse
                        {
                            Id = entity.Id,
                            Ad = entity.Ad,
                            Aciklama = entity.Aciklama,
                            SirketId = entity.SirketId,
                            SirketAd = entity.Sirket.Ad,
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
