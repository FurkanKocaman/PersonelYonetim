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
    public TimeSpan? CalismaBaslangic { get; set; }
    public TimeSpan? CalismaBitis { get; set; }
    public TimeSpan? MolaBaslangic { get; set; }
    public TimeSpan? MolaBitis { get; set; }
    public Guid CalismaTakvimId { get; set; }
    public CalismaTakvimi CalismaTakvimi { get; set; } = default!;
}
