using PersonelYonetim.Server.Domain.Abstractions;

namespace PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
public class Pozisyon:Entity
{
    public string Ad { get; set; } = default!;
    public string? Kod { get; set; }
    public string? Aciklama { get; set; }
    public Guid TenantId { get; set; }
}
