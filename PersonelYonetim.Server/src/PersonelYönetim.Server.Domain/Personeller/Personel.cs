using PersonelYönetim.Server.Domain.Abstractions;

namespace PersonelYönetim.Server.Domain.Personeller;

public sealed class Personel : Entity
{
    public string Ad { get; set; } = default!;
    public string Soyad { get; set; } = default!;
    public string FullName => string.Join(" ", Ad, Soyad);
    public DateTimeOffset DogumTarihi { get; set; }
    public bool? Cinsiyet { get; set; }
    public string? ProfilResimUrl { get; set; }
    public Iletisim Iletisim { get; set; } = default!;
    public Guid? AdresId { get; set; }
    public Guid DepartmanId { get; set; } = default!;
    public Guid PozisyonId { get; set; } = default!;
}
