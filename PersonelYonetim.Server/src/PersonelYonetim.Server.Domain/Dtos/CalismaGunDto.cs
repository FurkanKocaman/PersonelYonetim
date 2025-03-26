namespace PersonelYonetim.Server.Domain.Dtos;
public sealed class CalismaGunDto
{
    public Guid Id { get; set; }
    public DayOfWeek Gun { get; set; }
    public TimeSpan? CalismaBaslangic { get; set; }
    public TimeSpan? CalismaBitis { get; set; }
    public TimeSpan? MolaBaslangic { get; set; }
    public TimeSpan? MolaBitis { get; set; }
    public bool IsCalismaGunu { get; set; } = true;
}

