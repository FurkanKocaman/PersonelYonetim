using PersonelYönetim.Server.Domain.Abstractions;

namespace PersonelYönetim.Server.Domain.Personeller;

public sealed class Adres : Entity
{
    public string Ülke { get; set; } = default!;
    public string Şehir { get; set; } = default!;
    public string İlçe { get; set; } = default!;
    public string TamAdres { get; set; } = default!;
}
