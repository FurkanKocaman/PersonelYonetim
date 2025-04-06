using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.UnitOfWork;

namespace PersonelYonetim.Server.Infrastructure.Services;
internal class BildirimService(
    IBildirimRepository bildirimRepository,
    IPersonelBildirimRepository personelBildirimRepository,
    IUnitOfWork unitOfWork) : IBildirimService
{
    public async Task BildirimOlusturAsync(Bildirim bildirim)
    {
        bildirimRepository.Add(bildirim);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task BirdenFazlaKullaniciyaBildirimGonderAsync(Bildirim bildirim, List<Guid> kullaniciIdler)
    {
        await BildirimOlusturAsync(bildirim);

        foreach (var kullaniciId in kullaniciIdler)
        {
            var personelBildirim = new PersonelBildirim
            {
                BildirimId = bildirim.Id,
                PersonelId = kullaniciId
            };
            personelBildirimRepository.Add(personelBildirim);
        }
        await unitOfWork.SaveChangesAsync();
    }

    public async Task KullaniciyaBildirimGonderAsync(Bildirim bildirim, Guid kullaniciId)
    {
        await BildirimOlusturAsync(bildirim);

        var personelBildirim = new PersonelBildirim
        {
            BildirimId = bildirim.Id,
            PersonelId = kullaniciId
        };

        personelBildirimRepository.Add(personelBildirim);
        await unitOfWork.SaveChangesAsync();
    }
}
