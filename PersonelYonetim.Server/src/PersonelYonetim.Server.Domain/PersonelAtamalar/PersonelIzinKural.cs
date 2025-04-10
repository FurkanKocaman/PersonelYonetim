using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Sirketler;

namespace PersonelYonetim.Server.Domain.PersonelAtamalar;
public class PersonelIzinKural : Entity
{
    public Guid PersonelId { get; set; }
    public Personel? Personel { get; set; }
    public Guid IzinKuralId { get; set; }
    public IzinKural? IzinKural { get; set; }
    public Guid OnaySurecId { get; set; }
    public OnaySurec? OnaySurec { get; set; }
    public Guid SirketId { get; set; }
    public Sirket? Sirket { get; set; }
}
