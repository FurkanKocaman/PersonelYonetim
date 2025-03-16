using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.PersonelDepartmanlar;

namespace PersonelYonetim.Server.Domain.Pozisyonlar;

public sealed class Pozisyon : Entity
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public Guid DepartmanId { get; set; } = default!;
    public Departman Departman { get; set; } = default!;
    public ICollection<PersonelDepartman> PersonelDepartmanlar { get; set; } = default!;
}
