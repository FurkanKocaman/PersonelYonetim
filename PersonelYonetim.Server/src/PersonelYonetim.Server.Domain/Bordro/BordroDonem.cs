using Ardalis.SmartEnum;
using PersonelYonetim.Server.Domain.Abstractions;

namespace PersonelYonetim.Server.Domain.Bordro;
public class BordroDonem : Entity
{
    public int Yil { get; set; }
    public int Ay { get; set; }
    public DateTimeOffset DonemBaslangic { get; set; }
    public DateTimeOffset DonemBitis { get; set; }
    public BordroDonemDurumEnum BordroDonemDurum { get; set; } = BordroDonemDurumEnum.Planlandi;
    public ICollection<MaasPusula> MaasPusulalar { get; set; } = new List<MaasPusula>();
    public Guid TenantId { get; set; }
}

public sealed class BordroDonemDurumEnum : SmartEnum<BordroDonemDurumEnum>
{
    public static readonly BordroDonemDurumEnum Planlandi = new("Planlandi", 0);
    public static readonly BordroDonemDurumEnum Hesaplaniyor = new("Hesaplaniyor", 1);
    public static readonly BordroDonemDurumEnum Onaylandi = new("Onaylandi", 2);
    public static readonly BordroDonemDurumEnum Iptal = new("Iptal", 3);
    private BordroDonemDurumEnum(string name, int value) : base(name, value)
    {
    }
}