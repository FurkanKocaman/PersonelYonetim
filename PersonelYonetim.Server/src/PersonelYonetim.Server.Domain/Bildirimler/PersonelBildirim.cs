using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Personeller;

namespace PersonelYonetim.Server.Domain.Bildirimler;
public sealed class PersonelBildirim : Entity
{
    public Guid BildirimId { get; set; }
    public Bildirim Bildirim { get; set; } = default!;
    public Guid PersonelId { get; set; }
    public Personel Personel { get; set; } = default!;
    public bool OkunduMu { get; set; } = false;
    public DateTimeOffset? OkunmaTarihi { get; set; }
}
