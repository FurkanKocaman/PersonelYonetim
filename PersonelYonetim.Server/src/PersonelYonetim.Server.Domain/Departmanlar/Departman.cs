using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.PersonelDepartmanlar;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Subeler;

namespace PersonelYonetim.Server.Domain.Departmanlar;

public sealed class Departman : Entity
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public Guid SubeId { get; set; } = default!;
    public Sube Sube { get; set; } = default!; 
    public ICollection<Pozisyon>? Pozisyonlar { get; set; }
    public ICollection<PersonelDepartman> PersonelDepartmanlar { get; set; } = default!;
}
