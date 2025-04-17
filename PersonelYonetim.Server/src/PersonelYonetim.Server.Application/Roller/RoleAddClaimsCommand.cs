using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.RoleClaim;
using PersonelYonetim.Server.Domain.Rols;
using System.Security.Claims;
using TS.Result;

namespace PersonelYonetim.Server.Application.Roller;
public sealed record RoleAddClaimsCommand(
    Guid RoleId
    ) : IRequest<Result<string>>;

internal sealed class RoleAddClaimsCommandHandler(
    RoleManager<AppRole> roleManager
    ) : IRequestHandler<RoleAddClaimsCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleAddClaimsCommand request, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(request.RoleId.ToString());

        if (role is null)
            return Result<string>.Failure("Rol bulunamamdı");

        var claims = await roleManager.GetClaimsAsync(role);

        var allPermissions = new List<string>
        {
            Permissions.ViewPersonel, Permissions.CreatePersonel, Permissions.EditPersonel, Permissions.DeletePersonel,
            Permissions.ViewKurumsalYapi, Permissions.CreateKurumsalYapi, Permissions.EditKurumsalYapi, Permissions.DeleteKurumsalYapi,
            Permissions.ViewIzinler, Permissions.CreateIzinler, Permissions.ApproveIzinler,
            Permissions.ViewRaporlar
        };

        foreach (var permission in allPermissions)
        {
            if(!claims.Any(c => c.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("permission", permission));
            }
        }
        return Result<string>.Succeed("Rol yetkileri başarıyla eklendi");
    }
}
