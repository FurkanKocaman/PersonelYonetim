using Ardalis.SmartEnum;
using PersonelYonetim.Server.Domain.Personeller;

namespace PersonelYonetim.Server.Domain.IzinTalepler;

public sealed class IzinTalep
{
    public IzinTalep()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }
    public Guid PersonelId { get; set; }
    public Personel Personel { get; set; } = default!;
    public Guid DepartmanId { get; set; }
    public DateTimeOffset BaslangicTarihi { get; set; }
    public DateTimeOffset BitisTarihi { get; set; }
    public IzinTipiEnum IzinTipi { get; set; } = default!;
    public string? Aciklama { get; set; }
    public DegerlendirmeDurumEnum DegerlendirmeDurumu { get; set; } = default!;
    public Guid? DegerlendirenId { get; set; }
    public Personel? Degerlendiren { get; set; }
    public DateTimeOffset? DegerlendirilmeTarihi { get; set; }
}

public sealed class IzinTipiEnum : SmartEnum<IzinTipiEnum>
{
    public static readonly IzinTipiEnum YillikIzin = new("Yıllık İzin", 0);
    public static readonly IzinTipiEnum MazeretIzni = new("Mazeret İzni", 1);
    public static readonly IzinTipiEnum RaporluIzin = new("Raporlu İzin", 2);
    private IzinTipiEnum(string name, int value) : base(name, value)
    {
    }
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
