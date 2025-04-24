using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Personeller;

namespace PersonelYonetim.Server.Domain.PersonelDetaylar;

public sealed class PersonelDetay : Entity
{
    public Guid? PersonelId { get; set; }
    public Personel? Personel { get; set; }

    // Kimlik Bilgileri
    public string? TCKN { get; set; }
    public string? AnaAdi { get; set; }
    public string? BabaAdi { get; set; }
    public string? DogumYeri { get; set; }
    public string? MedeniHali { get; set; }
    public string? Uyruk { get; set; }

    // İletişim Bilgileri
    public string? IsTelefonu { get; set; }
    public string? EpostaIs { get; set; }
    public string? PostaKodu { get; set; }

    // Eğitim Bilgileri
    public string? EgitimDurumu { get; set; }
    public string? MezuniyetOkulu { get; set; }
    public string? MezuniyetBolumu { get; set; }
    public DateTime? MezuniyetTarihi { get; set; }

    // Askerlik Bilgileri
    public string? AskerlikDurumu { get; set; }
    public DateTime? AskerlikTarihi { get; set; }

    // Ehliyet Bilgileri
    public string? EhliyetSinifi { get; set; }
    public DateTime? EhliyetVerilisTarihi { get; set; }

    // Sağlık Bilgileri
    public bool EngelliMi { get; set; } = false;
    public int? EngelOrani { get; set; } = 0;
    public string? SaglikDurumu { get; set; }
    public string? KanGrubu { get; set; }

    // Acil Durum Bilgileri
    public string? AcilDurumKisiAdi { get; set; }
    public string? AcilDurumKisiTelefon { get; set; }
    public string? AcilDurumKisiYakinlik { get; set; }

    // Aile Bilgileri
    public int? CocukSayisi { get; set; }
    public bool? EsCalisiyorMu { get; set; }

    // Banka Bilgileri
    public string? BankaAdi { get; set; }
    public string? IBAN { get; set; }

    // Diğer
    public string? Notlar { get; set; }
    public Guid? TenantId { get; set; }
}

