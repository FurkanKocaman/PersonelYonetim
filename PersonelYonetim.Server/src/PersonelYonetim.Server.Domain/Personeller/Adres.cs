namespace PersonelYonetim.Server.Domain.Personeller;

public sealed class Adres
{
    public string Ulke { get; set; } = default!;
    public string Sehir { get; set; } = default!;
    public string Ilce { get; set; } = default!;
    public string TamAdres { get; set; } = default!;
}
