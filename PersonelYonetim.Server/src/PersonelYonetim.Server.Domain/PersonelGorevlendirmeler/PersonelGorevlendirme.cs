﻿using Ardalis.SmartEnum;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Rols;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
public sealed class PersonelGorevlendirme : Entity
{
    public Guid PersonelId { get; set; }
    public Personel Personel { get; set; } = default!;

    public Guid? KurumsalBirimId { get; set; }
    public KurumsalBirim? KurumsalBirim { get; set; }

    public Guid? PozisyonId { get; set; }
    public Pozisyon? Pozisyon { get; set; }

    public DateTimeOffset IseGirisTarihi { get; set; }
    public DateTimeOffset? IstenCikisTarihi { get; set; }

    public DateTimeOffset PozisyonBaslangicTarihi { get; set; }
    public DateTimeOffset? PozisyonBitisTarihi { get; set; }

    public bool BirincilGorevMi { get; set; } = true;
    public GorevlendirmeTipiEnum? GorevlendirmeTipi { get; set; }
    public CalismaSekliEnum? CalismaSekli { get; set; }

    public ICollection<GorevlendirmeRolu> GorevlendirmeRolleri { get; set; } = new List<GorevlendirmeRolu>();
    public GorevlendirmeIzinKurali? GorevlendirmeIzinKurali { get; set; }

    public Guid? RaporlananGorevlendirmeId { get; set; }
    public PersonelGorevlendirme? RaporlananGorevlendirme { get; set; }

    public Guid MesaiOnaySurecId { get; set; }
    public OnaySurec? MesaiOnaySurec { get; set; }

    public Guid CalismaTakvimId { get; set; }
    public CalismaTakvimi? CalismaTakvimi { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal BrutUcret { get; set; }
    public string? SGKIsyeri { get; set; }
    public string? SGKNumarasi { get; set; }
    public string? VergiDairesiAdi { get; set; }
    public string? VergiNumarasi { get; set; }
    public string? TabiOlduguKanun { get; set; }
    public string? MeslekKodu { get; set; }
    public decimal? BesKesintiOrani { get; set; }
    public bool BesKesintisiVarMi { get; set; } = false;
    public Guid TenantId { get; set; }
}

public class GorevlendirmeRolu : Entity
{
    public Guid PersonelGorevlendirmeId { get; set; }
    public PersonelGorevlendirme PersonelGorevlendirme { get; set; } = default!;

    public Guid RolId { get; set; }
    public AppRole Rol { get; set; } = default!;

}

public class GorevlendirmeIzinKurali : Entity
{
    public Guid PersonelGorevlendirmeId { get; set; }
    public PersonelGorevlendirme PersonelGorevlendirme { get; set; } = default!;
    public Guid IzinKuralId { get; set; }
    public IzinKural IzinKural { get; set; } = default!;
    public Guid? OzelOnaySurecId { get; set; }
    public OnaySurec? OzelOnaySurec { get; set; }
}

public sealed class GorevlendirmeTipiEnum : SmartEnum<GorevlendirmeTipiEnum>
{
    public static readonly GorevlendirmeTipiEnum Sureli = new("Süreli Sözleşme", 0);
    public static readonly GorevlendirmeTipiEnum Suresiz = new("Süresiz Sözleşme", 1);
    public static readonly GorevlendirmeTipiEnum Donemsel = new("Dönemsel Sözleşme", 2);
    public static readonly GorevlendirmeTipiEnum Esnek = new("Esnek Sözleşme", 3);
    private GorevlendirmeTipiEnum(string name, int value) : base(name, value)
    {
    }
}
public sealed class CalismaSekliEnum : SmartEnum<CalismaSekliEnum>
{
    public static readonly CalismaSekliEnum TamZamanli = new("Tam Zamanlı", 0);
    public static readonly CalismaSekliEnum YariZamanli = new("Yari zamanli", 1);
    private CalismaSekliEnum(string name, int value) : base(name, value)
    {
    }
}

