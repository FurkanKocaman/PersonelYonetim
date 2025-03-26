using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Sirketler;

namespace PersonelYonetim.Server.Domain.CalismaTakvimleri;

public sealed class CalismaTakvimi : Entity
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public Guid SirketId { get; set; }
    public Sirket Sirket { get; set; } = default!; 
    public ICollection<CalismaGun> CalismaGunler {  get; set; } = new List<CalismaGun>();
    public ICollection<PersonelAtama> Personeller { get; set; } = new List<PersonelAtama>();
}
