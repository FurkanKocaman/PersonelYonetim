using Ardalis.SmartEnum;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Sirketler;

namespace PersonelYonetim.Server.Domain.Izinler;

public sealed class IzinTur : Entity
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public bool UcretliMi { get; set; } = false;
    public LimitTipiEnum LimitTipi { get; set; } = default!;
    public int LimitGunSayisi {  get; set; }
    // Limit tipi 
    public EksiBakiyeHakkıEnum? EksiBakiyeHakkı { get; set; }
    public int EksiBakiyeLimit { get; set; }
    public HakEdisEnum HakEdis { get; set; } = default!;
    public bool HakEdisDonem { get; set; } = true; //Donem başında true /sonunda false
    public bool HakEdisBaslangic { get; set; } = false;//Yılın ilk günü true / işe başlama tarihi false
    public DevretmeTipiEnum? DevretmeTipi { get; set; }
    public int DevretmeGunLimit {  get; set; }
    public DateTimeOffset? DevretmelimitTarih { get; set; }
    public int KidemYil { get; set; }
    public int KidemArtıGun { get; set; }
    public int EnAzTalep {  get; set; }
    public int EnCokTalep { get; set; }
    //LimitTipi her talep için limitli ise sadece bu kısım alt taraf eğer yıl içinde limitli ise üset tarafta var
    public bool HesapSekli { get; set; } = true; // tamgün/yarımgün true  /  saatlik false
    public bool AciklamaZorunlu { get; set; } = false;
    public bool YerineBakacakZorunlu { get; set; } = false;
    public Guid SirketId { get; set; }
    public Sirket Sirket { get; set; } = default!;
    public Guid IzinKuralId { get; set; }
    public IzinKural? IzinKural { get; set; }
}

public sealed class LimitTipiEnum : SmartEnum<LimitTipiEnum>
{
    public static readonly LimitTipiEnum Limitsiz = new("Limitsiz", 0);
    public static readonly LimitTipiEnum TalepLimit = new("Her talep için limitli", 1);
    public static readonly LimitTipiEnum YılLimit = new("Yıl içinde limitli", 2);
    public LimitTipiEnum(string name, int value) : base(name, value)
    {
    }
}

public sealed class EksiBakiyeHakkıEnum : SmartEnum<EksiBakiyeHakkıEnum>
{
    public static readonly EksiBakiyeHakkıEnum HakYok = new("Eksi bakiye hakkı yok", 0);
    public static readonly EksiBakiyeHakkıEnum Limitsiz = new("Limitsiz eksi bakiye hakkı", 1);
    public static readonly EksiBakiyeHakkıEnum Limitli = new("Limitli eksi bakiye hakkı", 2);
    public EksiBakiyeHakkıEnum(string name, int value) : base(name, value)
    {
    }
}

public sealed class HakEdisEnum : SmartEnum<HakEdisEnum>
{
    public static readonly HakEdisEnum Yillik = new("Yıllık", 0);
    public static readonly HakEdisEnum Aylik = new("Aylık", 1);
    public static readonly HakEdisEnum Gunluk = new("Günlük", 2);
    public HakEdisEnum(string name, int value) : base(name, value)
    {
    }
}
public sealed class DevretmeTipiEnum : SmartEnum<DevretmeTipiEnum>
{
    public static readonly DevretmeTipiEnum Sifirlama = new("İzin hakkı sıfırlama", 0);
    public static readonly DevretmeTipiEnum Limitsiz = new("Limitsiz devretme", 1);
    public static readonly DevretmeTipiEnum LimitliToplamIzin = new("Limitli toplam izin hakkı", 2);
    public static readonly DevretmeTipiEnum LimitliDevretme = new("Limitli devretme", 3);
    public static readonly DevretmeTipiEnum TarihliLimitliDevretme = new("Geçerlilik tarihli limitli devretme", 4);
    public DevretmeTipiEnum(string name, int value) : base(name, value)
    {
    }
}
