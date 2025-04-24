using PersonelYonetim.Server.Domain.Bordro;
using PersonelYonetim.Server.Domain.PersonelDetaylar;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;

namespace PersonelYonetim.Server.Application.Services;
public interface IPDFService
{
    public  Task<byte[]> CreateBordroPdf(PersonelGorevlendirme personelGorevlendirme, PersonelDetay personelDetay, Tenant tenant, MaasPusula maasPusula);
}
