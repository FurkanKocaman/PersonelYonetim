using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.IzinTalepler;

public sealed record IzinTalepGetAllQuery() : IRequest<IQueryable<IzinTalepGetAllQueryResponse>>;

public sealed class IzinTalepGetAllQueryResponse : EntityDto
{
    public Guid PersonelId { get; set; }
    public string PersonelFullName { get; set; } = default!;
    public DateTimeOffset BaslangicTarihi { get; set; }
    public DateTimeOffset BitisTarihi { get; set; }
    public DateTimeOffset MesaiBaslangicTarihi { get; set; }
    public decimal ToplamSure { get; set; }
    public string IzinTuru { get; set; } = default!;
    public string? Aciklama { get; set; }
    public string DegerlendirmeDurumu { get; set; } = default!;
    public Guid? DegerlendirenId { get; set; }
    public string? DegerlendirenAd { get; set; }
}
internal sealed class IzinTalepGetAllQueryHandler(
    IIzinTalepRepository izinTalepRepository,
    UserManager<AppUser> userManager,
    IPersonelRepository personelRepository,
    IPersonelAtamaRepository personelAtamaRepository,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<IzinTalepGetAllQuery, IQueryable<IzinTalepGetAllQueryResponse>>
{
    public Task<IQueryable<IzinTalepGetAllQueryResponse>> Handle(IzinTalepGetAllQuery request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }

        var personelQuery = personelRepository.GetAll()
            .Where(p => p.UserId == Guid.Parse(userIdString) && !p.IsDeleted)
            .Select(p => new { p.Id });

        var personel = personelQuery.FirstOrDefault();
        if (personel is null)
        {
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
        }

        var personelAtama = personelAtamaRepository.GetAll()
            .Where(p => p.PersonelId == personel.Id && p.IsActive && !p.IsDeleted)
            .Select(p=>p)
            .FirstOrDefault();

        if (personelAtama is null)
        {
            throw new UnauthorizedAccessException("Personel atama bilgisi bulunamadı.");
        }

        var izinler = izinTalepRepository
         .GetAll()
         .Include(i => i.Personel)
         .Where(p => p.Personel.PersonelAtamalar.Any(pa=>pa.IsDeleted == false && pa.IsActive == true && pa.SirketId == personelAtama.SirketId))
         .Include(i => i.IzinTur)
         .Include(i => i.DegerlendirmeAdimlari)
             .ThenInclude(td => td.OnaySureciAdimi)
         .Where(i => i.DegerlendirmeAdimlari
                 .Where(td => td.DegerlendirmeDurumu == DegerlendirmeDurumEnum.Beklemede)
                 .OrderBy(td => td.OnaySureciAdimi!.Sira)
                 .Take(1)
                 .Any(td =>
                     (td.OnaySureciAdimi!.PersonelId.HasValue && td.OnaySureciAdimi.PersonelId == personel.Id) ||
                     (td.OnaySureciAdimi!.Rol != null && td.OnaySureciAdimi.Rol == personelAtama.RolTipi && 
                     ((td.OnaySureciAdimi.Rol == RolTipiEnum.DepartmanYonetici || td.OnaySureciAdimi.Rol == RolTipiEnum.DepartmanYardimci) ? i.Personel.PersonelAtamalar.Any(p => p.IsDeleted == false && p.DepartmanId == personelAtama.DepartmanId)
                     : (td.OnaySureciAdimi.Rol == RolTipiEnum.SubeYardimci || td.OnaySureciAdimi.Rol == RolTipiEnum.SubeYonetici) ? i.Personel.PersonelAtamalar.Any(p => p.IsDeleted == false && p.SubeId == personelAtama.SubeId)
                     : (td.OnaySureciAdimi.Rol == RolTipiEnum.SirketYardimci || td.OnaySureciAdimi.Rol == RolTipiEnum.SirketYonetici) ? i.Personel.PersonelAtamalar.Any(p => p.IsDeleted == false && p.SirketId == personelAtama.SirketId) : false)
                 ))
         );


        var x = izinler.ToList();
        var response = izinler

            .Select(entity => new IzinTalepGetAllQueryResponse
            {
                Id = entity.Id,
                PersonelId = entity.Personel.Id,
                PersonelFullName = entity.Personel.FullName,
                BaslangicTarihi = entity.BaslangicTarihi,
                BitisTarihi = entity.BitisTarihi,
                MesaiBaslangicTarihi = entity.MesaiBaslangicTarihi,
                ToplamSure = entity.ToplamSure,
                IzinTuru = entity.IzinTur.Ad,
                Aciklama = entity.Aciklama!,
                DegerlendirmeDurumu = entity.DegerlendirmeDurumu.Name!,
                DegerlendirenId = null,
                DegerlendirenAd = null,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                CreateUserId = entity.CreateUserId,
                CreateUserName = userManager.Users
                    .Where(u => u.Id == entity.CreateUserId)
                    .Select(u => $"{u.FirstName} {u.LastName} ({u.Email})")
                    .FirstOrDefault(),
                UpdateAt = entity.UpdateAt,
                UpdateUserId = entity.UpdateUserId,
                UpdateUserName = userManager.Users
                    .Where(u => u.Id == entity.UpdateUserId)
                    .Select(u => $"{u.FirstName} {u.LastName} ({u.Email})")
                    .FirstOrDefault(),
                IsDeleted = entity.IsDeleted,
                DeleteAt = entity.DeleteAt
            });

        return Task.FromResult(response);
    }

    bool sirketlerAyniMi(PersonelAtama personelAtama1, PersonelAtama personelAtama2)
    {
        return personelAtama1.SirketId == personelAtama2.SirketId;
    }
    bool subelerAyniMi(PersonelAtama personelAtama1, PersonelAtama personelAtama2)
    {
        return personelAtama1.SubeId == personelAtama2.SubeId;
    }
    bool departmanlarAyniMi(PersonelAtama personelAtama1, PersonelAtama personelAtama2)
    {
        return personelAtama1.DepartmanId == personelAtama2.DepartmanId;
    }
}

