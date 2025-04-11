using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Sirketler;

namespace PersonelYonetim.Server.Domain.Izinler;

public sealed class IzinKural : Entity
{
    public string Ad {  get; set; } = default!; 
    public string? Aciklama { get; set; }
    public Guid SirketId { get; set; }
    public Sirket Sirket { get; set; } = default!;
    public ICollection<PersonelIzinKural> PersonelIzinKurallar { get; set; } = new List<PersonelIzinKural>();
    public ICollection<IzinTur> IzinTurler { get; set; } = new List<IzinTur>();
}
