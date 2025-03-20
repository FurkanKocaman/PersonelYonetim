using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Rols;
using System.Security.Claims;

namespace PersonelYonetim.Server.WebAPI;

public class CustomClaimsTransformation(RoleManager<AppRole> roleManager) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(claim => claim.Type == "permission"))
            return principal;

        var identity = principal.Identity as ClaimsIdentity;
        if(identity == null)
            return principal;
        var roleClaims = identity.Claims.Where(p => p.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        if (!roleClaims.Any())
        {
            return principal;
        }
        foreach (var roleClaim in roleClaims)
        {
            var role = await roleManager.FindByNameAsync(roleClaim);
            if(role is not null)
            {
                var permissions = await roleManager.GetClaimsAsync(role);
                foreach (var permission in permissions)
                {
                    if(!identity.HasClaim(permission.Type, permission.Value))
                        identity.AddClaim(permission);
                }
            }
        }

        return principal;
    }
}
