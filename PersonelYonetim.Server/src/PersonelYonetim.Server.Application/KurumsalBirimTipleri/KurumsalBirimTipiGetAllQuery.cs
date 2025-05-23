﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using PersonelYonetim.Server.Domain.Users;

namespace PersonelYonetim.Server.Application.KurumsalBirimTipleri;
public sealed record KurumsalBirimTipiGetAllQuery(
    ) : IRequest<IQueryable<KurumsalBirimTipiGetAllQueryResponse>>;

public sealed class KurumsalBirimTipiGetAllQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public int HiyerarsiSeviyesi { get; set; }
    public bool YoneticisiOlabilirMi { get; set; } = false;
    public List<KurumsalBirimDto> KurumsalBirimler { get; set; } = new List<KurumsalBirimDto>();
    public int BirimCount { get; set; }
}
public sealed class KurumsalBirimDto
{
    public Guid Id { get; set; }
    public string Ad { get; set; } = default!;
    public string? Kod { get; set; }
    public Guid BirimTipiId { get; set; }
    public string BirimTipiAd { get; set; } = default!;
    public Guid? UstBirimId { get;set; }
    public int PersonelCount { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

internal sealed class KurumsalBirimTipiGetAllQueryHandler(
    IKurumsalBirimTipiRepository kurumsalBirimTipiRepository,
    IKurumsalBirimRepository kurumsalBirimRepository,
    UserManager<AppUser> userManager,
    ICurrentUserService currentUserService
    ) : IRequestHandler<KurumsalBirimTipiGetAllQuery, IQueryable<KurumsalBirimTipiGetAllQueryResponse>>
{
    public Task<IQueryable<KurumsalBirimTipiGetAllQueryResponse>> Handle(KurumsalBirimTipiGetAllQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        Guid? tenantId = currentUserService.TenantId;

        if(!userId.HasValue || !tenantId.HasValue)
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");


        var kurumsalBirimler = kurumsalBirimRepository.Where(p => p.TenantId == tenantId && p.IsDeleted == false).Include(p => p.Gorevlendirmeler);

        var response = kurumsalBirimTipiRepository.Where(p => p.TenantId == tenantId && !p.IsDeleted).OrderBy(p => p.HiyerarsiSeviyesi)
                 .GroupJoin(userManager.Users,
                    birimTipi => birimTipi.CreateUserId,
                    createUser => createUser.Id,
                    (birimTipi, createUsers) => new { birimTipi, createUsers })
                  .SelectMany(
                    ku => ku.createUsers.DefaultIfEmpty(),
                    (ku, createUser) => new { ku.birimTipi, createUser })
                  .GroupJoin(userManager.Users,
                    ku => ku.birimTipi.UpdateUserId,
                    updateUser => updateUser.Id,
                    (ku, updateUsers) => new { ku.birimTipi, ku.createUser, updateUsers })
                  .SelectMany(
                    kuu => kuu.updateUsers.DefaultIfEmpty(),
                    (kuu, updateUser) => new KurumsalBirimTipiGetAllQueryResponse
                    {
                        Id = kuu.birimTipi.Id,
                        Ad = kuu.birimTipi.Ad,
                        Aciklama = kuu.birimTipi.Aciklama,
                        HiyerarsiSeviyesi = kuu.birimTipi.HiyerarsiSeviyesi,
                        YoneticisiOlabilirMi = kuu.birimTipi.YoneticisiOlabilirMi,
                        KurumsalBirimler = kurumsalBirimler.Where(p => p.BirimTipiId == kuu.birimTipi.Id).Select(p => new KurumsalBirimDto
                        {
                            Id = p.Id,
                            Ad = p.Ad,
                            Kod = p.Kod,
                            BirimTipiId = p.BirimTipiId,
                            BirimTipiAd = kuu.birimTipi.Ad,
                            UstBirimId = p.UstBirimId,
                            PersonelCount = p.Gorevlendirmeler.Count(),
                            IsActive = p.IsActive,
                            CreatedAt = p.CreatedAt,
                        }).ToList(),
                        BirimCount = kurumsalBirimler.Where(p => p.BirimTipiId == kuu.birimTipi.Id).Count() ,
                        IsActive = kuu.birimTipi.IsActive,
                        CreatedAt = kuu.birimTipi.CreatedAt,
                        CreateUserId = kuu.createUser != null ? kuu.createUser.Id : Guid.Empty,
                        CreateUserName = kuu.createUser != null ? kuu.createUser.FirstName + " " + kuu.createUser.LastName + " (" + kuu.createUser.Email + ")" : "Bilinmiyor",
                        UpdateAt = kuu.birimTipi.UpdateAt,
                        UpdateUserId = null,
                        UpdateUserName = null,
                        IsDeleted = kuu.birimTipi.IsDeleted,
                        DeleteAt = kuu.birimTipi.DeleteAt
                    });
        return Task.FromResult(response);
    }
}

