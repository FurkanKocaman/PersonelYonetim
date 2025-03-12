using Ardalis.SmartEnum;

namespace PersonelYonetim.Server.Domain.IzinTalepler;

public sealed class IzinTalep
{
    public IzinTalep()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }
    public Guid PersonelId { get; set; }
    public Guid DepartmanId { get; set; }
    public DateTimeOffset BaslangicTarihi { get; set; }
    public DateTimeOffset BitisTarihi { get; set; }
    public IzinTipiEnum IzinTipi { get; set; } = default!;
    public string? Aciklama { get; set; }
    public OnayDurumEnum OnayDurumu { get; set; } = default!;
    public Guid? OnaylayanId { get; set; }
    public DateTimeOffset? OnaylanmaTarihi { get; set; }
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
public sealed class OnayDurumEnum : SmartEnum<OnayDurumEnum>
{
    public static readonly OnayDurumEnum Onaylandi = new("Onaylandı", 0);
    public static readonly OnayDurumEnum Reddedildi = new("Reddedildi", 1);
    public static readonly OnayDurumEnum Beklemede = new("Beklemede", 2);
    private OnayDurumEnum(string name, int value) : base(name, value)
    {
    }
}
