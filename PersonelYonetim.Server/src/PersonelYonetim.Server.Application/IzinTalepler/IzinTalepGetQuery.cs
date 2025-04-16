using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;
using TS.Result;

namespace PersonelYonetim.Server.Application.IzinTalepler;

public sealed record IzinTalepGetQuery() : IRequest<IQueryable<IzinTalepGetQueryResponse>>;

public sealed class IzinTalepGetQueryResponse : EntityDto
{
    public string PersonelFullName { get; set; } = default!;
    public DateTimeOffset BaslangicTarihi { get; set; }
    public DateTimeOffset BitisTarihi { get; set; }
    public DateTimeOffset MesaiBaslangicTarihi { get; set; }
    public decimal ToplamSure { get; set; }
    public string IzinTuru { get; set; } = default!;
    public string? Aciklama { get; set; }
    public string DegerlendirmeDurumu { get; set; } = default!;

    public List<OnaySureci> OnayAdimlari { get; set; } = new List<OnaySureci>();
}
internal sealed class IzinTalepGetQueryHandler(
    IIzinTalepRepository izinTalepRepository,
    ITalepDegerlendirmeRepository talepDegerlendirmeRepository,
    UserManager<AppUser> userManager,
    IPersonelRepository personelRepository,
    ICurrentUserService currentUserService
) : IRequestHandler<IzinTalepGetQuery, IQueryable<IzinTalepGetQueryResponse>>
{
    public Task<IQueryable<IzinTalepGetQueryResponse>> Handle(IzinTalepGetQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        Guid? tenantId = currentUserService.TenantId;

        if (!userId.HasValue || !tenantId.HasValue)
        {
            return Task.FromResult(Enumerable.Empty<IzinTalepGetQueryResponse>().AsQueryable());
        }

        var personel = personelRepository.Where(p => p.UserId == userId && p.TenantId == tenantId).Select(p => new { p.Id }).FirstOrDefault();
        if (personel == null)
            return Task.FromResult(Enumerable.Empty<IzinTalepGetQueryResponse>().AsQueryable());

        var talepDegerlendirmeler = talepDegerlendirmeRepository.Where(p => p.TenantId == tenantId);

        var izinTalepler = izinTalepRepository.Where(p => p.TenantId == tenantId && p.PersonelId == personel.Id);
        var response = izinTalepler
                .Join(userManager.Users,
                    izinTalep => izinTalep.CreateUserId,
                    createUser => createUser.Id,
                    (izinTalep, createUser) => new { izinTalep, createUser })
                .GroupJoin(userManager.Users,
                    itu => itu.izinTalep.UpdateUserId,
                    updateUser => updateUser.Id,
                    (itu, updateUsers) => new { itu.izinTalep, itu.createUser, updateUsers })
                .SelectMany(
                     itu => itu.updateUsers.DefaultIfEmpty(),
                     (ituu, updateUser) =>
                     new IzinTalepGetQueryResponse
                     {
                         Id = ituu.izinTalep.Id,
                         PersonelFullName = ituu.izinTalep.Personel.FullName,
                         BaslangicTarihi = ituu.izinTalep.BaslangicTarihi,
                         BitisTarihi = ituu.izinTalep.BitisTarihi,
                         MesaiBaslangicTarihi = ituu.izinTalep.MesaiBaslangicTarihi,
                         ToplamSure = ituu.izinTalep.ToplamSure,
                         IzinTuru = ituu.izinTalep.IzinTur.Ad,
                         Aciklama = ituu.izinTalep.Aciklama,
                         DegerlendirmeDurumu = ituu.izinTalep.GuncelDegerlendirmeDurumu().Name,
                         OnayAdimlari = talepDegerlendirmeler.Where(t => t.TalepId == ituu.izinTalep.Id).OrderBy(t => t.AdimSirasi).Include(t => t.AtananOnayciPersonel).ThenInclude(p => p!.PersonelGorevlendirmeler).Select(t => new OnaySureci
                         {
                             PersonelAd = t.AtananOnayciPersonel != null ? t.AtananOnayciPersonel.Ad : "Bilinmiyor",
                             AvatarUrl = t.AtananOnayciPersonel != null ? t.AtananOnayciPersonel.AvatarUrl : null,
                             KurumsalBirimAd = t.AtananOnayciPersonel!.PersonelGorevlendirmeler.FirstOrDefault(p => p.IsDeleted == false && p.TenantId == tenantId) != null ? t.AtananOnayciPersonel!.PersonelGorevlendirmeler.FirstOrDefault(p => p.IsDeleted == false && p.TenantId == tenantId)!.KurumsalBirim.Ad : "Bilinmiyor",
                             PozisyonAd = t.AtananOnayciPersonel!.PersonelGorevlendirmeler.FirstOrDefault(p => p.IsDeleted == false && p.TenantId == tenantId) != null ? t.AtananOnayciPersonel!.PersonelGorevlendirmeler.FirstOrDefault(p => p.IsDeleted == false && p.TenantId == tenantId)!.Pozisyon.Ad : "Bilinmiyor",
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
                     }).OrderBy(p => p.CreatedAt).AsQueryable();

        return Task.FromResult(response);
    }
}
