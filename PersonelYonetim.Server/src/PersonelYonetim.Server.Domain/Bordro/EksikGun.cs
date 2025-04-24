namespace PersonelYonetim.Server.Domain.Bordro;
public sealed class EksikGun
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid MaasPusulaId { get; set; } // MaasPusulasi.Id
    public MaasPusula MaasPusula { get; set; } = default!;
    public int GunSayisi { get; set; }
    public string? Aciklama { get; set; }
    public string? NedeniKodu { get; set; } // SGK kodları
    public string? MeslekKodu { get; set; } // ISCO 08 kodları
    public DateTimeOffset Tarih { get; set; }
    public Guid TenantId { get; set; }
}
