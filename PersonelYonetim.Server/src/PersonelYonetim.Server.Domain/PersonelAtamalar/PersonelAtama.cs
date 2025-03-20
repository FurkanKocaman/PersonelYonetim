using Ardalis.SmartEnum;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Sirketler;
using PersonelYonetim.Server.Domain.Subeler;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonelYonetim.Server.Domain.PersonelAtamalar;

public sealed class PersonelAtama
{
    public PersonelAtama()
    {
        Id = Guid.CreateVersion7();
        PozisyonBaslamaTarihi = DateTimeOffset.Now;
    }
    public Guid Id { get; set; }
    public Guid PersonelId { get; set; }
    public Personel Personel { get; set; } = default!;
    public Guid? DepartmanId { get; set; }
    public Departman? Departman { get; set; }
    public Guid? PozisyonId { get; set; }
    public Pozisyon? Pozisyon { get; set; }
    public Guid? SubeId { get; set; }
    public Sube? Sube { get; set; }
    public Guid SirketId { get; set; }
    public Sirket Sirket { get; set; } = default!;
    public bool IsActive = true;
    public YoneticiTipiEnum? YoneticiTipi { get; set; }
    public CalismaSekliEnum CalismaSekli { get; set; } = default!;
    public SozlesmeTuruEnum SozlesmeTuru { get; set; } = default!;
    public DateTimeOffset? SozlesmeBitisTarihi { get; set; }
    public DateTimeOffset PozisyonBaslamaTarihi { get; set; } = default!;
    public DateTimeOffset? PozisyonBitisTarihi { get; set; }
}

public sealed class YoneticiTipiEnum : SmartEnum<YoneticiTipiEnum>
{
    public static readonly YoneticiTipiEnum Departman = new("Departman Yöneticisi", 0);
    public static readonly YoneticiTipiEnum Sube = new("Şube Yöneticisi", 1);
    public static readonly YoneticiTipiEnum Sirket = new("Şirket Yöneticisi", 2);
    private YoneticiTipiEnum(string name, int value) : base(name, value)
    {
    }
}
public sealed class CalismaSekliEnum : SmartEnum<CalismaSekliEnum>
{
    public static readonly CalismaSekliEnum TamZamanli = new("Tam Zamanlı", 0);
    public static readonly CalismaSekliEnum YariZamanli = new("Yari Zamanli", 1);
    public static readonly CalismaSekliEnum EsnekZamanli = new("Esnek Zamanli", 2);
    private CalismaSekliEnum(string name, int value) : base(name, value)
    {
    }
}

public sealed class SozlesmeTuruEnum : SmartEnum<SozlesmeTuruEnum>
{
    public static readonly SozlesmeTuruEnum Sureli = new("Süreli Sözleşme", 0);
    public static readonly SozlesmeTuruEnum Suresiz = new("Süresiz Sözleşme", 1);
    public static readonly SozlesmeTuruEnum Donemsel = new("Dönemsel Sözleşme", 2);
    public static readonly SozlesmeTuruEnum Esnek = new("Esnek Sözleşme", 3);
    private SozlesmeTuruEnum(string name, int value) : base(name, value)
    {
    }
}
