using PersonelYönetim.Server.Domain.Abstractions;

namespace PersonelYönetim.Server.Domain.Departman;

public sealed class Departman : Entity
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
}
