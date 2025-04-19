using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Personeller;

namespace PersonelYonetim.Server.Domain.ZamanYonetimler;
public class GunlukCalisma : Entity
{
    public Guid CalismaCizelgesiId { get; set; }
    public CalismaCizelge CalismaCizelgesi { get; set; } = default!;
    public DateTimeOffset Tarih { get; set; }
    public Guid PersonelId { get; set; } 
    public Personel Personel { get; set; } = default!;
    public ICollection<CalismaPeriyodu> CalismaPeriyotlari { get; set; } = new List<CalismaPeriyodu>();
    public ICollection<MolaPeriyodu> MolaPeriyotlari { get; set; } = new List<MolaPeriyodu>();
    public ICollection<IzinPeriyodu> IzinPeriyotlari { get; set; } = new List<IzinPeriyodu>();
    public ICollection<FazlaMesaiPeriyodu> FazlaMesaiPeriyotlari { get; set; } = new List<FazlaMesaiPeriyodu>();
    public Guid TenantId { get; set; }
}
public class CalismaPeriyodu
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid GunlukCalismaId { get; set; }
    public GunlukCalisma GunlukCalisma { get; set; } = default!;
    public CalismaPeriyoduTipi CalismaPeriyoduTipi { get; set; } = CalismaPeriyoduTipi.Normal;
    public TimeOnly BaslangicSaati { get; set; }
    public TimeOnly BitisSaati { get; set; }

    public TimeSpan ToplamCalismaSuresi { get; set; }
    public Guid TenantId { get; set; }
}
public enum CalismaPeriyoduTipi
{
    Normal,
    FazlaMesai
}
public class MolaPeriyodu
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid GunlukCalismaId { get; set; }
    public GunlukCalisma GunlukCalisma { get; set; } = default!;

    public TimeOnly BaslangicSaati { get; set; }
    public TimeOnly BitisSaati { get; set; }

    public TimeSpan ToplamMolaSuresi { get; set; }
    public Guid TenantId { get; set; }
}
public class IzinPeriyodu
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid GunlukCalismaId { get; set; }
    public GunlukCalisma GunlukCalisma { get; set; } = default!;

    public TimeOnly BaslangicSaati { get; set; }
    public TimeOnly BitisSaati { get; set; }

    public string IzinTipi { get; set; } = default!;  // Örneğin: "Mazeret", "Aylık İzin", vb.
    public Guid TenantId { get; set; }
}
public class FazlaMesaiPeriyodu
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid GunlukCalismaId { get; set; }
    public GunlukCalisma GunlukCalisma { get; set; } = default!;

    public TimeOnly BaslangicSaati { get; set; }
    public TimeOnly BitisSaati { get; set; }

    public TimeSpan ToplamFazlaMesaiSuresi { get; set; }
    public Guid TenantId { get; set; }
}