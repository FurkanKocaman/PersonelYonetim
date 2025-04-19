namespace PersonelYonetim.Server.Domain.Dtos;
public sealed class MolaPeriyotDto
{
    public Guid Id { get; set; } 
    public Guid GunlukCalismaId { get; set; }
    public TimeOnly BaslangicSaati { get; set; }
    public TimeOnly BitisSaati { get; set; }
    public TimeSpan ToplamMolaSuresi { get; set; }
}
