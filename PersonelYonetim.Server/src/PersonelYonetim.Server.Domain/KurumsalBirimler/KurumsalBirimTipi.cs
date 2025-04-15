using PersonelYonetim.Server.Domain.Abstractions;

namespace PersonelYonetim.Server.Domain.KurumsalBirimler;
public sealed class KurumsalBirimTipi : Entity
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public int HiyerarsiSeviyesi { get; set; }
    public bool YoneticisiOlabilirMi { get; set; } = false;
    public Guid TenantId { get; set; }
}
