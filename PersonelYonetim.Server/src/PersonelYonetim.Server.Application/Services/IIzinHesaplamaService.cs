using PersonelYonetim.Server.Domain.Izinler;
using TS.Result;

namespace PersonelYonetim.Server.Application.Services;
public interface IIzinHesaplamaService
{
    Task<Result<IzinHesaplamaResult>> HesaplaIzinSuresiAsync(
            Guid calismaTakvimId,
            DateTimeOffset baslangic,
            DateTimeOffset bitis,
            IzinTur izinTur,
            CancellationToken cancellationToken);

    public record IzinHesaplamaResult(decimal ToplamGun, DateTimeOffset MesaiBaslangicTarihi);
}
