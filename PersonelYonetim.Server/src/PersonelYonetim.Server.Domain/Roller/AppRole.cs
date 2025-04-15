using Ardalis.SmartEnum;
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
public sealed class RolTipiEnum : SmartEnum<RolTipiEnum>
{
    public static readonly RolTipiEnum Calisan = new("Çalışan", 0);
    public static readonly RolTipiEnum DepartmanYardimci = new("Departman Yönetici Yardımcı", 1);
    public static readonly RolTipiEnum DepartmanYonetici = new("Departman Yönetici", 2);
    public static readonly RolTipiEnum SubeYardimci = new("Şube Yönetici Yardımcı", 3);
    public static readonly RolTipiEnum SubeYonetici = new("Şube Yönetici", 4);
    public static readonly RolTipiEnum SirketYardimci = new("Şirket Yönetici Yardımcı", 5);
    public static readonly RolTipiEnum SirketYonetici = new("Şirket Yönetici", 6);
    public static readonly RolTipiEnum Admin = new("Admin", 7);
    private RolTipiEnum(string name, int value) : base(name, value)
    {
    }
}