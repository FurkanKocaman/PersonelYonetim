namespace PersonelYonetim.Server.Application.Services;
public interface ICurrentUserService
{
    Guid? UserId { get;  }
    Guid? TenantId { get;  }
}
