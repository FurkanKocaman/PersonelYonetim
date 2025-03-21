using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Subeler;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.Departmanlar;

public sealed record DepartmanGetAllQuery(Guid? SubeId) : IRequest<IQueryable<DepartmanGetAllQueryResponse>>;

public sealed class DepartmanGetAllQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public string SubeAd { get; set; } = default!;
    public Guid SubeId { get; set; } = default!;
    public string SirketAd { get; set; } = default!;
    public Guid SirketId { get; set; } = default!;
}

internal sealed class DepartmanGetAllQueryHandler(
    UserManager<AppUser> userManager,
    IPersonelRepository personelRepository,
    IPersonelAtamaRepository personelAtamaRepository,
    IDepartmanRepository departmanRepository,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<DepartmanGetAllQuery, IQueryable<DepartmanGetAllQueryResponse>>
{
    public Task<IQueryable<DepartmanGetAllQueryResponse>> Handle(DepartmanGetAllQuery request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }

        var personel = personelRepository.GetAll()
            .Where(p => p.UserId == Guid.Parse(userIdString))
            .Select(p => new { p.Id, p.PersonelAtamalar })
            .FirstOrDefault();

        if (personel == null)
        {
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
        }

        var departmanlar = departmanRepository.GetAll();
        if (request.SubeId is not null)
        {
            departmanlar = departmanlar.Where(d => d.SubeId == request.SubeId);
        }

        var response = departmanlar
            .Join(personelAtamaRepository.GetAll(),
                  departman => departman.SirketId,
                  personelAtama => personelAtama.SirketId,
                  (departman, personelAtama) => new { departman, personelAtama })
            .Where(dp => dp.personelAtama.PersonelId == personel.Id)
            .Join(userManager.Users,
                  dp => dp.departman.CreateUserId,
                  createUser => createUser.Id,
                  (dp, createUser) => new { dp.departman, createUser })
            .GroupJoin(userManager.Users,
                  dp => dp.departman.UpdateUserId,
                  updateUser => updateUser.Id,
                  (dp, updateUsers) => new { dp.departman, dp.createUser, updateUsers })
            .SelectMany(
                  dpu => dpu.updateUsers.DefaultIfEmpty(),
                  (dpu, updateUser) => new DepartmanGetAllQueryResponse
                  {
                      Id = dpu.departman.Id,
                      Ad = dpu.departman.Ad,
                      Aciklama = dpu.departman.Aciklama,
                      SubeId = dpu.departman.SubeId,
                      SubeAd = dpu.departman.Sube.Ad,
                      SirketId = dpu.departman.Sirket.Id,
                      SirketAd = dpu.departman.Sirket.Ad,
                      IsActive = dpu.departman.IsActive,
                      CreatedAt = dpu.departman.CreatedAt,
                      CreateUserId = dpu.createUser.Id,
                      CreateUserName = dpu.createUser.FirstName + " " + dpu.createUser.LastName + " (" + dpu.createUser.Email + ")",
                      UpdateAt = dpu.departman.UpdateAt,
                      UpdateUserId = updateUser != null ? updateUser.Id : null,
                      UpdateUserName = updateUser != null
                          ? updateUser.FirstName + " " + updateUser.LastName + " (" + updateUser.Email + ")"
                          : null,
                      IsDeleted = dpu.departman.IsDeleted,
                      DeleteAt = dpu.departman.DeleteAt
                  });

        return Task.FromResult(response);
    }

}
