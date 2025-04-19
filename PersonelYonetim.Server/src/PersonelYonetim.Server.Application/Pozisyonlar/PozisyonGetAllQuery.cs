using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Users;

namespace PersonelYonetim.Server.Application.Pozisyonlar;

public sealed record PozisyonGetAllQuery(
    Guid? tenantId) :IRequest<IQueryable<PozisyonGetAllQueryResponse>> ;

public sealed class PozisyonGetAllQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string? Kod { get; set; }
    public string? Aciklama { get; set; }
}

internal sealed class PozisyonGetAllQueryHandler(
    IPozisyonRepository pozisyonRepository,
    ICurrentUserService currentUserService,
    UserManager<AppUser> userManager) : IRequestHandler<PozisyonGetAllQuery, IQueryable<PozisyonGetAllQueryResponse>>
{
    public Task<IQueryable<PozisyonGetAllQueryResponse>> Handle(PozisyonGetAllQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if(!tenantId.HasValue)
            throw new UnauthorizedAccessException("Tenant bilgisi bulunamadı.");
        

        var pozisyonlar = pozisyonRepository.GetAll().Where(p => !p.IsDeleted && p.TenantId == tenantId.Value);

        var response = pozisyonlar
            .GroupJoin(userManager.Users,
                  pozisyon => pozisyon.CreateUserId,
                  createUsers => createUsers.Id,
                  (pozisyon, createUsers) => new { pozisyon, createUsers })
            .SelectMany(
                    dp => dp.createUsers.DefaultIfEmpty(),
                    (dp, createUser)=> new { dp.pozisyon, createUser })
            .GroupJoin(userManager.Users,
                  dp => dp.pozisyon.UpdateUserId,
                  updateUser => updateUser.Id,
                  (dp, updateUsers) => new { dp.pozisyon, dp.createUser, updateUsers })
            .SelectMany(
                    ppu => ppu.updateUsers.DefaultIfEmpty(),
                    (ppu, updateUser) => new PozisyonGetAllQueryResponse
                    {
                        Id = ppu.pozisyon.Id,
                        Ad = ppu.pozisyon.Ad,
                        Kod = ppu.pozisyon.Kod,
                        Aciklama = ppu.pozisyon.Aciklama,
                        IsActive = ppu.pozisyon.IsActive,
                        CreatedAt = ppu.pozisyon.CreatedAt,
                        CreateUserId = ppu.createUser != null ? ppu.createUser.Id : Guid.Empty,
                        CreateUserName = ppu.createUser != null ? ppu.createUser.FirstName + " " + ppu.createUser.LastName + " (" + ppu.createUser.Email + ")" : "Bulunamadı",
                        UpdateAt = ppu.pozisyon.UpdateAt,
                        UpdateUserId = updateUser != null ? updateUser.Id : null,
                        UpdateUserName = updateUser != null
                          ? updateUser.FirstName + " " + updateUser.LastName + " (" + updateUser.Email + ")"
                          : null,
                        IsDeleted = ppu.pozisyon.IsDeleted,
                        DeleteAt = ppu.pozisyon.DeleteAt
                    });
        return Task.FromResult(response);
    }
}
