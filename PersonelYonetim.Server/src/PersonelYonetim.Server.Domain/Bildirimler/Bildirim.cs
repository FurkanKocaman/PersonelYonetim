using Ardalis.SmartEnum;
using PersonelYonetim.Server.Domain.Abstractions;

namespace PersonelYonetim.Server.Domain.Bildirimler;
public sealed class Bildirim : Entity
{
    public string Baslik { get; set; } = string.Empty;
    public string Aciklama { get; set; } = string.Empty;
    public BildirimTipiEnum BildirimTipi { get; set; } = default!;
    public AliciTipiEnum AliciTipi { get; set; } = default!;
    public Guid? AliciId { get; set; }
    public List<Guid>? AliciIdler { get; set; }
    public string? Url { get; set; }
    public ICollection<PersonelBildirim> PersonelBildirimler { get; set; } = new List<PersonelBildirim>();
    public Guid TenantId { get; set; }
}

public sealed class BildirimTipiEnum : SmartEnum<BildirimTipiEnum>
{
    public static readonly BildirimTipiEnum Bilgilendirme = new("Bilgilendirme", 0);
    public static readonly BildirimTipiEnum Onay = new("Onay", 1);
    public static readonly BildirimTipiEnum Hatirlatma = new("Hatırlatma", 2);
    public static readonly BildirimTipiEnum Uyarı = new("Uyarı", 3);
    private BildirimTipiEnum(string name, int value) : base(name, value)
    {
    }
}

public sealed class AliciTipiEnum : SmartEnum<AliciTipiEnum>
{
    public static readonly AliciTipiEnum Personel = new("Personel", 0);
    public static readonly AliciTipiEnum Departman = new("Departman", 1);
    public static readonly AliciTipiEnum Sube = new("Şube", 2);
    public static readonly AliciTipiEnum Sirket = new("Şirket", 3);
    public static readonly AliciTipiEnum Personeller = new("Personeller", 4);
    public static readonly AliciTipiEnum Herkes = new("Herkes", 5);
    private AliciTipiEnum(string name, int value) : base(name, value)
    {
    }
}
