namespace PersonelYonetim.Server.Domain.CalismaTakvimleri;

public sealed class CalismaGun
{
    public CalismaGun()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }
    public DayOfWeek Gun { get; set; }
    public bool IsCalismaGunu { get; set; } = false;
    public TimeOnly? CalismaBaslangic { get; set; }
    public TimeOnly? CalismaBitis { get; set; }
    public TimeOnly? MolaBaslangic { get; set; }
    public TimeOnly? MolaBitis { get; set; }
    public Guid CalismaTakvimId { get; set; }
    public CalismaTakvimi CalismaTakvimi { get; set; } = default!;
    public Guid TenantId { get; set; }
}
