using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Users;

namespace PersonelYonetim.Server.Domain.Personeller;

public sealed class Personel : Entity
{
    public string Ad { get; set; } = default!;
    public string Soyad { get; set; } = default!;
    public string FullName => string.Join(" ", Ad, Soyad);
    public DateTimeOffset DogumTarihi { get; set; }
    public bool? Cinsiyet { get; set; }
    public string? AvatarUrl { get; set; }
    public Iletisim Iletisim { get; set; } = default!;
    public Adres Adres { get; set; } = default!;
    public Guid UserId { get; set; } = default!;
    public AppUser User { get; set; } = default!;
    public ICollection<PersonelAtama> PersonelAtamalar { get; set; } = default!;
    public ICollection<PersonelBildirim> PersonelBildirimler { get; set; } = new List<PersonelBildirim>();
}
