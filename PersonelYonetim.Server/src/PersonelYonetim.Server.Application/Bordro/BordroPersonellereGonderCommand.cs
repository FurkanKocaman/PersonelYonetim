using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bordro;
using TS.Result;

namespace PersonelYonetim.Server.Application.Bordro;
public sealed record BordroPersonellereGonderCommand(
    int Yil,
    int Ay,
    List<Guid> PersonelIdler
    ) : IRequest<Result<string>>;

internal sealed class BordroPersonellereGonderCommandHandler(
    IBordroDonemRepository bordroDonemRepository,
    ICurrentUserService currentUserService
    ) : IRequestHandler<BordroPersonellereGonderCommand, Result<string>>
{
    public async Task<Result<string>> Handle(BordroPersonellereGonderCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant bulunammadı");

        var bordroDonem = await bordroDonemRepository.Where(p => p.TenantId == tenantId && !p.IsDeleted).Include(p => p.MaasPusulalar).FirstOrDefaultAsync();
        if (bordroDonem is null)
            return Result<string>.Failure("Bordro bulunamamdı");

        var maasPusulalar = bordroDonem.MaasPusulalar.Where(p => request.PersonelIdler.Contains(p.Id) && !p.IsDeleted).ToList();

        return Result<string>.Succeed("Personellere bordro PDF'leri mail adresleri üzerinden gönderildi.");
    }
}
