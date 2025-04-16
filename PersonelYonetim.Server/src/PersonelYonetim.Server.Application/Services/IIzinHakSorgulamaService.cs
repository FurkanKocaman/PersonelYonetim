using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using TS.Result;

namespace PersonelYonetim.Server.Application.Services;
public interface IIzinHakSorgulamaService
{
    Task<Result<bool>> TalepHakkiKontrolEtAsync(
            Guid personelGorevlendirmeId,
            Guid izinTurId,
            decimal talepEdilenGun,
            DateTimeOffset talepBaslangicTarihi,
            CancellationToken cancellationToken);
}
