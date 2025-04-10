using Ardalis.SmartEnum;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Sirketler;

namespace PersonelYonetim.Server.Domain.Mesailer;
public class MesaiTalep : Entity
{
    public Guid PersonelId { get; set; }
    public Personel? Personel { get; set; }
    public string Aciklama { get; set; } = default!;    
    public DateTime BaslangicTarihi { get; set; }
    public DateTime BitisTarihi { get; set; }
    public decimal ToplamSure { get; set; }
    public MesaiDegerlendirmeDurumEnum MesaiDegerlendirmeDurum { get; set; } = default!;
    public decimal? Ucret { get; set; }// mesai ücrete çevrilirse
    public decimal? IzinGun { get; set; } // mesai izine çevrilirse
    public ICollection<TalepDegerlendirme> DegerlendirmeAdimlari { get; set; } = new List<TalepDegerlendirme>();
    public Guid SirketId { get; set; }
    public Sirket? Sirket { get; set; }
}

public sealed class MesaiDegerlendirmeDurumEnum : SmartEnum<MesaiDegerlendirmeDurumEnum>
{
    public static readonly MesaiDegerlendirmeDurumEnum Onaylandi = new("Beklemede", 0);
    public static readonly MesaiDegerlendirmeDurumEnum IzneCevrildi = new("İzne Çevrildi", 1);
    public static readonly MesaiDegerlendirmeDurumEnum UcreteCevrildi = new("Ücrete Çevrildi", 2);
    public static readonly MesaiDegerlendirmeDurumEnum Reddedildi = new("Reddedildi", 3);
    private MesaiDegerlendirmeDurumEnum(string name, int value) : base(name, value)
    {
    }
}

