using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.KurumsalBirimTipleri;
public sealed record KurumsalBirimTipiCreateCommand(
    string Ad,
    string? Aciklama,
    int HiyerarsiSeviyesi,
    bool YoneticisiOlabilirMi
    ) : IRequest<Result<string>>;

internal sealed class KurumsalBirimTipiCreateCommandHandler(
    IKurumsalBirimTipiRepository kurumsalBirimTipiRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<KurumsalBirimTipiCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(KurumsalBirimTipiCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<string>.Failure("tenant bulunamamdı");

        var eskiTipler = await kurumsalBirimTipiRepository.WhereWithTracking(p => p.HiyerarsiSeviyesi >= request.HiyerarsiSeviyesi && p.TenantId == tenantId).ToListAsync();

        foreach(var tip in eskiTipler)
        {
            tip.HiyerarsiSeviyesi += 1;
        }

        KurumsalBirimTipi kurumsalBirimTipi = request.Adapt<KurumsalBirimTipi>();
        kurumsalBirimTipi.TenantId = tenantId.Value;

        kurumsalBirimTipiRepository.Add(kurumsalBirimTipi);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Kurumsal birim tipi oluşturuldu");
    }
}
