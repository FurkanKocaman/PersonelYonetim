using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using TS.Result;

namespace PersonelYonetim.Server.Application.KurumsalBirimler;
public sealed record KurumsalBirimGetUstBirimler(
    Guid BirimTipiId
    ) : IRequest<Result<List<UstBirimDto>>>;

public sealed class UstBirimDto
{
    public Guid BirimTipiId { get; set; }
    public Guid BirimId { get; set; }
    public string BirimAd { get; set; } = default!;
}


internal sealed class KurumsalBirimGetUstBirimlerHandler(
    IKurumsalBirimTipiRepository kurumsalBirimTipiRepository,
    IKurumsalBirimRepository kurumsalBirimRepository,
    ICurrentUserService currentUserService
    ) : IRequestHandler<KurumsalBirimGetUstBirimler, Result<List<UstBirimDto>>>
{
    public Task<Result<List<UstBirimDto>>> Handle(KurumsalBirimGetUstBirimler request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Task.FromResult(Result<List<UstBirimDto>>.Failure("Tenant bulunamamdı"));

        var birimTipi = kurumsalBirimTipiRepository.Where(p => p.Id == request.BirimTipiId).FirstOrDefault();
        if (birimTipi is null)
            return Task.FromResult(Result<List<UstBirimDto>>.Failure("Birim tipi bulunamamdı"));

        var kurumsalBirimler = kurumsalBirimRepository.Where(p => p.BirimTipi.HiyerarsiSeviyesi <= birimTipi.HiyerarsiSeviyesi && p.TenantId == tenantId)
            .Include(p => p.BirimTipi)
            .Select(p => new UstBirimDto
            {
                BirimTipiId = p.BirimTipiId,
                BirimId = p.Id,
                BirimAd = p.Ad,
            }).ToList();

        return Task.FromResult(Result<List<UstBirimDto>>.Succeed(kurumsalBirimler));
    }
}
