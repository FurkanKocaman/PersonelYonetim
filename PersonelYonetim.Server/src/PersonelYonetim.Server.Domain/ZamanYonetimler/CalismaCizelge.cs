using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Sirketler;

namespace PersonelYonetim.Server.Domain.ZamanYonetimler;
public class CalismaCizelge: Entity
{
    public Guid PersonelId { get; set; }
    public Personel Personel { get; set; } = default!;
    public int Yil { get; set; }
    public int Ay { get; set; }
    public Guid? SirketId { get; set; }
    public Sirket? Sirket { get; set; }
    public ICollection<GunlukCalisma> GunlukCalismalar { get; set; } = new List<GunlukCalisma>();
    public Guid TenantId { get; set; }
}
