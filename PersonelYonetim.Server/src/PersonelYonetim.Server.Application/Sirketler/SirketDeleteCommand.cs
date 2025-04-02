using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Sirketler;
using PersonelYonetim.Server.Domain.Subeler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Sirketler;
public sealed record SirketDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class SirketDeleteCommandHandler(
    ISirketRepository sirketRepository,
    ISubeRepository subeRepository,
    IDepartmanRepository departmanRepository,
    IPozisyonRepository pozisyonRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<SirketDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SirketDeleteCommand request, CancellationToken cancellationToken)
    {
        Sirket? sirket = await sirketRepository.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted);
        if (sirket == null)
            return Result<string>.Failure("Şirket bulunamadı");

        sirket.IsDeleted = true;

        var subeler = await subeRepository.WhereWithTracking(p => p.SirketId == request.Id && !p.IsDeleted).ToListAsync(cancellationToken);
        foreach (var sube in subeler)
        {
            sube.IsDeleted = true;
        }

        var subeIds = subeler.Select(s => s.Id).ToList();
        var departmanlar = await departmanRepository.WhereWithTracking(p => subeIds.Contains(p.SubeId) && !p.IsDeleted).ToListAsync(cancellationToken);
        foreach (var departman in departmanlar)
        {
            departman.IsDeleted = true;
        }

        var pozisyonlar = await pozisyonRepository.WhereWithTracking(p => p.SirketId == request.Id && !p.IsDeleted).ToListAsync(cancellationToken);
        foreach (var pozisyon in pozisyonlar)
        {
            pozisyon.IsDeleted = true;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Şirket ve bağlantıları başarıyla silindi");
    }
}
