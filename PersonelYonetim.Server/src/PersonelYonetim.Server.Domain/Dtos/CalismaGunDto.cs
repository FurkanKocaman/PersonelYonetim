namespace PersonelYonetim.Server.Domain.Dtos;
public sealed class CalismaGunDto
{
    public Guid Id { get; set; }
    public DayOfWeek Gun { get; set; }
    public TimeOnly? CalismaBaslangic { get; set; }
    public TimeOnly? CalismaBitis { get; set; }
    public TimeOnly? MolaBaslangic { get; set; }
    public TimeOnly? MolaBitis { get; set; }
    public bool IsCalismaGunu { get; set; } = true;
}

