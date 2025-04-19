using PersonelYonetim.Server.Domain.Abstractions;


namespace PersonelYonetim.Server.Domain.TakvimEtkinlikler;
public sealed class TakvimEtkinlik : Entity
{
    public string Baslik { get; set; } = default!;
    public string? Aciklama { get; set; }
    public DateTimeOffset BaslangicTarihi { get; set; } = default!;
    public DateTimeOffset? BitisTarihi { get; set; }
    public bool IsPublic { get; set; } = true;
    public IEnumerable<Guid>? PersonelIdler { get; set; } = null;
    public Guid TenantId { get; set; }
}
