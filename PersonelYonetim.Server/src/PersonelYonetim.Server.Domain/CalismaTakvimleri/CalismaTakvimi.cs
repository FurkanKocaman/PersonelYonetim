using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;

namespace PersonelYonetim.Server.Domain.CalismaTakvimleri;

public sealed class CalismaTakvimi : Entity
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public ICollection<CalismaGun> CalismaGunler {  get; set; } = new List<CalismaGun>();
    public ICollection<PersonelGorevlendirme> Personeller { get; set; } = new List<PersonelGorevlendirme>();
    public Guid TenantId { get; set; }
}
