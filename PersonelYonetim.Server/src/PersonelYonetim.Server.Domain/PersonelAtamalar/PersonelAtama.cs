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
    public YoneticiTipiEnum? YoneticiTipi { get; set; } = default!;
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
