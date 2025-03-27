namespace PersonelYonetim.Server.Domain.Izinler;

public sealed class IzinTurIzinKural
{
    public Guid IzinTurId { get; set; }
    public IzinTur IzinTur { get; set; } = default!;

    public Guid IzinKuralId { get; set; }
    public IzinKural IzinKural { get; set; } = default!;
}
