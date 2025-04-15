using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Rols;

namespace PersonelYonetim.Server.Domain.OnaySurecleri;
public sealed class TalepDegerlendirme
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid TalepId { get; private set; }
    public int AdimSirasi { get; private set; }
    public Guid OnaySureciAdimiId { get; private set; }
    public OnaySureciAdimi? OnaySureciAdimi { get; private set; }

    public Guid? AtananOnayciPersonelId { get; private set; }
    public Personel? AtananOnayciPersonel { get; private set; }
    public RolTipiEnum? AtananOnayciRol { get; private set; }


    public Guid? DegerlendirenId { get; private set; }
    public Personel? Degerlendiren { get; private set; }

    public DegerlendirmeDurumEnum DegerlendirmeDurumu { get; private set; } = DegerlendirmeDurumEnum.Beklemede;
    public DateTimeOffset? DegerlendirilmeTarihi { get; private set; }
    public string? Yorum { get; private set; }
    public Guid TenantId { get; set; }
    private TalepDegerlendirme() { }

    public TalepDegerlendirme(Guid talepId, int adimSirasi, Guid onaySureciAdimiId)
    {
        Id = Guid.CreateVersion7();
        TalepId = talepId;
        AdimSirasi = adimSirasi;
        OnaySureciAdimiId = onaySureciAdimiId;
        DegerlendirmeDurumu = DegerlendirmeDurumEnum.Beklemede;
    }

    public void OnayciAta(Guid? personelId, RolTipiEnum? rol)
    {
        AtananOnayciPersonelId = personelId;
        AtananOnayciRol = rol;
    }

    public void DurumuGuncelle(DegerlendirmeDurumEnum yeniDurum, Guid? degerlendirenId, string? yorum, DateTimeOffset degerlendirmeTarihi)
    {
        if (DegerlendirmeDurumu != DegerlendirmeDurumEnum.Beklemede && yeniDurum != DegerlendirmeDurumEnum.IptalEdildi)
        {
            throw new InvalidOperationException("Sadece 'Beklemede' durumundaki bir adım değerlendirilebilir.");
        }

        DegerlendirmeDurumu = yeniDurum;
        DegerlendirenId = degerlendirenId;
        Yorum = yorum;
        DegerlendirilmeTarihi = degerlendirmeTarihi;
    }

}