﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.Dtos;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
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
     ICurrentUserService currentUserService,
     ICalismaTakvimRepository calismaTakvimRepository,
     IPersonelRepository personelRepository,
     IPersonelGorevlendirmeRepository personelGorevlendirmeRepository) : IRequestHandler<CalismaTakvimiGetQuery, IQueryable<CalismaTakvimiGetQueryResponse>>
{
    public Task<IQueryable<CalismaTakvimiGetQueryResponse>> Handle(CalismaTakvimiGetQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        Guid? tenantId = currentUserService.TenantId;

        if (!userId.HasValue || !tenantId.HasValue)
            return Task.FromResult(Enumerable.Empty<CalismaTakvimiGetQueryResponse>().AsQueryable());

        var personel = personelRepository.GetAll()
            .Where(p => p.UserId == userId&& !p.IsDeleted)
            .Select(p => new { p.Id })
            .FirstOrDefault();

        if (personel is null)
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");

        var response = calismaTakvimRepository.GetAll()
            .Where(t => t.TenantId == tenantId && personelGorevlendirmeRepository.GetAll()
                .Any(p =>p.PersonelId == personel.Id))
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
