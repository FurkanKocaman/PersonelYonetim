using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Pozisyonlar;

namespace PersonelYonetim.Server.Domain.PersonelDepartmanlar;

public sealed class PersonelDepartman
{
    public PersonelDepartman()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }
    public Guid PersonelId { get; set; }
    public Personel Personel { get; set; } = default!;
    public Guid DepartmanId { get; set; }
    public Departman Departman { get; set; } = default!;
    public Guid PozisyonId { get; set; }
    public Pozisyon Pozisyon { get; set; } = default!;
}
