using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.KurumsalBirimler;
public sealed record KurumsalBirimTipiDeleteCommand(
    Guid Id
    ) : IRequest<Result<string>>;

internal sealed class KurumsalBirimTipiDeleteCommandHandler(
    IKurumsalBirimTipiRepository kurumsalBirimTipiRepository,
    IKurumsalBirimRepository kurumsalBirimRepository,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<KurumsalBirimTipiDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(KurumsalBirimTipiDeleteCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant bulunamadı");

        var birimTipi = await kurumsalBirimTipiRepository.WhereWithTracking(p => p.Id == request.Id && p.TenantId == tenantId).FirstOrDefaultAsync();
        if (birimTipi is null)
            return Result<string>.Failure("Birim tipi bulunamamdı");

        var birimler = await kurumsalBirimRepository.WhereWithTracking(p => p.BirimTipiId == birimTipi.Id && p.TenantId == tenantId).ToListAsync();
        var birimIdListesi = birimler.Select(b => b.Id).ToList();

        var personelGorevlendirmeler = await personelGorevlendirmeRepository.WhereWithTracking(p => birimIdListesi.Contains(p.KurumsalBirimId!.Value) && p.TenantId == tenantId && !p.IsDeleted).ToListAsync();

        birimTipi.IsDeleted = true;
        foreach(var birim in birimler)
        {
            birim.IsDeleted = true;
        }

        foreach(var personelGorevlendirme in personelGorevlendirmeler)
        {
            personelGorevlendirme.KurumsalBirimId = null;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Birim tipi başarıyla silindi");
    }
}
