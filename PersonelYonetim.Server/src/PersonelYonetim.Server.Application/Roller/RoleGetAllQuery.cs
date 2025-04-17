using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Pozisyonlar;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Domain.Users;

namespace PersonelYonetim.Server.Application.Roller;
public sealed record RoleGetAllQuery(
    Guid? tenantId
    ) : IRequest<IQueryable<RoleGetAllQueryResponse>>;

public sealed class RoleGetAllQueryResponse
{
    public Guid Id { get; set; }
    public string Ad { get; set; } = default!;
    public bool YapisalRolMu { get; set; }
    public string? Aciklama { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid CreateUserId { get; set; } = default!;
    public string CreateUserName { get; set; } = default!;  
    public DateTimeOffset? UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public string? UpdateUserName { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeleteAt { get; set; }
    public Guid? DeleteUserId { get; set; }
}

internal sealed class RoleGetAllQueryHandler(
    ICurrentUserService currentUserService,
    RoleManager<AppRole> roleManager,
    UserManager<AppUser> userManager
    ) : IRequestHandler<RoleGetAllQuery, IQueryable<RoleGetAllQueryResponse>>
{
    public Task<IQueryable<RoleGetAllQueryResponse>> Handle(RoleGetAllQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if(!tenantId.HasValue)
            throw new UnauthorizedAccessException("Tenant bilgisi bulunamadı.");

        var response = roleManager.Roles
            .Where(p => !p.IsDeleted && p.TenantId == tenantId.Value)
            .GroupJoin(userManager.Users,
                  role => role.CreateUserId,
                  createUsers => createUsers.Id,
                  (role, createUsers) => new { role, createUsers })
            .SelectMany(
                    dp => dp.createUsers.DefaultIfEmpty(),
                    (dp, createUser) => new { dp.role, createUser })
            .GroupJoin(userManager.Users,
                  dp => dp.role.UpdateUserId,
                  updateUser => updateUser.Id,
                  (dp, updateUsers) => new { dp.role, dp.createUser, updateUsers })
            .SelectMany(
                    ppu => ppu.updateUsers.DefaultIfEmpty(),
                    (ppu, updateUser) => new  RoleGetAllQueryResponse
                    {
                        Id = ppu.role.Id,
                        Ad = ppu.role.Name!,
                        Aciklama = ppu.role.Aciklama,
                        CreatedAt = ppu.role.CreatedAt,
                        CreateUserId = ppu.createUser != null ? ppu.createUser.Id : Guid.Empty,
                        CreateUserName = ppu.createUser != null ? ppu.createUser.FirstName + " " + ppu.createUser.LastName + " (" + ppu.createUser.Email + ")" : "Bulunamadı",
                        UpdateAt = ppu.role.UpdateAt,
                        UpdateUserId = updateUser != null ? updateUser.Id : null,
                        UpdateUserName = updateUser != null ? updateUser.FirstName + " " + updateUser.LastName + " (" + updateUser.Email + ")" : "Bulunamadı",
                        IsDeleted = ppu.role.IsDeleted,
                        DeleteAt = ppu.role.DeleteAt,
                        DeleteUserId = ppu.role.DeleteUserId,
                        YapisalRolMu = ppu.role.YapisalRolMu
                    });

        return Task.FromResult(response.AsQueryable());

    }
}
