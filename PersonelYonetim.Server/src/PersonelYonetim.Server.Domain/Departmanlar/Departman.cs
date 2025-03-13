using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.PersonelDepartmanlar;

namespace PersonelYonetim.Server.Domain.Departmanlar;

public sealed class Departman : Entity
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public ICollection<PersonelDepartman> PersonelDepartmanlar { get; set; } = default!;
}
