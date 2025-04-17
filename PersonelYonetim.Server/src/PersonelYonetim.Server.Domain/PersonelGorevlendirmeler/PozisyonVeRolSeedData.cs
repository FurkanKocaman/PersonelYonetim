using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Rols;

namespace PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
public static class PozisyonVeRolSeedData
{
    public static readonly Guid RolIdBirimYoneticisi = Guid.CreateVersion7();
    public static readonly Guid RolIdOnayYetkilisi = Guid.CreateVersion7();
    public static readonly Guid RolIdIkYetkilisi = Guid.CreateVersion7();

    public static (List<Pozisyon> Pozisyonlar, List<AppRole> Roller,Pozisyon genelMudur, AppRole yonetici) OlusturDefaultPozisyonVeRoller(Guid tenantId)
    {
        var pozisyonlar = new List<Pozisyon>
        {
            // Yönetim Seviyeleri
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Genel Müdür", Kod = "GM", CreatedAt = DateTimeOffset.Now, TenantId = tenantId},
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Direktör", Kod = "DIR", CreatedAt = DateTimeOffset.Now , TenantId = tenantId},
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Şube Müdürü", Kod = "SM" , CreatedAt = DateTimeOffset.Now, TenantId = tenantId},
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Departman Müdürü", Kod = "DM" , CreatedAt = DateTimeOffset.Now, TenantId = tenantId},
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Ekip Lideri", Kod = "EL" , CreatedAt = DateTimeOffset.Now, TenantId = tenantId},

            // Uzmanlık Seviyeleri
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Uzman", Kod = "UZM" , CreatedAt = DateTimeOffset.Now, TenantId = tenantId},
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Kıdemli Uzman", Kod = "KUZM" , CreatedAt = DateTimeOffset.Now, TenantId = tenantId},
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Yardımcı Uzman", Kod = "YUZM", CreatedAt = DateTimeOffset.Now, TenantId = tenantId},

            // Departman Bazlı Örnekler
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Yazılım Geliştirici", Kod = "YG", CreatedAt = DateTimeOffset.Now , TenantId = tenantId},
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Kıdemli Yazılım Geliştirici", Kod = "KYG" , CreatedAt = DateTimeOffset.Now, TenantId = tenantId},
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Sistem Yöneticisi", Kod = "SYSADM" , CreatedAt = DateTimeOffset.Now, TenantId = tenantId},
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "İnsan Kaynakları Uzmanı", Kod = "IKUZM", CreatedAt = DateTimeOffset.Now , TenantId = tenantId},
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Muhasebe Uzmanı", Kod = "MUHU" , CreatedAt = DateTimeOffset.Now, TenantId = tenantId},
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Finans Uzmanı", Kod = "FINUZM" , CreatedAt = DateTimeOffset.Now, TenantId = tenantId},
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Satış Temsilcisi", Kod = "ST", CreatedAt = DateTimeOffset.Now , TenantId = tenantId},
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Pazarlama Uzmanı", Kod = "PAZUZM", CreatedAt = DateTimeOffset.Now , TenantId = tenantId},
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Operasyon Sorumlusu", Kod = "OPS", CreatedAt = DateTimeOffset.Now , TenantId = tenantId},

            // Diğer
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Yönetici Asistanı", Kod = "ASST", CreatedAt = DateTimeOffset.Now , TenantId = tenantId},
            new Pozisyon { Id = Guid.CreateVersion7(), Ad = "Stajyer", Kod = "STJ", CreatedAt = DateTimeOffset.Now , TenantId = tenantId}
        };

        var genelMudur = pozisyonlar.FirstOrDefault(p => p.Kod == "GM");

        var roller = new List<AppRole>
        {
            // Yapısal Roller
            new AppRole {
                Id = RolIdBirimYoneticisi, 
                Name = "Birim Yöneticisi",
                Aciklama = "Atandığı kurumsal birimin (Şirket, Şube, Departman vb.) yönetiminden sorumlu rol.",
                YapisalRolMu = true, 
                CreatedAt = DateTimeOffset.Now,
                TenantId = tenantId
            },

            // Onay Rolleri
            new AppRole {
                Id = RolIdOnayYetkilisi, 
                Name = "Onay Yetkilisi (Genel)",
                Aciklama = "İzin, mesai vb. talepleri onaylama yetkisine sahip genel rol.",
                YapisalRolMu = false,
                CreatedAt = DateTimeOffset.Now,
                TenantId = tenantId
            },
             new AppRole {
                Id = RolIdIkYetkilisi,
                Name = "İK Yetkilisi",
                Aciklama = "İnsan Kaynakları süreçlerinde işlem yapma ve onay verme yetkisi.",
                YapisalRolMu = false,
                CreatedAt = DateTimeOffset.Now,
                TenantId = tenantId
            },
            new AppRole {
                Id = Guid.CreateVersion7(),
                Name = "Finans Onay Yetkilisi",
                Aciklama = "Finansal talepleri (avans, masraf vb.) onaylama yetkisi.",
                YapisalRolMu = false,
                CreatedAt = DateTimeOffset.Now,
                TenantId = tenantId
            },

            // Fonksiyonel / Sistem Rolleri
            new AppRole {
                Id = Guid.CreateVersion7(),
                Name = "Proje Lideri",
                Aciklama = "Atandığı projelerin yönetiminden sorumlu.",
                YapisalRolMu = false,
                CreatedAt = DateTimeOffset.Now,
                TenantId = tenantId
            },
             new AppRole {
                Id = Guid.CreateVersion7(),
                Name = "Sistem Yöneticisi (Admin)",
                Aciklama = "Sistem genelinde yönetim yetkilerine sahip rol.",
                YapisalRolMu = false,
                CreatedAt = DateTimeOffset.Now,
                TenantId = tenantId
             },
            new AppRole {
                Id = Guid.CreateVersion7(),
                Name = "Standart Kullanıcı",
                Aciklama = "Sisteme giriş yapabilen ve temel işlemleri gerçekleştirebilen kullanıcı.",
                YapisalRolMu = false,
                CreatedAt = DateTimeOffset.Now,
                TenantId = tenantId
            },
             new AppRole {
                Id = Guid.CreateVersion7(),
                Name = "Rapor Görüntüleyici",
                Aciklama = "Sistemdeki raporları görüntüleme yetkisi.",
                YapisalRolMu = false,
                CreatedAt = DateTimeOffset.Now,
                TenantId = tenantId
            }
        };
        var yonetici = roller.FirstOrDefault(p => p.Name == "Birim Yöneticisi");
        return (pozisyonlar, roller,genelMudur!,yonetici!);
    }
}
