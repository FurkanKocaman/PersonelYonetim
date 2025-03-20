using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Sirketler;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.Sirketler;

public sealed record SirketlerGetQuery() : IRequest<IQueryable<SirketlerGetQueryResponse>>;

public sealed class SirketlerGetQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public string? LogoUrl { get; set; }
    public Adres Adres { get; set; } = default!;
    public Iletisim Iletisim { get; set; } = default!;
}

internal sealed class SirketlerGetQueryHandler(
    UserManager<AppUser> userManager,
    IPersonelRepository personelRepository,
    ISirketRepository sirketRepository,
    IPersonelAtamaRepository personelAtamaRepository,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<SirketlerGetQuery, IQueryable<SirketlerGetQueryResponse>>
{
    public Task<IQueryable<SirketlerGetQueryResponse>> Handle(SirketlerGetQuery request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }
        var personel = personelRepository.FirstOrDefault(p => p.UserId == Guid.Parse(userIdString));
        if (personel == null) { 
        }

        var result = (from personelAtama in personelAtamaRepository.GetAll()
                      where personel!.Id == personelAtama.PersonelId
                      join sirket in sirketRepository.GetAll() on personelAtama.SirketId equals sirket.Id
                      join create_user in userManager.Users.AsQueryable() on sirket.CreateUserId equals create_user.Id
                      join update_user in userManager.Users.AsQueryable() on sirket.UpdateUserId equals update_user.Id
                      into update_user
                      from update_users in update_user.DefaultIfEmpty()
                          //where personelAtama.YoneticiTipi >= 0
                      select new SirketlerGetQueryResponse
                      {
                          Id = sirket.Id,
                          Ad=sirket.Ad,
                          Aciklama=sirket.Aciklama,
                          LogoUrl=sirket.LogoUrl,
                          Adres = sirket.Adres,
                          Iletisim = sirket.Iletisim,
                          IsActive = sirket.IsActive,
                          CreatedAt = sirket.CreatedAt,
                          CreateUserId = create_user.Id,
                          CreateUserName = create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",
                          UpdateAt = sirket.UpdateAt,
                          UpdateUserId = update_users.Id,
                          UpdateUserName = sirket.UpdateUserId == null ? null : update_users.FirstName + " " + update_users.LastName + " (" + update_users.Email + ")",
                          IsDeleted = sirket.IsDeleted,
                          DeleteAt = sirket.DeleteAt,
                      });
        return Task.FromResult(result);
    }
}
