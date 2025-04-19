namespace PersonelYonetim.Server.Domain.Dtos;
public sealed class FazlaMesaiPeriyotDto
{
    public Guid Id { get; set; }
    public Guid GunlukCalismaId { get; set; }
    public TimeOnly BaslangicSaati { get; set; }
    public TimeOnly BitisSaati { get; set; }
    public TimeSpan ToplamFazlaMesaiSuresi { get; set; }
}
