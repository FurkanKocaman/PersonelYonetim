namespace PersonelYonetim.Server.Domain.Dtos;
public sealed class IzinPeriyotDto
{
    public Guid Id { get; set; }
    public Guid GunlukCalismaId { get; set; }
    public TimeOnly BaslangicSaati { get; set; }
    public TimeOnly BitisSaati { get; set; }
    public string IzinTipi { get; set; } = default!;  // Örneğin: "Mazeret", "Aylık İzin", vb.
}
