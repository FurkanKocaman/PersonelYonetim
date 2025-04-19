using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;

namespace PersonelYonetim.Server.Domain.KurumsalBirimler;
public class KurumsalBirim : Entity
{
    public string Ad { get; set; } = default!;
    public string? Kod { get; set; }

    public Guid BirimTipiId { get; set; }
    public KurumsalBirimTipi BirimTipi { get; set; } = default!;

    public Guid? UstBirimId { get; set; } // null ise en üst seviyedir
    public KurumsalBirim? UstBirim { get; set; }

    public ICollection<KurumsalBirim> AltBirimler { get; set; } = new List<KurumsalBirim>();
    public ICollection<PersonelGorevlendirme> Gorevlendirmeler { get; set; } = new List<PersonelGorevlendirme>();

    public Guid TenantId { get; set; }
}
