using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Subeler;
using PersonelYonetim.Server.Domain.Users;

namespace PersonelYonetim.Server.Application.Subeler;

public sealed record SubelerGetQuery(Guid SirketId) : IRequest<IQueryable<SubelerGetQueryResponse>>;

public sealed class SubelerGetQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public Adres Adres { get; set; } = default!;
    public Iletisim Iletisim { get; set; } = default!;
    public Guid SirketId { get; set; } = default!;
}

internal sealed class SubelerGetQueryHandler(
    UserManager<AppUser> userManager,
    ISubeRepository subeRepository) : IRequestHandler<SubelerGetQuery, IQueryable<SubelerGetQueryResponse>>
{
    public Task<IQueryable<SubelerGetQueryResponse>> Handle(SubelerGetQuery request, CancellationToken cancellationToken)
    {

        var result = (from sube in subeRepository.GetAll() where sube.SirketId == request.SirketId
                      join create_user in userManager.Users.AsQueryable() on sube.CreateUserId equals create_user.Id
                      join update_user in userManager.Users.AsQueryable() on sube.UpdateUserId equals update_user.Id
                      into update_user
                      from update_users in update_user.DefaultIfEmpty()
                      select new SubelerGetQueryResponse
                      {
                          Id= sube.Id,
                          Ad = sube.Ad,
                          Aciklama = sube.Aciklama,
                          Adres = sube.Adres,
                          Iletisim = sube.Iletisim,
                          SirketId = sube.SirketId,
                          IsActive = sube.IsActive,
                          CreatedAt = sube.CreatedAt,
                          CreateUserId = create_user.Id,
                          CreateUserName = create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",
                          UpdateAt = sube.UpdateAt,
                          UpdateUserId = update_users.Id,
                          UpdateUserName = sube.UpdateUserId == null ? null : update_users.FirstName + " " + update_users.LastName + " (" + update_users.Email + ")",
                          IsDeleted = sube.IsDeleted,
                          DeleteAt = sube.DeleteAt,
                      });
        return Task.FromResult(result);
    }
}
