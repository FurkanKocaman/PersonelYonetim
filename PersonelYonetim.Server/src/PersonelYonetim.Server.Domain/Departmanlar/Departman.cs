using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Sirketler;
using PersonelYonetim.Server.Domain.Subeler;

namespace PersonelYonetim.Server.Domain.Departmanlar;

public sealed class Departman : Entity
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public Guid SirketId { get; set; } = default!;
    public Sirket Sirket { get; set; } = default!;

    public Guid SubeId { get; set; }
    public Sube Sube { get; set; } = default!;

    public ICollection<Pozisyon>? Pozisyonlar { get; set; }

}
