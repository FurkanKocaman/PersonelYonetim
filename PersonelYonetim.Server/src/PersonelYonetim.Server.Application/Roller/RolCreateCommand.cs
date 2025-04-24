using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Rols;
using System.Security.Claims;
using TS.Result;

namespace PersonelYonetim.Server.Application.Roller;

public sealed record RolCreateCommand(
    string RoleName,
    bool YapisalRolMu,
    List<string> Claims,
    string? Aciklama
    ) : IRequest<Result<string>>;

internal sealed class RoleCreateCommandHandler(
    RoleManager<AppRole> roleManager,
    ICurrentUserService currentUserService
    ) : IRequestHandler<RolCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RolCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        Guid? userId = currentUserService.UserId;

        if (!tenantId.HasValue ||!userId.HasValue)
            return Result<string>.Failure("tenant bulunamadı");

        var roleExist = await roleManager.Roles.Where(p =>p.Name == request.RoleName && p.TenantId == tenantId).FirstOrDefaultAsync();

        if (roleExist is not null)
            return Result<string>.Failure("Bu isimde bir rol zaten oluşturulmuş");

        AppRole role = request.Adapt<AppRole>();
        role.CreateUserId = userId.Value;
        role.TenantId = tenantId.Value;
        role.Name = request.RoleName;

        await roleManager.CreateAsync(role);

        var claims = await roleManager.GetClaimsAsync(role);

        foreach (var permission in request.Claims)
        {
            if (!claims.Any(c => c.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("permission", permission));
            }
        }

        return Result<string>.Succeed("Rol başarıyla oluşturuldu");
    }
}
