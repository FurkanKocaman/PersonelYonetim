using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Sirketler;

namespace PersonelYonetim.Server.Domain.Subeler;

public sealed class Sube : Entity
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public Guid SirketId { get; set; } = default!;
    public Sirket Sirket { get; set; } = default!;
    public Adres Adres { get; set; } = default!;
    public Iletisim Iletisim { get; set; } = default!;
    public ICollection<Departman>? Departmanlar { get; set; }
}
