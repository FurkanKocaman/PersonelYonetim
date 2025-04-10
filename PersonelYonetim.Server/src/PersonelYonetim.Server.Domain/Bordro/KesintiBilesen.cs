using PersonelYonetim.Server.Domain.Abstractions;

namespace PersonelYonetim.Server.Domain.Bordro;
public class KesintiBilesen : Entity
{
    public Guid MaasPusulaId { get; set; }
    public MaasPusula? MaasPusula { get; set; }
    public string KesintiTuru { get; set; } = default!;
    public string? Aciklama { get; set; }
    public decimal Tutar { get; set; }
}
