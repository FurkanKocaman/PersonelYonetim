using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using TS.Result;

namespace PersonelYonetim.Server.Application.Services;
public interface IOnaylayiciResolverService
{
    Task<Result<Guid?>> OnaylayiciGetirAsync(OnaySureciAdimi adim, PersonelGorevlendirme requesterAssignment, CancellationToken cancellationToken);
}
