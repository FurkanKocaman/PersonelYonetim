using Ardalis.SmartEnum;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.Personeller;
using TS.Result;

namespace PersonelYonetim.Server.Domain.Izinler;

public sealed class IzinTalep : Entity
{
    public Guid PersonelId { get; private set; }
    public Personel Personel { get; private set; } = default!;
    public DateTimeOffset BaslangicTarihi { get; private set; }
    public DateTimeOffset BitisTarihi { get; private set; }
    public DateTimeOffset MesaiBaslangicTarihi { get; private set; }
    public decimal ToplamSure { get; private set; }
    public Guid IzinTurId { get; private set; }
    public IzinTur IzinTur { get; private set; } = default!;
    public string? Aciklama { get; private set; }
    public Guid OnaySurecId { get; private set; } // Aktif hale getirildi
    public OnaySurec? OnaySurec { get; private set; } // İlişki

    private readonly List<TalepDegerlendirme> _degerlendirmeAdimlari = new();
    public IReadOnlyCollection<TalepDegerlendirme> DegerlendirmeAdimlari => _degerlendirmeAdimlari.AsReadOnly();
    public Guid TenantId { get; set; }

    private IzinTalep() { }

    public IzinTalep(
     Guid personelId,
     DateTimeOffset baslangicTarihi,
     DateTimeOffset bitisTarihi,
     DateTimeOffset mesaiBaslangicTarihi,
     decimal toplamSure,
     Guid izinTurId,
     Guid onaySurecId,
     string? aciklama,
     Guid tenantId)
    {
        PersonelId = personelId;
        BaslangicTarihi = baslangicTarihi;
        BitisTarihi = bitisTarihi;
        MesaiBaslangicTarihi = mesaiBaslangicTarihi;
        ToplamSure = toplamSure;
        IzinTurId = izinTurId;
        Aciklama = aciklama;
        OnaySurecId = onaySurecId;
        TenantId = tenantId;
    }

    public void DegerlendirmeAdimiEkle(TalepDegerlendirme adim)
    {
        _degerlendirmeAdimlari.Add(adim);
    }

    public Result<string> IptalEt(Guid iptalEdenPersonelId)
    {
        if (iptalEdenPersonelId != PersonelId)
        {
            return Result<string>.Failure("Sadece talep sahibi izni iptal edebilir.");
        }

        var mevcutDurum = GuncelDegerlendirmeDurumu();
        if (mevcutDurum == DegerlendirmeDurumEnum.Onaylandi || mevcutDurum == DegerlendirmeDurumEnum.Reddedildi)
        {
            return Result<string>.Failure("Tamamlanmış veya reddedilmiş bir talep iptal edilemez.");
        }

        foreach (var adim in _degerlendirmeAdimlari.Where(a => a.DegerlendirmeDurumu == DegerlendirmeDurumEnum.Beklemede))
        {
            adim.DurumuGuncelle(DegerlendirmeDurumEnum.IptalEdildi, null, "Talep Sahibi İptal Etti", DateTimeOffset.UtcNow);
        }

        return Result<string>.Succeed("");
    }

    public TalepDegerlendirme? AktifDegerlendirmeAdimi()
    {
        return _degerlendirmeAdimlari
            .OrderBy(a => a.AdimSirasi)
            .FirstOrDefault(a => a.DegerlendirmeDurumu == DegerlendirmeDurumEnum.Beklemede);
    }
    public DegerlendirmeDurumEnum GuncelDegerlendirmeDurumu()
    {
        if (!_degerlendirmeAdimlari.Any())
            return DegerlendirmeDurumEnum.Beklemede;

        if (_degerlendirmeAdimlari.Any(a => a.DegerlendirmeDurumu == DegerlendirmeDurumEnum.Reddedildi))
            return DegerlendirmeDurumEnum.Reddedildi;


        bool hepsiIptalMi = _degerlendirmeAdimlari.All(a => a.DegerlendirmeDurumu == DegerlendirmeDurumEnum.IptalEdildi || a.DegerlendirmeDurumu == DegerlendirmeDurumEnum.Onaylandi);
        if (hepsiIptalMi && _degerlendirmeAdimlari.Any(a => a.DegerlendirmeDurumu == DegerlendirmeDurumEnum.IptalEdildi))
            return DegerlendirmeDurumEnum.IptalEdildi;


        if (_degerlendirmeAdimlari.All(a => a.DegerlendirmeDurumu == DegerlendirmeDurumEnum.Onaylandi))
            return DegerlendirmeDurumEnum.Onaylandi;

        return DegerlendirmeDurumEnum.Beklemede;
    }

}


public sealed class DegerlendirmeDurumEnum : SmartEnum<DegerlendirmeDurumEnum>
{
    public static readonly DegerlendirmeDurumEnum Beklemede = new("Beklemede", 0);
    public static readonly DegerlendirmeDurumEnum Onaylandi = new("Onaylandı", 1);
    public static readonly DegerlendirmeDurumEnum Reddedildi = new("Reddedildi", 2);
    public static readonly DegerlendirmeDurumEnum IptalEdildi = new("İptal Edildi", 3);

    private DegerlendirmeDurumEnum(string name, int value) : base(name, value)
    {
    }
}
