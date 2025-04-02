using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Departmanlar;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Subeler;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.Subeler;

public sealed record SubelerGetQuery(Guid? SirketId) : IRequest<IQueryable<SubelerGetQueryResponse>>;

public sealed class SubelerGetQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public Adres Adres { get; set; } = default!;
    public Iletisim Iletisim { get; set; } = default!;
    public Guid SirketId { get; set; } = default!;
    public string SirketAd { get; set; } = default!;
}

internal sealed class SubelerGetQueryHandler(
    UserManager<AppUser> userManager,
    IPersonelRepository personelRepository,
    IPersonelAtamaRepository personelAtamaRepository,
    ISubeRepository subeRepository,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<SubelerGetQuery, IQueryable<SubelerGetQueryResponse>>
{
    public Task<IQueryable<SubelerGetQueryResponse>> Handle(SubelerGetQuery request, CancellationToken cancellationToken)
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

        var subeler = subeRepository.GetAll()
                .Where(p => !p.IsDeleted);
        if(request.SirketId is not null)
        {
            subeler = subeler.Where(s => s.SirketId == request.SirketId);
        }

        var response = subeler
            .Join(personelAtamaRepository.GetAll(),
                  sube => sube.SirketId,
                  personelAtama => personelAtama.SirketId,
                  (sube, personelAtama) => new { sube, personelAtama })
            .Where(sp => !sp.personelAtama.IsDeleted &&  sp.personelAtama.PersonelId == personel.Id)
            .Join(userManager.Users,
                  sp => sp.sube.CreateUserId,
                  createUser => createUser.Id,
                  (sp, createUser) => new { sp.sube, createUser })
            .GroupJoin(userManager.Users,
                  sp => sp.sube.UpdateUserId,
                  updateUser => updateUser.Id,
                  (sp, updateUsers) => new { sp.sube, sp.createUser, updateUsers })
            .SelectMany(
                  spu => spu.updateUsers.DefaultIfEmpty(),
                  (spu, updateUser) => new SubelerGetQueryResponse
                  {
                      Id = spu.sube.Id,
                      Ad = spu.sube.Ad,
                      Aciklama = spu.sube.Aciklama,
                      Adres = spu.sube.Adres,
                      Iletisim = spu.sube.Iletisim,
                      SirketId = spu.sube.Sirket.Id,
                      SirketAd = spu.sube.Sirket.Ad,
                      IsActive = spu.sube.IsActive,
                      CreatedAt = spu.sube.CreatedAt,
                      CreateUserId = spu.createUser.Id,
                      CreateUserName = spu.createUser.FirstName + " " + spu.createUser.LastName + " (" + spu.createUser.Email + ")",
                      UpdateAt = spu.sube.UpdateAt,
                      UpdateUserId = updateUser != null ? updateUser.Id : null,
                      UpdateUserName = updateUser != null
                          ? updateUser.FirstName + " " + updateUser.LastName + " (" + updateUser.Email + ")"
                          : null,
                      IsDeleted = spu.sube.IsDeleted,
                      DeleteAt = spu.sube.DeleteAt
                  });


        return Task.FromResult(response);
    }
}
