using PersonelYonetim.Server.Domain.Sirketler;
using PersonelYonetim.Server.Domain.Tenants;

namespace PersonelYonetim.Server.Domain.CalismaTakvimleri;
public static class DefaultCalismaTakvim
{
    public static CalismaTakvimi GetDefaultTamCalismaTakvim(Guid tenantId)
    {
        CalismaTakvimi calismaTakvim = new()
        {
            Ad = "Tam zamanlı çalışma takvimi",
            Aciklama = "Şirketler için default tam zamanlı çalışma takvimi",
            TenantId = tenantId,
        };

        return calismaTakvim;
    }
    public static CalismaTakvimi GetDefaultYarimCalismaTakvim(Guid tenantId)
    {
        CalismaTakvimi calismaTakvim = new()
        {
            Ad = "Yari zamanlı çalışma takvimi",
            Aciklama = "Şirketler için default yarı zamanlı çalışma takvimi",
            TenantId = tenantId,
        };

        return calismaTakvim;
    }
    public static List<CalismaGun> GetDefaultTamCalismaGunler(Guid calismaTakvimId, Guid tenantId)
    {
        var defaultCalismaGunler = new List<CalismaGun>
        {
            new CalismaGun
            {
                Gun = DayOfWeek.Monday,
                IsCalismaGunu = true,
                CalismaBaslangic = new TimeOnly(9, 0),
                CalismaBitis = new TimeOnly(18, 0),
                MolaBaslangic = new TimeOnly(12, 30),
                MolaBitis = new TimeOnly(13, 30),
                CalismaTakvimId = calismaTakvimId,
                TenantId = tenantId,
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Tuesday,
                IsCalismaGunu = true,
                CalismaBaslangic = new TimeOnly(9, 0),
                CalismaBitis = new TimeOnly(18, 0),
                MolaBaslangic = new TimeOnly(12, 30),
                MolaBitis = new TimeOnly(13, 30),
                CalismaTakvimId = calismaTakvimId,
                TenantId = tenantId,
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Wednesday,
                IsCalismaGunu = true,
                CalismaBaslangic = new TimeOnly(9, 0),
                CalismaBitis = new TimeOnly(18, 0),
                MolaBaslangic = new TimeOnly(12, 30),
                MolaBitis = new TimeOnly(13, 30),
                CalismaTakvimId = calismaTakvimId,
                TenantId = tenantId,
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Thursday,
                IsCalismaGunu = true,
                CalismaBaslangic = new TimeOnly(9, 0),
                CalismaBitis = new TimeOnly(18, 0),
                MolaBaslangic = new TimeOnly(12, 30),
                MolaBitis = new TimeOnly(13, 30),
                CalismaTakvimId = calismaTakvimId,
                TenantId = tenantId,
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Friday,
                IsCalismaGunu = true,
                CalismaBaslangic = new TimeOnly(9, 0),
                CalismaBitis = new TimeOnly(18, 0),
                MolaBaslangic = new TimeOnly(12, 30),
                MolaBitis = new TimeOnly(13, 30),
                CalismaTakvimId = calismaTakvimId,
                TenantId = tenantId,
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Saturday,
                IsCalismaGunu = false,
                CalismaTakvimId = calismaTakvimId,
                TenantId = tenantId,
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Sunday,
                IsCalismaGunu = false,
                CalismaTakvimId = calismaTakvimId,
                TenantId = tenantId,
            }
        };

        return defaultCalismaGunler;
    }
    public static List<CalismaGun> GetDefaultYariCalismaGunler(Guid calismaTakvimId, Guid tenantId)
    {
        var partTimeCalismaGunler = new List<CalismaGun>
        {
            new CalismaGun
            {
                Gun = DayOfWeek.Monday,
                IsCalismaGunu = true,
                CalismaBaslangic = new TimeOnly(9, 0),
                CalismaBitis = new TimeOnly(13, 0),
                CalismaTakvimId = calismaTakvimId,
                TenantId = tenantId,
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Tuesday,
                IsCalismaGunu = true,
                CalismaBaslangic = new TimeOnly(9, 0),
                CalismaBitis = new TimeOnly(13, 0),
                CalismaTakvimId = calismaTakvimId,
                TenantId = tenantId,
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Wednesday,
                IsCalismaGunu = true,
                CalismaBaslangic = new TimeOnly(9, 0),
                CalismaBitis = new TimeOnly(13, 0),
                CalismaTakvimId = calismaTakvimId,
                TenantId = tenantId,
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Thursday,
                IsCalismaGunu = true,
                CalismaBaslangic = new TimeOnly(9, 0),
                CalismaBitis = new TimeOnly(13, 0),
                CalismaTakvimId = calismaTakvimId,
                TenantId = tenantId,
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Friday,
                IsCalismaGunu = true,
                CalismaBaslangic = new TimeOnly(9, 0),
                CalismaBitis = new TimeOnly(13, 0),
                CalismaTakvimId = calismaTakvimId,
                TenantId = tenantId,
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Saturday,
                IsCalismaGunu = false,
                CalismaTakvimId = calismaTakvimId,
                TenantId = tenantId,
            },
            new CalismaGun
            {
                Gun = DayOfWeek.Sunday,
                IsCalismaGunu = false,
                CalismaTakvimId = calismaTakvimId,
                TenantId = tenantId,
            }
        };

        return partTimeCalismaGunler;
    }

}
