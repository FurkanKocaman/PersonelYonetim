using PersonelYönetim.Server.Domain.Abstractions;

namespace PersonelYönetim.Server.Domain.Pozisyon;

public sealed class Pozisyon : Entity
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
}
