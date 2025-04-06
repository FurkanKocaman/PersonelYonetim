using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Subeler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Subeler;
public sealed record SubeDeleteCommand(
    Guid Id):IRequest<Result<string>>;

internal sealed class SubeDeleteCommandHandler(
    ISubeRepository subeRepository,
    IPersonelAtamaRepository personelAtamaRepository,
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

        List<Guid> departmanIds = departmanlar.Select(p => p.Id).ToList();

        var personelAtamalar = await personelAtamaRepository.WhereWithTracking(p => p.SubeId == request.Id || (p.DepartmanId.HasValue && departmanIds.Contains(p.DepartmanId.Value))).ToListAsync(cancellationToken);
        
        foreach(var personelAtama in personelAtamalar)
        {
            personelAtama.SubeId = null;
            personelAtama.DepartmanId = null;
            personelAtama.PozisyonId = null;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Şube ve departmanlar başarıyla silindi");
    }
}
