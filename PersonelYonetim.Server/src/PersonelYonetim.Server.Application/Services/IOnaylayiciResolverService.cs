using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.Personeller;
using TS.Result;

namespace PersonelYonetim.Server.Application.Services;
public interface IOnaylayiciResolverService
{
    Task<Result<Guid?>> OnaylayiciGetirAsync(OnaySureciAdimi adim, Personel requester, CancellationToken cancellationToken);
}
