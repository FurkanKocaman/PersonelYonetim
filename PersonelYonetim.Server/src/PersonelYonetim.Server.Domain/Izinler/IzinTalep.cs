using Ardalis.SmartEnum;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Personeller;

namespace PersonelYonetim.Server.Domain.Izinler;

public sealed class IzinTalep : Entity
{
    public Guid PersonelId { get; set; }
    public Personel Personel { get; set; } = default!;
    public DateTimeOffset BaslangicTarihi { get; set; }
    public DateTimeOffset BitisTarihi { get; set; }
    public DateTimeOffset MesaiBaslangicTarihi { get; set; }
    public decimal ToplamSure {  get; set; }
    public Guid IzinTurId { get; set; }
    public IzinTur IzinTur { get; set; } = default!;
    public string? Aciklama { get; set; }
    public DegerlendirmeDurumEnum DegerlendirmeDurumu { get; set; } = default!;
    public Guid? DegerlendirenId { get; set; }
    public Personel? Degerlendiren { get; set; }
    public DateTimeOffset? DegerlendirilmeTarihi { get; set; }
}

public sealed class DegerlendirmeDurumEnum : SmartEnum<DegerlendirmeDurumEnum>
{
    public static readonly DegerlendirmeDurumEnum Onaylandi = new("Onaylandı", 0);
    public static readonly DegerlendirmeDurumEnum Reddedildi = new("Reddedildi", 1);
    public static readonly DegerlendirmeDurumEnum Beklemede = new("Beklemede", 2);
    private DegerlendirmeDurumEnum(string name, int value) : base(name, value)
    {
    }
}
