
using MediatR;
using PersonelYonetim.Server.Domain.TakvimEtkinlikler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.TakvimEtkinlikler;
public sealed record TakvimEtkinlikDeleteCommand(
    Guid Id):IRequest<Result<string>>;

internal sealed class TakvimEtkinlikDeleteCommandHandler(
    ITakvimEtkinlikRepository takvimEtkinlikRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<TakvimEtkinlikDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(TakvimEtkinlikDeleteCommand request, CancellationToken cancellationToken)
    {
        TakvimEtkinlik takvimEtkinlik = await takvimEtkinlikRepository.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted);

        if (takvimEtkinlik == null)
            return Result<string>.Failure("Etkinlik bulunamadı");

        takvimEtkinlik.IsDeleted = true;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Etkinlik başarıyla silindi");
    }
}
