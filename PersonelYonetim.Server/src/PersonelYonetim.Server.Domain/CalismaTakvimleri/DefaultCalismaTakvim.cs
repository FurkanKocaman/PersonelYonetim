namespace PersonelYonetim.Server.Domain.CalismaTakvimleri;
public static class DefaultCalismaTakvim
{
    public static CalismaTakvimi GetDefaultCalismaTakvim(Guid sirketId)
    {
        CalismaTakvimi calismaTakvimi = new()
        {
            Ad = "Default Çalışma takvimi",
            Aciklama = "Şirketler için default çalışma takvimi",
            SirketId = sirketId
        };

        return calismaTakvimi;
    }
    public static List<CalismaGun> GetDefaultCalismaGunler(Guid CalismaTakvimId)
    {
        var defaultCalismaGunler = new List<CalismaGun>
        {
            new CalismaGun
            {
                Gun = DayOfWeek.Monday,
                IsCalismaGunu = true,
                CalismaBaslangic = TimeSpan.FromHours(9),
                CalismaBitis = TimeSpan.FromHours(18),
                MolaBaslangic = TimeSpan.FromHours(12) + TimeSpan.FromMinutes(30),
                MolaBitis = TimeSpan.FromHours(13) + TimeSpan.FromMinutes(30),
                CalismaTakvimId = CalismaTakvimId,
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Tuesday,
                IsCalismaGunu = true,
                CalismaBaslangic = TimeSpan.FromHours(9),
                CalismaBitis = TimeSpan.FromHours(18),
                MolaBaslangic = TimeSpan.FromHours(12) + TimeSpan.FromMinutes(30),
                MolaBitis = TimeSpan.FromHours(13) + TimeSpan.FromMinutes(30),
                CalismaTakvimId = CalismaTakvimId,
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Wednesday,
                IsCalismaGunu = true,
                CalismaBaslangic = TimeSpan.FromHours(9),
                CalismaBitis = TimeSpan.FromHours(18),
                MolaBaslangic = TimeSpan.FromHours(12) + TimeSpan.FromMinutes(30),
                MolaBitis = TimeSpan.FromHours(13) + TimeSpan.FromMinutes(30),
                CalismaTakvimId = CalismaTakvimId,
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Thursday,
                IsCalismaGunu = true,
                CalismaBaslangic = TimeSpan.FromHours(9),
                CalismaBitis = TimeSpan.FromHours(18),
                MolaBaslangic = TimeSpan.FromHours(12) + TimeSpan.FromMinutes(30),
                MolaBitis = TimeSpan.FromHours(13) + TimeSpan.FromMinutes(30),
                CalismaTakvimId = CalismaTakvimId,
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Friday,
                IsCalismaGunu = true,
                CalismaBaslangic = TimeSpan.FromHours(9),
                CalismaBitis = TimeSpan.FromHours(18),
                MolaBaslangic = TimeSpan.FromHours(12) + TimeSpan.FromMinutes(30),
                MolaBitis = TimeSpan.FromHours(13) + TimeSpan.FromMinutes(30),
                CalismaTakvimId = CalismaTakvimId
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Saturday,
                IsCalismaGunu = false,
                CalismaTakvimId = CalismaTakvimId
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Sunday,
                IsCalismaGunu = false,
                CalismaTakvimId = CalismaTakvimId
            }
        };

        return defaultCalismaGunler;
    }
}
