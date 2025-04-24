
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.KurumsalBirimler;
public sealed record KurumsalBirimDeleteCommand(
    Guid Id
    ) : IRequest<Result<string>>;


internal sealed class KurumsalBirimDeleteCommandHandler(
    IKurumsalBirimRepository kurumsalBirimRepository,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService 
    ) : IRequestHandler<KurumsalBirimDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(KurumsalBirimDeleteCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant bulunamamdı");

        var kurumsalBirim = await kurumsalBirimRepository.WhereWithTracking(p => p.Id == request.Id && p.TenantId == tenantId).FirstOrDefaultAsync();

        if (kurumsalBirim is null)
            return Result<string>.Failure("Birim bulunamadı");

        var personelGorevlendirmeler = await personelGorevlendirmeRepository.WhereWithTracking(p => p.KurumsalBirimId == kurumsalBirim.Id).ToListAsync();

        kurumsalBirim.IsDeleted = true;

        foreach(var personelGorevlendirme in personelGorevlendirmeler)
        {
            personelGorevlendirme.KurumsalBirimId = null;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Birim başarıyla silindi.");
    }
}
