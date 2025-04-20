using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.Personeller;
using TS.Result;

namespace PersonelYonetim.Server.Domain.OnaySurecleri;
public sealed class TalepDegerlendirme
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid TalepId { get; private set; }
    public int AdimSirasi { get; private set; }
    public OnaySurecTuruEnum TalepTipi { get; set; } = default!;
    public Guid OnaySureciAdimiId { get; private set; }
    public OnaySureciAdimi? OnaySureciAdimi { get; private set; }

    public Guid? AtananOnayciPersonelId { get; private set; }
    public Personel? AtananOnayciPersonel { get; private set; }
    public Guid? AtananOnayciRolId { get; private set; }


    public Guid? DegerlendirenId { get; private set; }
    public Personel? Degerlendiren { get; private set; }

    public DegerlendirmeDurumEnum DegerlendirmeDurumu { get; private set; } = DegerlendirmeDurumEnum.Beklemede;
    public DateTimeOffset? DegerlendirilmeTarihi { get; private set; }
    public string? Yorum { get; private set; }
    public Guid TenantId { get; set; }
    private TalepDegerlendirme() { }

    public TalepDegerlendirme(Guid talepId, int adimSirasi, Guid onaySureciAdimiId, OnaySurecTuruEnum talepTipi, Guid tenantId)
    {
        Id = Guid.CreateVersion7();
        TalepId = talepId;
        AdimSirasi = adimSirasi;
        OnaySureciAdimiId = onaySureciAdimiId;
        TalepTipi = talepTipi;
        DegerlendirmeDurumu = DegerlendirmeDurumEnum.Beklemede;
        TenantId = tenantId;
    }

    public void OnayciAta(Guid? personelId, Guid? rolId)
    {
        AtananOnayciPersonelId = personelId;
        AtananOnayciRolId = rolId;
    }

    public Result<string> DurumuGuncelle(DegerlendirmeDurumEnum yeniDurum, Guid? degerlendirenId, string? yorum, DateTimeOffset degerlendirmeTarihi)
    {
        if (DegerlendirmeDurumu != DegerlendirmeDurumEnum.Beklemede && yeniDurum != DegerlendirmeDurumEnum.IptalEdildi)
        {
            return Result<string>.Failure("Sadece 'Beklemede' durumundaki bir adım değerlendirilebilir.");
        }

        DegerlendirmeDurumu = yeniDurum;
        DegerlendirenId = degerlendirenId;
        Yorum = yorum;
        DegerlendirilmeTarihi = degerlendirmeTarihi;
        return Result<string>.Succeed("Başarıyla izin durumu güncellendi");
    }
}