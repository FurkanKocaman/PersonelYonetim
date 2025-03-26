using PersonelYonetim.Server.Domain.Personeller;

namespace PersonelYonetim.Server.Domain.PersonelIzinler;
public sealed class PersonelIzin
{
    public PersonelIzin()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }
    public Guid PersonelId { get; set; }
    public Personel Personel { get; set; } = default!;
    public Guid SirketId { get; set; }
    public decimal ToplamIzin { get; set; }
    public decimal KullanilanIzin { get; set; }
    public decimal KalanIzin => ToplamIzin - KullanilanIzin;
}
