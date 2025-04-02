using MediatR;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Pozisyonlar;

public sealed record PozisyonDeleteCommand(
    Guid Id) : IRequest<Result<string>>;


internal sealed class PozisyonDeleteCommandHandler(
    IPozisyonRepository pozisyonRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<PozisyonDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PozisyonDeleteCommand request, CancellationToken cancellationToken)
    {
        Pozisyon pozisyon = await pozisyonRepository.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted);
        if (pozisyon == null)
            return Result<string>.Failure("Pozisyon bulunamadı");

        pozisyon.IsDeleted = true;
        pozisyonRepository.Update(pozisyon);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Pozisyon başarıyla silindi");
    }
}

