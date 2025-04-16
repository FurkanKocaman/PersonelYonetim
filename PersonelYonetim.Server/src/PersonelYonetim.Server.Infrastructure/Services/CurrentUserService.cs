using Microsoft.AspNetCore.Http;
using PersonelYonetim.Server.Application.Services;
using System.Security.Claims;

namespace PersonelYonetim.Server.Infrastructure.Services;
internal sealed class CurrentUserService(
    IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? UserId
    {
        get
        {
            var userIdString = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return null;
            }
            return Guid.Parse(userIdString);
        }
    }

    public Guid? TenantId
    {
        get
        {
            var tenantIdString = httpContextAccessor.HttpContext?.User?.FindFirstValue("tenant_id");
            if (string.IsNullOrEmpty(tenantIdString))
            {
                return null;
            }
            return Guid.Parse(tenantIdString);
        }
    }

}
