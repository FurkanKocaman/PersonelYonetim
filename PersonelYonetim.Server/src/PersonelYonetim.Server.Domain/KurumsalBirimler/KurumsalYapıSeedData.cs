namespace PersonelYonetim.Server.Domain.KurumsalBirimler;
public static class KurumsalYapiSeedData
{
    public static List<KurumsalBirimTipi> OlusturDefaultYapi(Guid tenantId)
    {
        var tipSirket = new KurumsalBirimTipi
        {
            Id = Guid.CreateVersion7(),
            Ad = "Şirket",
            Aciklama = "Ana Şirket / Holding",
            HiyerarsiSeviyesi = 1,
            YoneticisiOlabilirMi = true, // Şirketin de yöneticisi olabilir (CEO/Genel Müdür)
            CreatedAt = DateTimeOffset.Now,
            TenantId = tenantId
        };

        var tipSube = new KurumsalBirimTipi
        {
            Id = Guid.CreateVersion7(),
            Ad = "Şube",
            Aciklama = "Şirkete bağlı coğrafi veya işlevsel şube",
            HiyerarsiSeviyesi = 2,
            YoneticisiOlabilirMi = true, // Şube Müdürü
            CreatedAt = DateTimeOffset.Now,
            TenantId = tenantId
        };

        var tipDepartman = new KurumsalBirimTipi
        {
            Id = Guid.CreateVersion7(),
            Ad = "Departman",
            Aciklama = "Şube veya şirkete bağlı işlevsel departman",
            HiyerarsiSeviyesi = 3,
            YoneticisiOlabilirMi = true, // Departman Yöneticisi
            CreatedAt = DateTimeOffset.Now,
            TenantId = tenantId
        };
        var birimTipleri = new List<KurumsalBirimTipi> { tipSirket, tipSube, tipDepartman };
        return birimTipleri;

    }
}
