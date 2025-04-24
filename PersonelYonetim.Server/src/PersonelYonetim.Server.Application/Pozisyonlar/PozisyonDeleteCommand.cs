using MediatR;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Pozisyonlar;

public sealed record PozisyonDeleteCommand(
    Guid Id) : IRequest<Result<string>>;


internal sealed class PozisyonDeleteCommandHandler(
    IPozisyonRepository pozisyonRepository,
    ICurrentUserService currentUserService,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<PozisyonDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PozisyonDeleteCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return Result<string>.Failure("tenant bulunammadı");
        Pozisyon pozisyon = await pozisyonRepository.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted);
        if (pozisyon == null)
            return Result<string>.Failure("Pozisyon bulunamadı");
        pozisyon.IsDeleted = true;

        var personelAtamalar = personelGorevlendirmeRepository.WhereWithTracking(p => p.PozisyonId == request.Id).ToList();

        foreach (var personelAtama in personelAtamalar)
        {
            personelAtama.PozisyonId = null;
        }

        pozisyonRepository.Update(pozisyon);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Pozisyon başarıyla silindi");
    }
}

