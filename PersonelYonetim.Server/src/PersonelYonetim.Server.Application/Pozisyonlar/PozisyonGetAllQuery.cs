using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Users;

namespace PersonelYonetim.Server.Application.Pozisyonlar;

public sealed record PozisyonGetAllQuery(
    Guid DepartmanId) :IRequest<IQueryable<PozisyonGetAllQueryResponse>> ;

public sealed class PozisyonGetAllQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
}

internal sealed class PozisyonGetAllQueryHandler(
    IPozisyonRepository pozisyonRepository,
    UserManager<AppUser> userManager) : IRequestHandler<PozisyonGetAllQuery, IQueryable<PozisyonGetAllQueryResponse>>
{
    public Task<IQueryable<PozisyonGetAllQueryResponse>> Handle(PozisyonGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = (from entity in pozisyonRepository.GetAll() where entity.DepartmanId == request.DepartmanId
                        join create_user in userManager.Users.AsQueryable() on entity.CreateUserId equals create_user.Id
                        join update_user in userManager.Users.AsQueryable() on entity.UpdateUserId equals update_user.Id
                        into update_user
                        from update_users in update_user.DefaultIfEmpty()
                        select new PozisyonGetAllQueryResponse
                        {
                            Id = entity.Id,
                            Ad = entity.Ad,
                            Aciklama = entity.Aciklama,
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
