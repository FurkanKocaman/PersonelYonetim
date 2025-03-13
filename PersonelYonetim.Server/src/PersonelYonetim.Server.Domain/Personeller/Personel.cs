using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.PersonelDepartmanlar;

namespace PersonelYonetim.Server.Domain.Personeller;

public sealed class Personel : Entity
{
    public string Ad { get; set; } = default!;
    public string Soyad { get; set; } = default!;
    public string FullName => string.Join(" ", Ad, Soyad);
    public DateTimeOffset DogumTarihi { get; set; }
    public bool? Cinsiyet { get; set; }
    public string? ProfilResimUrl { get; set; }
    public Iletisim Iletisim { get; set; } = default!;
    public Adres Adres { get; set; } = default!;
    public ICollection<PersonelDepartman> PersonelDepartmanlar { get; set; } = default!;
}
