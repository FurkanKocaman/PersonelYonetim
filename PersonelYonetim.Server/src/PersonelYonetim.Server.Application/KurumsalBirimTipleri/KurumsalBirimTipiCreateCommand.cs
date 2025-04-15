using MediatR;
using TS.Result;

namespace PersonelYonetim.Server.Application.KurumsalBirimTipleri;
public sealed record KurumsalBirimTipiCreateCommand(
    string Ad,
    string? Aciklama,
    int HiyerarsiSeviyesi,
    bool YoneticisiOlabilirMi,
    Guid? TenantId
    ) : IRequest<Result<string>>;

internal sealed class KurumsalBirimTipiCreateCommandHandler(
    ) : IRequestHandler<KurumsalBirimTipiCreateCommand, Result<string>>
{
    public Task<Result<string>> Handle(KurumsalBirimTipiCreateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
