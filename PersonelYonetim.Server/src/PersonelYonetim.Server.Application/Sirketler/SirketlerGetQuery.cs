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

        var personel = personelRepository.GetAll()
            .Where(p => p.UserId == Guid.Parse(userIdString) && !p.IsDeleted)
            .Select(p => new { p.Id, p.PersonelAtamalar })
            .FirstOrDefault();

        if (personel == null)
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");

        var sirketler = sirketRepository.GetAll().Where(p => p.IsDeleted == false);

        var response = sirketler
            .Join(personelAtamaRepository.GetAll(),
                  sirket => sirket.Id,
                  personelAtama => personelAtama.SirketId,
                  (sirket, personelAtama) => new { sirket, personelAtama })
            .Where(sp => sp.personelAtama.IsDeleted == false && sp.personelAtama.PersonelId == personel.Id)
            .GroupJoin(userManager.Users,
                  sp => sp.sirket.CreateUserId,
                  createUser => createUser.Id,
                  (sp, createUsers) => new { sp.sirket, sp.personelAtama, createUsers })
            .SelectMany(
                   spu => spu.createUsers.DefaultIfEmpty(),
                   (spu, createUser)=> new { spu.sirket, spu.personelAtama, createUser = createUser })
            .GroupJoin(userManager.Users,
                  sp => sp.sirket.UpdateUserId,
                  updateUser => updateUser.Id,
                  (sp, updateUsers) => new { sp.sirket, sp.createUser, updateUsers })
            .SelectMany(
                  spu => spu.updateUsers.DefaultIfEmpty(),
                  (spu, updateUser) => new SirketlerGetQueryResponse
                  {
                      Id = spu.sirket.Id,
                      Ad = spu.sirket.Ad,
                      Aciklama = spu.sirket.Aciklama,
                      LogoUrl = spu.sirket.LogoUrl,
                      Adres = spu.sirket.Adres,
                      Iletisim = spu.sirket.Iletisim,
                      IsActive = spu.sirket.IsActive,
                      CreatedAt = spu.sirket.CreatedAt,
                      CreateUserId = spu.sirket.CreateUserId != Guid.Empty ? spu.sirket.CreateUserId : null,
                      CreateUserName = spu.createUser != null ? spu.createUser.FirstName + " " + spu.createUser.LastName + " (" + spu.createUser.Email + ")" : null,
                      UpdateAt = spu.sirket.UpdateAt,
                      UpdateUserId = updateUser != null ? updateUser.Id : null,
                      UpdateUserName = updateUser == null ? null : updateUser.FirstName + " " + updateUser.LastName + " (" + updateUser.Email + ")",
                      IsDeleted = spu.sirket.IsDeleted,
                      DeleteAt = spu.sirket.DeleteAt
                  });

        return Task.FromResult(response);
    }
}
