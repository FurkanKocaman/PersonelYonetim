using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.Duyurular;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Sirketler;
using PersonelYonetim.Server.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PersonelYonetim.Server.Application.Duyurular;
public sealed record DuyuruGetAllQuery(
    ) : IRequest<IQueryable<DuyuruGetAllQueryResponse>>;

public sealed class DuyuruGetAllQueryResponse:EntityDto
{
    public string Baslik { get; set; } = default!;
    public string? Aciklama { get; set; }
    public string? SirketAd { get; set; }
    public string AliciTipi { get; set; } = default!;
}

internal sealed class DuyuruGetAllQueryResponseHandler(
    IPersonelRepository personelRepository,
    IPersonelAtamaRepository personelAtamaRepository,
    IDuyuruRepository duyuruRepository,
    IHttpContextAccessor httpContextAccessor,
    UserManager<AppUser> userManager
    ) : IRequestHandler<DuyuruGetAllQuery, IQueryable<DuyuruGetAllQueryResponse>>
{
    public Task<IQueryable<DuyuruGetAllQueryResponse>> Handle(DuyuruGetAllQuery request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }

        var personel = personelRepository.GetAll()
            .Where(p => p.UserId == Guid.Parse(userIdString) && !p.IsDeleted)
            .Select(p => new { p.Id })
            .FirstOrDefault();

        if (personel == null)
        {
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
        }

        var duyurular = duyuruRepository.Where(p =>
            p.AliciTipi == AliciTipiEnum.Herkes ||
            p.AliciId == personel.Id ||
            (p.AliciIdler != null && p.AliciIdler.Contains(personel.Id))
        );
        var response = duyurular
                    .Join(personelAtamaRepository.GetAll(),
                    duyuru => duyuru.SirketId,
                    personelAtama => personelAtama.SirketId,
                    (duyuru, personelAtama) => new { duyuru, personelAtama })
                    .Where(dp => dp.personelAtama.PersonelId == personel.Id && dp.personelAtama.IsActive && dp.personelAtama.IsDeleted == false)
                     .Join(userManager.Users,
                    dp => dp.duyuru.CreateUserId,
                    createUser => createUser.Id,
                    (dp, createUser) => new { dp.duyuru, createUser })
                     .Select(dp => new DuyuruGetAllQueryResponse
                     {
                         Id = dp.duyuru.Id,
                         Baslik = dp.duyuru.Baslik,
                         Aciklama = dp.duyuru.Aciklama,
                         SirketAd = dp.duyuru.Sirket.Ad,
                         AliciTipi = dp.duyuru.AliciTipi.Name,
                         IsActive = dp.duyuru.IsActive,
                         CreatedAt = dp.duyuru.CreatedAt,
                         CreateUserId = dp.duyuru.CreateUserId,
                         CreateUserName = dp.createUser.FirstName + " " + dp.createUser.LastName + " (" + dp.createUser.Email + ")",
                         IsDeleted = dp.duyuru.IsDeleted,
                         DeleteAt = dp.duyuru.DeleteAt
                     });

        return Task.FromResult(response);

    }
}
