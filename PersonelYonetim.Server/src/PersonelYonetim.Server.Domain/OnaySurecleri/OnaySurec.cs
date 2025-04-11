using Ardalis.SmartEnum;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Domain.Sirketler;

namespace PersonelYonetim.Server.Domain.OnaySurecleri;
public class OnaySurec : Entity
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public OnaySurecTuruEnum OnaySurecTuruEnum { get; set; } = default!;
    public ICollection<OnaySureciAdimi> OnayAdimlari { get; set; } = new List<OnaySureciAdimi>(); 
    public Guid SirketId { get; set; }
    public Sirket? Sirket { get; set; }
}
public class OnaySureciAdimi : Entity
{
    public int Sira { get; set; }
    public Guid? PersonelId { get; set; }
    public Personel? Personel { get; set; }
    public RolTipiEnum? Rol { get; set; }
    public Guid OnaySurecId { get; set; }
    public OnaySurec? OnaySurec { get; set; }
}

public sealed class OnaySurecTuruEnum : SmartEnum<OnaySurecTuruEnum>
{
    public static readonly OnaySurecTuruEnum Izin = new("İzin", 0);
    public static readonly OnaySurecTuruEnum Mesai = new("Mesai", 1);
    public static readonly OnaySurecTuruEnum EsyaTalep = new("Eşya Talebi", 2);
    public static readonly OnaySurecTuruEnum Avans = new("Avans", 3);
    private OnaySurecTuruEnum(string name, int value) : base(name, value)
    {
    }
}
