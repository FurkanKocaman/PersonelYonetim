using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Personeller;

namespace PersonelYonetim.Server.Domain.Bordro;
public class MaasPusula  :Entity
{
    public Guid PersonelId { get; set; }
    public Personel? Personel { get; set; } 
    public Guid BordroDonemId { get; set; }
    public BordroDonem? BordroDonem { get; set; }

    // Kazanç Özetleri
    public decimal BrutUcret { get; set; } // Dönemlik ana maaş
    public decimal EkKazancToplam { get; set; } // Diğer kazançların toplamı
    public decimal ToplamBrutKazanc { get; set; } // Hesaplama A adımı (BrutUcret + EkKazancToplam)

    // Hesaplama Matrahları ve Ara Değerler
    public decimal SGKMatrahi { get; set; }//SGK primi ve İşsizlik primi için temel ToplamBrutKazanc üzerinden, SGK mevzuatına göre prime esas kazançtan istisna tutulan kazançlar (örneğin, yemek yardımının istisna kısmı ) düşülür. Hangi KazancBileseni'nin SGK matrahına dahil olup olmadığı (SGKMatrahinaDahil bayrağı) bu aşamada kullanılır.   
    public decimal GelirVergisiMatrahi { get; set; } // Hesaplama D adımı , MaasPusulasi.ToplamBrutKazanc değerinden, az önce hesaplanan MaasPusulasi.SGKPrimiIsci ve MaasPusulasi.IssizlikPrimiIsci tutarları düşülür. , Varsa, gelir vergisinden istisna olan diğer kazanç bileşenleri (KazancBileseni.GelirVergisiMatrahinaDahil bayrağı false olanlar) de düşülebilir.
    public decimal KumulatifGelirVergisiMatrahiOnceki { get; set; } // Bu dönem *öncesi* YTD GV matrahı
    public decimal KumulatifGelirVergisiMatrahiDonemSonu { get; set; } // Bu dönem *dahil* YTD GV matrahı
    public decimal HesaplananGelirVergisi { get; set; } // Hesaplama E adımı (İstisna öncesi) GelirVergisiMatrahi, ilgili vergi dilimi oranıyla çarpılarak MaasPusulasi.HesaplananGelirVergisi bulunur.
    public decimal GelirVergisiIstisnasiUygulanan { get; set; }// Hesaplama F adımı (O ay için geçerli TL tutar)
    //MaasPusulasi.ToplamBrutKazanc değeri alınır (Bu, istisna öncesi Damga Vergisi Matrahıdır - H adımı ). 
    public decimal HesaplananDamgaVergisi { get; set; } // Hesaplama K_gross adımı (İstisna öncesi) MaasPusulasi.ToplamBrutKazanc değeri alınır (Bu, istisna öncesi Damga Vergisi Matrahıdır - H adımı ).Bu tutar, binde 7,59 (0.00759) oranıyla çarpılarak MaasPusulasi.HesaplananDamgaVergisi bulunur(K_gross adımı)  
    public decimal DamgaVergisiIstisnasiUygulanan { get; set; } // O ay için geçerli sabit TL tutar O ay için geçerli olan Asgari Ücret Damga Vergisi İstisnası tutarı (Brüt asgari ücret * 0.00759, TL cinsinden sabit bir değer, PayrollParameter veya konfigürasyondan alınır ) MaasPusulasi.DamgaVergisiIstisnasiUygulanan alanına kaydedilir.

    // Kesinti Özetleri
    public decimal SGKPrimiIsci { get; set; } // Hesaplama B adımı Emekli değilse ? SGKMatrahi * %14 : SGKMatrahi * %7.5
    public decimal IssizlikPrimiIsci { get; set; } // Hesaplama C adımı Emekli değilse ? SGKMatrahi * %1 : 0
    public decimal OdenecekGelirVergisi { get; set; } // Hesaplama G adımı (E - F, min 0)
    public decimal OdenecekDamgaVergisi { get; set; } // Hesaplama K adımı (K_gross - DV İstisna, min 0) OdenecekDamgaVergisi = HesaplananDamgaVergisi - DamgaVergisiIstisnasiUygulanan (Sonuç negatifse 0 olarak alınır). Bu değer MaasPusulasi.OdenecekDamgaVergisi alanına kaydedilir. Bu, K adımıdır.
    public decimal DigerKesintilerToplam { get; set; } // Avans, icra vb. (KesintiBilesenleri'nden toplanabilir) Varsa çalışana ait diğer kesintiler (avans mahsubu, icra, nafaka, BES kesintisi vb.) KesintiBileseni (Kesinti Bileşeni) kayıtları olarak MaasPusulasi'na eklenir. Her birinin Tutar'ı ve KesintiTuruKodu belirtilir.Bu diğer kesintilerin toplamı MaasPusulasi.DigerKesintilerToplam alanında tutulabilir.
    public decimal ToplamKesinti { get; set; } // Tüm işçi kesintileri toplamı (B + C + G + K + DigerKesintiler)| MaasPusulasi içindeki tüm işçi kesintileri toplanır: SGKPrimiIsci + IssizlikPrimiIsci + OdenecekGelirVergisi + OdenecekDamgaVergisi + DigerKesintilerToplam
    public decimal NetMaas { get; set; } // ToplamBrutKazanc - ToplamKesinti

    // İşveren Maliyeti ve SGK/MUHSGK Bilgileri
    public decimal SGKPrimiIsveren { get; set; } // Hesaplama Y adımı Sosyal güvenlik matrahı * 0.2075
    public decimal IssizlikPrimiIsveren { get; set; } // Hesaplama Z adımı Sosyal güvenlik matrahı * 0.02 
    public decimal ToplamIsverenMaliyeti { get; set; } // ToplamBrutKazanc + Y + Z
    public int SGKGunSayisi { get; set; } // MUHSGK için gerekli
    public int EksikGunSayisi { get; set; } // MUHSGK için gerekli
    public string? EksikGunNedeniKodu { get; set; } // MUHSGK için gerekli (SGK kodları)
    public string? MeslekKodu { get; set; } // MUHSGK için gerekli (ISCO 08 kodları)
    public bool BesKesintisiVarMi { get; set; }
    public decimal BesKesintiTutari { get; set; }

    public  ICollection<KazancBilesen> KazancBilesenleri { get; set; } = new List<KazancBilesen>();
    public  ICollection<KesintiBilesen> KesintiBilesenleri { get; set; } = new List<KesintiBilesen>();

    public Guid TenantId { get; set; }

}
