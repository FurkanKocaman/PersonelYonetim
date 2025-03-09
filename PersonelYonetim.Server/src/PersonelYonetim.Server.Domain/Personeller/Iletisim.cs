namespace PersonelYonetim.Server.Domain.Personeller;

public sealed record Iletisim
{
    public string Eposta { get; set; } = default!;
    public string Telefon { get; set; } = default!;
}
