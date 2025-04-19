using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Personeller;

namespace PersonelYonetim.Server.Domain.ZamanYonetimler;
public class CalismaCizelge: Entity
{
    public Guid PersonelId { get; set; }
    public Personel Personel { get; set; } = default!;
    public int Yil { get; set; }
    public int Ay { get; set; }

    public ICollection<GunlukCalisma> GunlukCalismalar { get; set; } = new List<GunlukCalisma>();
    public Guid TenantId { get; set; }
}
