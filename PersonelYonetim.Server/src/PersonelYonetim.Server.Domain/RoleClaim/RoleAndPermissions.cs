namespace PersonelYonetim.Server.Domain.RoleClaim;

public static class RoleClaims
{
    public const string Admin = "Admin";
    public const string SirketSahibi = "SirketSahibi";
    public const string Yonetici = "Yonetici";
    public const string Calisan = "Calisan";
}
public static class Permissions
{
    public const string ViewPersonel = "personeller.view";
    public const string CreatePersonel = "personeller.create";
    public const string EditPersonel = "personeller.edit";
    public const string DeletePersonel = "personeller.delete";

    public const string ViewSirket = "sirketler.view";
    public const string CreateSirket = "sirketler.create";
    public const string EditSirket = "sirketler.edit";
    public const string DeleteSirket = "sirketler.delete";

    public const string ViewSube = "subeler.view";
    public const string CreateSube = "subeler.create";
    public const string EditSube = "subeler.edit";
    public const string DeleteSube = "subeler.delete";

    public const string ViewDepartman = "departmanlar.view";
    public const string CreateDepartman = "departmanlar.create";
    public const string EditDepartman = "departmanlar.edit";
    public const string DeleteDepartman = "departmanlar.delete";

    public const string ViewPozisyon = "pozisyonlar.view";
    public const string CreatePozisyon = "pozisyonlar.create";
    public const string EditPozisyon = "pozisyonlar.edit";
    public const string DeletePozisyon = "pozisyonlar.delete";

    public const string ViewIzinler = "izinler.view";
    public const string CreateIzinler = "izinler.create";
    public const string ApproveIzinler = "izinler.approve";

    public const string ViewRaporlar = "raporlar.genel";
}

