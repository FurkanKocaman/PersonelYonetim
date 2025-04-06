namespace PersonelYonetim.Server.Domain.Bildirimler;
public interface IBildirimService 
{
    public Task BildirimOlusturAsync(Bildirim bildirim);
    public Task KullaniciyaBildirimGonderAsync(Bildirim bildirim, Guid kullaniciId);
    public Task BirdenFazlaKullaniciyaBildirimGonderAsync(Bildirim bildirim, List<Guid> kullaniciIdler);
}
