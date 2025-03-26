using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.Dtos;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.CalismaTakvimleri;
public sealed record CalismaTakvimiGetQuery(
    ) : IRequest<IQueryable<CalismaTakvimiGetQueryResponse>>;

public sealed class CalismaTakvimiGetQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public IEnumerable<CalismaGunDto> Gunler { get; set; } = new List<CalismaGunDto>();
}

internal sealed class CalismaTakvimiGetQueryHandler(
    UserManager<AppUser> userManager,
     IHttpContextAccessor httpContextAccessor,
     ICalismaTakvimRepository calismaTakvimRepository,
     IPersonelRepository personelRepository,
     IPersonelAtamaRepository personelAtamaRepository) : IRequestHandler<CalismaTakvimiGetQuery, IQueryable<CalismaTakvimiGetQueryResponse>>
{
    public Task<IQueryable<CalismaTakvimiGetQueryResponse>> Handle(CalismaTakvimiGetQuery request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }

        var personel = personelRepository.GetAll()
            .Where(p => p.UserId == Guid.Parse(userIdString))
            .Select(p => new { p.Id })
            .FirstOrDefault();

        if (personel is null)
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");

        var takvimler = calismaTakvimRepository.GetAll();
        var response = calismaTakvimRepository.GetAll()
            .Where(t => personelAtamaRepository.GetAll()
                .Any(p => p.SirketId == t.SirketId && p.PersonelId == personel.Id))
            .Include(t => t.CalismaGunler) 
            .Select(takvim => new CalismaTakvimiGetQueryResponse
            {
                Id = takvim.Id,
                Ad = takvim.Ad,
                Aciklama = takvim.Aciklama,
                Gunler = takvim.CalismaGunler.Select(g => new CalismaGunDto
                {
                    Id = g.Id,
                    Gun = g.Gun,
                    IsCalismaGunu = g.IsCalismaGunu,
                    CalismaBaslangic = g.CalismaBaslangic,
                    CalismaBitis = g.CalismaBitis,
                    MolaBaslangic = g.MolaBaslangic,
                    MolaBitis = g.MolaBitis,
                }).ToList(),

                CreateUserId = takvim.CreateUserId,
                CreateUserName = userManager.Users
                    .Where(u => u.Id == takvim.CreateUserId)
                    .Select(u => u.FirstName + " " + u.LastName + " (" + u.Email + ")")
                    .FirstOrDefault()!,

                UpdateUserId = takvim.UpdateUserId,
                UpdateUserName = userManager.Users
                    .Where(u => u.Id == takvim.UpdateUserId)
                    .Select(u => u.FirstName + " " + u.LastName + " (" + u.Email + ")")
                    .FirstOrDefault(),

                IsActive = takvim.IsActive,
                CreatedAt = takvim.CreatedAt,
                UpdateAt = takvim.UpdateAt,
                IsDeleted = takvim.IsDeleted,
                DeleteAt = takvim.DeleteAt
            }).AsQueryable();

        return Task.FromResult(response);

    }
}
