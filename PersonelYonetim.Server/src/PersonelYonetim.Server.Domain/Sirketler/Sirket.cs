using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Subeler;

namespace PersonelYonetim.Server.Domain.Sirketler;

public sealed class Sirket : Entity
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public string? LogoUrl { get; set; }
    public Adres Adres { get; set; } = default!;
    public Iletisim Iletisim { get; set; } = default!;

    public ICollection<Sube>? Subeler { get; set; }
}
