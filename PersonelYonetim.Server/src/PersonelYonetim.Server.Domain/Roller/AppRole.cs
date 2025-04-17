using Microsoft.AspNetCore.Identity;

namespace PersonelYonetim.Server.Domain.Rols;

public sealed class AppRole : IdentityRole<Guid>
{
    public string? Aciklama { get; set; }
    public bool YapisalRolMu { get; set; } = false;
    public Guid TenantId { get; set; }
    #region Audit Log
    public DateTimeOffset CreatedAt { get; set; }
    public Guid CreateUserId { get; set; } = default!;
    public DateTimeOffset? UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeleteAt { get; set; }
    public Guid? DeleteUserId { get; set; }
    #endregion
}