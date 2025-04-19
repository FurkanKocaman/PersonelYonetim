namespace PersonelYonetim.Server.Domain.Dtos;
public sealed class CalismaPeriyotDto
{
    public Guid Id { get; set; }
    public Guid GunlukCalismaId { get; set; }
    public string CalismaTipi { get; set; } = string.Empty;
    public TimeOnly BaslangicSaati { get; set; }
    public TimeOnly BitisSaati { get; set; }
    public TimeSpan ToplamCalismaSuresi { get; set; }
}
