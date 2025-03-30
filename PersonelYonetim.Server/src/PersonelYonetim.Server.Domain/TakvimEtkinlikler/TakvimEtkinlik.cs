using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Sirketler;

namespace PersonelYonetim.Server.Domain.TakvimEtkinlikler;
public sealed class TakvimEtkinlik : Entity
{
    public string Baslik { get; set; } = default!;
    public string? Aciklama { get; set; }
    public DateTimeOffset BaslangicTarihi { get; set; } = default!;
    public DateTimeOffset? BitisTarihi { get; set; }
    public Guid SirketId { get; set; } = default!;
    public Sirket Sirket { get; set; } = default!;
    public bool IsPublic { get; set; } = true;
    public IEnumerable<Guid>? PersonelIdler { get; set; } = null;
}
