using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Bildirimler;

namespace PersonelYonetim.Server.Domain.Duyurular;
public sealed class Duyuru : Entity
{
    public string Baslik { get; set; } = string.Empty;
    public string? Aciklama {get;set;}
    public AliciTipiEnum AliciTipi { get; set; } = AliciTipiEnum.Herkes;
    public Guid? AliciId { get; set; } // AliciTipi.Personel ise
    public List<Guid>? AliciIdler { get; set; } //AliciTipi.Personeller ise
    public Guid TenantId { get; set; }
}
