using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonelYonetim.Server.Domain.KurumsalBirimler;
public static class KurumsalYapiSeedData
{
    //public static (List<KurumsalBirimTipi> Tipler, List<KurumsalBirim> Birimler) OlusturDefaultYapi(Guid tenantId)
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

        //var anaSirket = new KurumsalBirim
        //{
        //    Id = Guid.NewGuid(),
        //    Ad = "Merkez Holding A.Ş.",
        //    Kod = "MHOLD",
        //    BirimTipiId = tipSirket.Id,
        //    BirimTipi = tipSirket,
        //    UstBirimId = null,
        //    UstBirim = null
        //};

        //var subeIstanbul = new KurumsalBirim
        //{
        //    Id = Guid.NewGuid(),
        //    Ad = "İstanbul Bölge Şubesi",
        //    Kod = "IST",
        //    BirimTipiId = tipSube.Id,
        //    BirimTipi = tipSube,
        //    UstBirimId = anaSirket.Id, // Ana şirkete bağlı
        //    UstBirim = anaSirket
        //};

        //var subeAnkara = new KurumsalBirim
        //{
        //    Id = Guid.NewGuid(),
        //    Ad = "Ankara Bölge Şubesi",
        //    Kod = "ANK",
        //    BirimTipiId = tipSube.Id,
        //    BirimTipi = tipSube,
        //    UstBirimId = anaSirket.Id, // Ana şirkete bağlı
        //    UstBirim = anaSirket
        //};

        //// İstanbul Şubesi Departmanları
        //var depIstIK = new KurumsalBirim
        //{
        //    Id = Guid.NewGuid(),
        //    Ad = "İnsan Kaynakları Departmanı (IST)",
        //    Kod = "IST-IK",
        //    BirimTipiId = tipDepartman.Id,
        //    BirimTipi = tipDepartman,
        //    UstBirimId = subeIstanbul.Id, // İstanbul şubesine bağlı
        //    UstBirim = subeIstanbul
        //};

        //var depIstYazilim = new KurumsalBirim
        //{
        //    Id = Guid.NewGuid(),
        //    Ad = "Yazılım Geliştirme Departmanı (IST)",
        //    Kod = "IST-YAZ",
        //    BirimTipiId = tipDepartman.Id,
        //    BirimTipi = tipDepartman,
        //    UstBirimId = subeIstanbul.Id, // İstanbul şubesine bağlı
        //    UstBirim = subeIstanbul
        //};

        //// Ankara Şubesi Departmanları
        //var depAnkMuhasebe = new KurumsalBirim
        //{
        //    Id = Guid.NewGuid(),
        //    Ad = "Muhasebe ve Finans Departmanı (ANK)",
        //    Kod = "ANK-FIN",
        //    BirimTipiId = tipDepartman.Id,
        //    BirimTipi = tipDepartman,
        //    UstBirimId = subeAnkara.Id, // Ankara şubesine bağlı
        //    UstBirim = subeAnkara
        //};

        //var depAnkSatis = new KurumsalBirim
        //{
        //    Id = Guid.NewGuid(),
        //    Ad = "Satış ve Pazarlama Departmanı (ANK)",
        //    Kod = "ANK-SAT",
        //    BirimTipiId = tipDepartman.Id,
        //    BirimTipi = tipDepartman,
        //    UstBirimId = subeAnkara.Id, // Ankara şubesine bağlı
        //    UstBirim = subeAnkara
        //};

        //var kurumsalBirimler = new List<KurumsalBirim>
        //{
        //    anaSirket
        //};

        //anaSirket.AltBirimler.Add(subeIstanbul);
        //anaSirket.AltBirimler.Add(subeAnkara);
        //subeIstanbul.AltBirimler.Add(depIstIK);
        //subeIstanbul.AltBirimler.Add(depIstYazilim);
        //subeAnkara.AltBirimler.Add(depAnkMuhasebe);
        //subeAnkara.AltBirimler.Add(depAnkSatis);

        //return (birimTipleri, kurumsalBirimler);
        return birimTipleri;

    }
}
