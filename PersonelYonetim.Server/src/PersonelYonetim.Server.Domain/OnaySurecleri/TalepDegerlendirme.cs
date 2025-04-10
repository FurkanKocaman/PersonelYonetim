using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.Mesailer;
using PersonelYonetim.Server.Domain.Personeller;

namespace PersonelYonetim.Server.Domain.OnaySurecleri;
public sealed class TalepDegerlendirme
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid TalepId { get; set; }
    public OnaySurecTuruEnum TalepTuru { get; set; } = default!;
    //public IzinTalep? IzinTalep { get; set; }
    //public MesaiTalep? MesaiTalep { get; set; }
    public Guid DegerlendirenId { get; set; }
    public Personel? Degerlendiren { get; set; }
    public int AdimSirasi { get; set; }
    public DegerlendirmeDurumEnum DegerlendirmeDurumu { get; set; } = DegerlendirmeDurumEnum.Beklemede;
    public DateTimeOffset DegerlendirilmeTarihi { get; set; } = DateTimeOffset.Now;
}
