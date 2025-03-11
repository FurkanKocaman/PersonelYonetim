using PersonelYonetim.Server.Domain.Abstractions;

namespace PersonelYonetim.Server.Domain.Pozisyonlar;

public sealed class Pozisyon : Entity
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public Guid DepartmanId { get; set; } = default!;
}
