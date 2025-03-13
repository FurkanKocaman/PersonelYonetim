using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.Users;

namespace PersonelYonetim.Server.Application.Departmanlar;

public sealed record DepartmanGetAllQuery() : IRequest<IQueryable<DepartmanGetAllQueryResponse>>;

public sealed class DepartmanGetAllQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }

}

internal sealed class DepartmanGetAllQueryHandler(
    IDepartmanRepository departmanRepository,
    UserManager<AppUser> userManager) : IRequestHandler<DepartmanGetAllQuery, IQueryable<DepartmanGetAllQueryResponse>>
{
    public Task<IQueryable<DepartmanGetAllQueryResponse>> Handle(DepartmanGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = (from entity in departmanRepository.GetAll()
                        join create_user in userManager.Users.AsQueryable() on entity.CreateUserId equals create_user.Id
                        join update_user in userManager.Users.AsQueryable() on entity.UpdateUserId equals update_user.Id
                        into update_user
                        from update_users in update_user.DefaultIfEmpty()
                        select new DepartmanGetAllQueryResponse
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
