namespace PersonelYonetim.Server.Domain.Izinler;
public sealed class IzinDegerlendirmeSurec
{
    public IzinDegerlendirmeSurec()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }
}
