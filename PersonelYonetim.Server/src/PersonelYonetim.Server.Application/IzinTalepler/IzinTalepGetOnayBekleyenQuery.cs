//using MediatR;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using PersonelYonetim.Server.Domain.Abstractions;
//using PersonelYonetim.Server.Domain.Izinler;
//using PersonelYonetim.Server.Domain.PersonelAtamalar;
//using PersonelYonetim.Server.Domain.Personeller;
//using PersonelYonetim.Server.Domain.Rols;
//using PersonelYonetim.Server.Domain.Users;
//using System.Security.Claims;

//namespace PersonelYonetim.Server.Application.IzinTalepler;
//public sealed record IzinTalepGetOnayBekleyenQuery() : IRequest<IQueryable<IzinTalepGetOnayBekleyenQueryResponse>>;

//public sealed class IzinTalepGetOnayBekleyenQueryResponse : EntityDto
//{
//    public Guid PersonelId { get; set; }
//    public string PersonelFullName { get; set; } = default!;
//    public DateTimeOffset BaslangicTarihi { get; set; }
//    public DateTimeOffset BitisTarihi { get; set; }
//    public DateTimeOffset MesaiBaslangicTarihi { get; set; }
//    public decimal ToplamSure { get; set; }
//    public string IzinTuru { get; set; } = default!;
//    public string? Aciklama { get; set; }
//    public string DegerlendirmeDurumu { get; set; } = default!;
//    public List<CakisanIzinTalep> CakisanIzinTalepler { get; set; } = new List<CakisanIzinTalep>();
//    public List<OnaySureci?>? OnayAdimlari { get; set; } = null;
//}
//public sealed class CakisanIzinTalep
//{
//    public Guid? PersonelId { get; set; }
//    public string? PersonelFullName { get; set; }
//    public DateTimeOffset BaslangicTarihi { get; set; }
//    public DateTimeOffset BitisTarihi { get; set; }
//}
//public sealed class OnaySureci
//{
//    public Guid? PersonelId { get; set; }
//    public string? PersonelAd { get; set; } = string.Empty;
//    public string? AvatarUrl { get; set; }
//    public string? Rol { get; set; }
//    public int Sira { get; set; }
//    public string Durum { get; set; } = string.Empty;
//}
//    internal sealed class IzinTalepGetOnayBekleyenQueryHandler(
//    IIzinTalepRepository izinTalepRepository,
//    UserManager<AppUser> userManager,
//    IPersonelRepository personelRepository,
//    IPersonelAtamaRepository personelAtamaRepository,
//    IHttpContextAccessor httpContextAccessor
//    ) : IRequestHandler<IzinTalepGetOnayBekleyenQuery, IQueryable<IzinTalepGetOnayBekleyenQueryResponse>>
//{
//    public Task<IQueryable<IzinTalepGetOnayBekleyenQueryResponse>> Handle(IzinTalepGetOnayBekleyenQuery request, CancellationToken cancellationToken)
//    {
//        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//        if (string.IsNullOrEmpty(userIdString))
//        {
//            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
//        }

//        var personelQuery = personelRepository.GetAll()
//            .Where(p => p.UserId == Guid.Parse(userIdString) && !p.IsDeleted)
//            .Select(p => new { p.Id });

//        var personel = personelQuery.FirstOrDefault();
//        if (personel is null)
//        {
//            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
//        }

//        var personelAtama = personelAtamaRepository.GetAll()
//           .Where(p => p.PersonelId == personel.Id && p.IsActive && !p.IsDeleted)
//           .Select(p => p)
//           .FirstOrDefault();

//        if (personelAtama is null)
//        {
//            throw new UnauthorizedAccessException("Personel atama bilgisi bulunamadı.");
//        }

//        //Bunu ilerde değiştir çünkü tüm kayıtları alıp belleğe atıyor
//        var allIzinler = izinTalepRepository.GetAll()
//        .Include(i => i.Personel)
//        .Where(i => i.IsDeleted == false
//        //&& i.DegerlendirmeDurumu != DegerlendirmeDurumEnum.Reddedildi
//        )
//        .ToList();

//        var izinler = izinTalepRepository
//       .GetAll()
//       .Include(i => i.Personel)
//       .ThenInclude(p => p.PersonelAtamalar).Where(p => p.IsDeleted == false && p.SirketId == personelAtama.SirketId)
//       .Include(i => i.IzinTur)
//       .Include(i => i.DegerlendirmeAdimlari)
//           .ThenInclude(td => td.OnaySureciAdimi)
//       .Where(i => i.DegerlendirmeAdimlari
//               //.Where(td => td.DegerlendirmeDurumu == DegerlendirmeDurumEnum.Beklemede) // Bu kısım sadece beklemede olanlar
//               .OrderBy(td => td.OnaySureciAdimi!.Sira)
//               .Take(1)
//               .Any(td =>
//                   (td.OnaySureciAdimi!.PersonelId.HasValue && td.OnaySureciAdimi.PersonelId == personel.Id) 
//                   //||(td.OnaySureciAdimi!.Rol != null && td.OnaySureciAdimi.Rol == personelAtama.RolTipi && CanViewTalep(td.OnaySureciAdimi.Rol,i.Personel.PersonelAtamalar.FirstOrDefault(p => !p.IsDeleted && p.IsActive && p.SirketId == personelAtama.SirketId),personelAtama ))))
//                )).ToList();

//        var response = izinler.AsEnumerable()
//              .Select(entity => {
//                  var cakisanlar = allIzinler
//                       .Where(x =>
//                           x.Id != entity.Id &&
//                           x.PersonelId != entity.PersonelId &&
//                           x.SirketId == entity.SirketId &&
//                           !(entity.BitisTarihi < x.BaslangicTarihi || entity.BaslangicTarihi > x.BitisTarihi)
//                       )
//                       .Select(x => new CakisanIzinTalep
//                          {
//                              PersonelId = x.PersonelId,
//                              PersonelFullName = x.Personel.FullName,
//                              BaslangicTarihi = x.BaslangicTarihi,
//                              BitisTarihi = x.BitisTarihi
//                          })
//                       .ToList();

//                  var entityPa = entity.Personel.PersonelAtamalar.Where(p => p.IsDeleted == false && p.SirketId == personelAtama.SirketId).FirstOrDefault();

//                  var onayAdimlari = entity.DegerlendirmeAdimlari.OrderBy(da => da.OnaySureciAdimi?.Sira ?? int.MaxValue)
//                  .Select(x =>
//                  {
//                      var adim = x.OnaySureciAdimi;
//                      if (adim is null) return null;

//                      Guid approverPersonelId = Guid.Empty;
//                      string approverName = "Bilinmiyor"; // Varsayılan
//                      string? approverAvatarUrl = null;
//                      string? roleName = adim.Rol?.ToString(); // Enum ise ToString() veya Name özelliği

//                      if (adim.PersonelId.HasValue)
//                      {
//                          approverPersonelId = adim.PersonelId.Value;
//                          approverName = personelRepository.FirstOrDefault(p => p.Id == adim.PersonelId)?.FullName ?? "Bilinmiyor";
//                      }
//                      else if (adim.Rol is not null)
//                      {
//                          var roleApprover = personelAtamaRepository.Where(pa =>
//                              pa.RolTipi == adim.Rol.Value && CanViewTalep(adim.Rol,pa, entity.Personel.PersonelAtamalar.FirstOrDefault(p => !p.IsDeleted && p.IsActive))
//                          ).Include(p => p.Personel).FirstOrDefault();

//                          if (roleApprover?.Personel != null)
//                          {
//                              approverPersonelId = roleApprover.PersonelId;
//                              approverName = roleApprover.Personel.FullName;
//                              approverAvatarUrl = roleApprover.Personel.AvatarUrl;
//                          }
//                          else
//                          {
//                              approverName = $"{roleName} Rolü (Atanmamış)"; // Atanmamışsa belirt
//                          }
//                      }

//                      return new OnaySureci
//                      {
//                          PersonelId = approverPersonelId,
//                          PersonelAd = approverName,
//                          AvatarUrl = approverAvatarUrl, // sonradan eklendi
//                          Rol = roleName,
//                          Sira = x.OnaySureciAdimi!.Sira,
//                          Durum = x.DegerlendirmeDurumu.Name,// sonradan eklendi
//                      };
//                  } 
//                ).ToList();

//                  return new IzinTalepGetOnayBekleyenQueryResponse
//                  {
//                      Id = entity.Id,
//                      PersonelId = entity.Personel.Id,
//                      PersonelFullName = entity.Personel.FullName,
//                      BaslangicTarihi = entity.BaslangicTarihi,
//                      BitisTarihi = entity.BitisTarihi,
//                      MesaiBaslangicTarihi = entity.MesaiBaslangicTarihi,
//                      ToplamSure = entity.ToplamSure,
//                      IzinTuru = entity.IzinTur.Ad,
//                      Aciklama = entity.Aciklama!,
//                      DegerlendirmeDurumu = entity.DegerlendirmeDurumu.Name!,
//                      CakisanIzinTalepler = cakisanlar,
//                      OnayAdimlari = onayAdimlari,
//                      IsActive = entity.IsActive,
//                      CreatedAt = entity.CreatedAt,
//                      CreateUserId = entity.CreateUserId,
//                      CreateUserName = userManager.Users
//                          .Where(u => u.Id == entity.CreateUserId)
//                          .Select(u => $"{u.FirstName} {u.LastName} ({u.Email})")
//                          .FirstOrDefault(),
//                      UpdateAt = entity.UpdateAt,
//                      UpdateUserId = entity.UpdateUserId,
//                      UpdateUserName = userManager.Users
//                          .Where(u => u.Id == entity.UpdateUserId)
//                          .Select(u => $"{u.FirstName} {u.LastName} ({u.Email})")
//                          .FirstOrDefault(),
//                      IsDeleted = entity.IsDeleted,
//                      DeleteAt = entity.DeleteAt
//                  };
//              }).AsQueryable();

//        return Task.FromResult(response);
//    }
//    private bool CanViewTalep(RolTipiEnum role, PersonelAtama? personelAtama1, PersonelAtama? personelAtama2)
//    {
//        if (personelAtama1 is null || personelAtama2 is null) return false;

//        if(role == RolTipiEnum.DepartmanYardimci || role == RolTipiEnum.DepartmanYonetici)
//        {
//            return personelAtama1.DepartmanId == personelAtama2.DepartmanId;
//        }
//        if (role == RolTipiEnum.SubeYardimci || role == RolTipiEnum.SubeYonetici)
//        {
//            return personelAtama1.SubeId == personelAtama2.SubeId;
//        }
//        if (role == RolTipiEnum.SirketYardimci || role == RolTipiEnum.SirketYonetici)
//        {
//            return personelAtama1.SirketId == personelAtama2.SirketId;
//        }
//        return false;
//    }
//}
