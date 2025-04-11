using PersonelYonetim.Server.Domain.Sirketler;

namespace PersonelYonetim.Server.Domain.CalismaTakvimleri;
public static class DefaultCalismaTakvim
{
    public static CalismaTakvimi GetDefaultTamCalismaTakvim(Guid sirketId)
    {
        CalismaTakvimi calismaTakvim = new()
        {
            Ad = "Tam zamanlı çalışma takvimi",
            Aciklama = "Şirketler için default tam zamanlı çalışma takvimi",
            SirketId = sirketId
        };

        return calismaTakvim;
    }
    public static CalismaTakvimi GetDefaultYarimCalismaTakvim(Guid sirketId)
    {
        CalismaTakvimi calismaTakvim = new()
        {
            Ad = "Yari zamanlı çalışma takvimi",
            Aciklama = "Şirketler için default yarı zamanlı çalışma takvimi",
            SirketId = sirketId
        };

        return calismaTakvim;
    }
    public static List<CalismaGun> GetDefaultTamCalismaGunler(Guid CalismaTakvimId)
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
    public static List<CalismaGun> GetDefaultYariCalismaGunler(Guid calismaTakvimId)
    {
        var partTimeCalismaGunler = new List<CalismaGun>
    {
        new CalismaGun
        {
            Gun = DayOfWeek.Monday,
            IsCalismaGunu = true,
            CalismaBaslangic = TimeSpan.FromHours(9),
            CalismaBitis = TimeSpan.FromHours(13),
            CalismaTakvimId = calismaTakvimId
        },
        new CalismaGun
        {
            Gun = DayOfWeek.Tuesday,
            IsCalismaGunu = true,
            CalismaBaslangic = TimeSpan.FromHours(9),
            CalismaBitis = TimeSpan.FromHours(13),
            CalismaTakvimId = calismaTakvimId
        },
        new CalismaGun
        {
            Gun = DayOfWeek.Wednesday,
            IsCalismaGunu = true,
            CalismaBaslangic = TimeSpan.FromHours(9),
            CalismaBitis = TimeSpan.FromHours(13),
            CalismaTakvimId = calismaTakvimId
        },
        new CalismaGun
        {
            Gun = DayOfWeek.Thursday,
            IsCalismaGunu = true,
            CalismaBaslangic = TimeSpan.FromHours(9),
            CalismaBitis = TimeSpan.FromHours(13),
            CalismaTakvimId = calismaTakvimId
        },
        new CalismaGun
        {
            Gun = DayOfWeek.Friday,
            IsCalismaGunu = true,
            CalismaBaslangic = TimeSpan.FromHours(9),
            CalismaBitis = TimeSpan.FromHours(13),
            CalismaTakvimId = calismaTakvimId
        },
        new CalismaGun
        {
            Gun = DayOfWeek.Saturday,
            IsCalismaGunu = false,
            CalismaTakvimId = calismaTakvimId
        },
        new CalismaGun
        {
            Gun = DayOfWeek.Sunday,
            IsCalismaGunu = false,
            CalismaTakvimId = calismaTakvimId
        }
    };

        return partTimeCalismaGunler;
    }

}
