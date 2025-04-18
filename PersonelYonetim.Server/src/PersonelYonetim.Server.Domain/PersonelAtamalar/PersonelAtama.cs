﻿using Ardalis.SmartEnum;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Domain.Sirketler;
using PersonelYonetim.Server.Domain.Subeler;

namespace PersonelYonetim.Server.Domain.PersonelAtamalar;

public sealed class PersonelAtama : Entity
{
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
    public Guid? YoneticiId { get; set; }
    public Personel? Yonetici { get; set; }
    public RolTipiEnum RolTipi { get; set; } = default!;
    public SozlesmeTuruEnum SozlesmeTuru { get; set; } = default!;
    public DateTimeOffset? SozlesmeBitisTarihi { get; set; }
    public DateTimeOffset PozisyonBaslamaTarihi { get; set; } = DateTimeOffset.Now;
    public Guid? IzinKuralId { get; set; }
    public IzinKural? IzinKural { get; set; }
    public Guid? CalismaTakvimId { get; set; }
    public CalismaTakvimi CalismaTakvimi{ get; set; } = default!;
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
