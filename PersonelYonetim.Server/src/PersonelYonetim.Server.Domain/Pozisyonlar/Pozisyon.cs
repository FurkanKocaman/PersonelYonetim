using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.Sirketler;

namespace PersonelYonetim.Server.Domain.Pozisyonlar;

public sealed class Pozisyon : Entity
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public Guid SirketId { get; set; } = default!;
    public Sirket Sirket { get; set; } = default!;
    public Guid? DepartmanId { get; set; }
    public Departman? Departman { get; set; }
    public Guid TenantId { get; set; }
}
