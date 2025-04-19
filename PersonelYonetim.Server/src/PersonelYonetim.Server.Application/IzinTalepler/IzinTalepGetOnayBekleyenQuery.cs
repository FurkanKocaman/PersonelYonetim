using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Users;

namespace PersonelYonetim.Server.Application.IzinTalepler;
public sealed record IzinTalepGetOnayBekleyenQuery() : IRequest<IQueryable<IzinTalepGetOnayBekleyenQueryResponse>>;

public sealed class IzinTalepGetOnayBekleyenQueryResponse : EntityDto
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
    public List<CakisanIzinTalep> CakisanIzinTalepler { get; set; } = new List<CakisanIzinTalep>();
    public List<OnaySureci> OnayAdimlari { get; set; } = new List<OnaySureci>();
}
public sealed class CakisanIzinTalep
{
    public Guid? PersonelId { get; set; }
    public string? PersonelFullName { get; set; }
    public DateTimeOffset BaslangicTarihi { get; set; }
    public DateTimeOffset BitisTarihi { get; set; }
}
public sealed class OnaySureci
{
    public string? PersonelAd { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string KurumsalBirimAd { get; set; } = default!;
    public string PozisyonAd { get; set; } = default!;
    public int Sira { get; set; }
    public string OnayDurum { get; set; } = string.Empty;
}
internal sealed class IzinTalepGetOnayBekleyenQueryHandler(
IIzinTalepRepository izinTalepRepository,
ITalepDegerlendirmeRepository talepDegerlendirmeRepository,
UserManager<AppUser> userManager,
ICurrentUserService currentUserService,
IPersonelRepository personelRepository
) : IRequestHandler<IzinTalepGetOnayBekleyenQuery, IQueryable<IzinTalepGetOnayBekleyenQueryResponse>>
{
    public Task<IQueryable<IzinTalepGetOnayBekleyenQueryResponse>> Handle(IzinTalepGetOnayBekleyenQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        Guid? tenantId = currentUserService.TenantId;

        if (!userId.HasValue || !tenantId.HasValue)
            return Task.FromResult(Enumerable.Empty<IzinTalepGetOnayBekleyenQueryResponse>().AsQueryable());
        

        var personel = personelRepository.Where(p => p.UserId == userId && p.TenantId == tenantId).Select(p => new { p.Id }).FirstOrDefault();
        if (personel == null)
            return Task.FromResult(Enumerable.Empty<IzinTalepGetOnayBekleyenQueryResponse>().AsQueryable());

        var talepDegerlendirmeler = talepDegerlendirmeRepository.Where(p => p.TenantId == tenantId);

        var izinTalepler = izinTalepRepository.Where(p => p.TenantId == tenantId);

        var response = izinTalepRepository.GetAll()
                .Join(talepDegerlendirmeRepository.GetAll(),
                    izinTalep => izinTalep.Id,
                    talepDegerlendirme => talepDegerlendirme.TalepId,
                    (izinTalep, talepDegerlendirme) => new {izinTalep, talepDegerlendirme})
                .Where(it => it.talepDegerlendirme.AtananOnayciPersonelId == personel.Id && it.izinTalep.TenantId == tenantId)
                .Join(userManager.Users,
                    it => it.izinTalep.CreateUserId,
                    createUser => createUser.Id,
                    (it,createUser) => new {it.izinTalep, createUser})
                .GroupJoin(userManager.Users,
                    itu => itu.izinTalep.UpdateUserId,
                    updateUser => updateUser.Id,
                    (itu, updateUsers) => new {itu.izinTalep, itu.createUser, updateUsers})
                .SelectMany(
                     ituu => ituu.updateUsers.DefaultIfEmpty(),
                     (ituu, updateUser) =>
                     new IzinTalepGetOnayBekleyenQueryResponse
                     {
                         Id = ituu.izinTalep.Id,
                         PersonelId = ituu.izinTalep.PersonelId,
                         PersonelFullName = ituu.izinTalep.Personel.FullName,
                         BaslangicTarihi = ituu.izinTalep.BaslangicTarihi,
                         BitisTarihi = ituu.izinTalep.BitisTarihi,
                         MesaiBaslangicTarihi = ituu.izinTalep.MesaiBaslangicTarihi,
                         ToplamSure = ituu.izinTalep.ToplamSure,
                         IzinTuru = ituu.izinTalep.IzinTur.Ad,
                         Aciklama = ituu.izinTalep.Aciklama,
                         DegerlendirmeDurumu = ituu.izinTalep.GuncelDegerlendirmeDurumu().Name,
                         CakisanIzinTalepler = izinTalepler.Where(i =>i.Id != ituu.izinTalep.Id && (ituu.izinTalep.BaslangicTarihi <= i.BitisTarihi && ituu.izinTalep.BitisTarihi >= i.BaslangicTarihi)).Include(i => i.Personel).Select(i => new CakisanIzinTalep
                         {
                             PersonelId = i.PersonelId,
                             PersonelFullName = i.Personel != null ? i.Personel.FullName : "Bilinmiyor",
                             BaslangicTarihi = i.BaslangicTarihi,
                             BitisTarihi = i.BitisTarihi
                         }).ToList(),
                         OnayAdimlari = talepDegerlendirmeler.Where(t => t.TalepId == ituu.izinTalep.Id).OrderBy(t => t.AdimSirasi).Include(t => t.AtananOnayciPersonel).ThenInclude(p => p!.PersonelGorevlendirmeler).Select(t => new OnaySureci
                         {
                             PersonelAd = t.AtananOnayciPersonel!= null ? t.AtananOnayciPersonel.Ad : "Bilinmiyor",
                             AvatarUrl = t.AtananOnayciPersonel != null ? t.AtananOnayciPersonel.AvatarUrl : null,
                             KurumsalBirimAd = t.AtananOnayciPersonel!.PersonelGorevlendirmeler.FirstOrDefault(p => p.IsDeleted == false && p.TenantId == tenantId) != null ? t.AtananOnayciPersonel!.PersonelGorevlendirmeler.FirstOrDefault(p => p.IsDeleted == false && p.TenantId == tenantId)!.KurumsalBirim!.Ad : "Bilinmiyor",
                             PozisyonAd = t.AtananOnayciPersonel!.PersonelGorevlendirmeler.FirstOrDefault(p => p.IsDeleted == false && p.TenantId == tenantId) != null ? t.AtananOnayciPersonel!.PersonelGorevlendirmeler.FirstOrDefault(p => p.IsDeleted == false && p.TenantId == tenantId)!.Pozisyon!.Ad : "Bilinmiyor",
                             Sira = t.AdimSirasi,
                             OnayDurum = t.DegerlendirmeDurumu.Name
                         }).ToList(),
                         IsActive = ituu.izinTalep.IsActive,
                         CreatedAt = ituu.izinTalep.CreatedAt,
                         CreateUserId = ituu.createUser != null ? ituu.createUser.Id : Guid.Empty,
                         CreateUserName = ituu.createUser != null ? ituu.createUser.FirstName + " " + ituu.createUser.LastName + " (" + ituu.createUser.Email + ")" : "Bilinmiyor",
                         UpdateAt = ituu.izinTalep.UpdateAt,
                         UpdateUserId = updateUser != null ? updateUser.Id : Guid.Empty,
                         UpdateUserName = updateUser != null ? updateUser.FirstName + " " + updateUser.LastName + " (" + updateUser.Email + ")" : "Bilinmiyor",
                         IsDeleted = ituu.izinTalep.IsDeleted,
                         DeleteAt = ituu.izinTalep.DeleteAt
                     });

        return Task.FromResult(response);
    }
}
