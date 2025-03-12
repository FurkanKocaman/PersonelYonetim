using PersonelYonetim.Server.Domain.Abstractions;

namespace PersonelYonetim.Server.Domain.Departmanlar;

public sealed class Departman : Entity
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
}
