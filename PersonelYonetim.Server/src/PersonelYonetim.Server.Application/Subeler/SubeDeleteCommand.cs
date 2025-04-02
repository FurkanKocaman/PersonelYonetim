using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Subeler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Subeler;
public sealed record SubeDeleteCommand(
    Guid Id):IRequest<Result<string>>;

internal sealed class SubeDeleteCommandHandler(
    ISubeRepository subeRepository,
    IDepartmanRepository departmanRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<SubeDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SubeDeleteCommand request, CancellationToken cancellationToken)
    {
        Sube sube = await subeRepository.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted);
        if (sube == null)
            return Result<string>.Failure("Şube bulunamadı");
        sube.IsDeleted = true;

        var departmanlar = await departmanRepository.WhereWithTracking(p => p.SubeId == sube.Id && !p.IsDeleted).ToListAsync(cancellationToken);
        foreach (var departman in departmanlar)
        {
            departman.IsDeleted = true;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Şube ve departmanlar başarıyla silindi");
    }
}
