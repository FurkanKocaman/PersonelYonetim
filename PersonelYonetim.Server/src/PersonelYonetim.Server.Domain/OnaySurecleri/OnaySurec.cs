using Ardalis.SmartEnum;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Rols;

namespace PersonelYonetim.Server.Domain.OnaySurecleri;
public class OnaySurec : Entity
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public OnaySurecTuruEnum OnaySurecTuruEnum { get; set; } = default!;
    public ICollection<OnaySureciAdimi> OnayAdimlari { get; set; } = new List<OnaySureciAdimi>(); 
    public Guid TenantId { get; set; }
}
public class OnaySureciAdimi : Entity
{
    public int Sira { get; set; }
    public OnaylayiciTanimTipiEnum OnaylayiciTanimTipi { get; set; } = default!;

    public Guid? PersonelId { get; set; }
    public Personel? Personel { get; set; }

    public Guid? RolId { get; set; }
    public AppRole? Role { get; set; }

    public Guid? HedefKurumsalBirimId { get; set; }
    public KurumsalBirim? HedefKurumsalBirim { get; set; } 

    public Guid OnaySurecId { get; set; }
    public OnaySurec? OnaySurec { get; set; }
    public Guid TenantId { get; set; }
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
public sealed class OnaylayiciTanimTipiEnum : SmartEnum<OnaylayiciTanimTipiEnum>
{
    public static readonly OnaylayiciTanimTipiEnum BelirliKisi = new("BelirliKisi", 0);
    public static readonly OnaylayiciTanimTipiEnum GenelRol = new("GenelRol", 1);
    public static readonly OnaylayiciTanimTipiEnum YapisalRol_TalepEdenBirimi = new("YapisalRol_TalepEdenBirimi", 2);
    public static readonly OnaylayiciTanimTipiEnum YapisalRol_HedefBirim = new("YapisalRol_HedefBirim", 3);
    private OnaylayiciTanimTipiEnum(string name, int value) : base(name, value)
    {
    }
}
