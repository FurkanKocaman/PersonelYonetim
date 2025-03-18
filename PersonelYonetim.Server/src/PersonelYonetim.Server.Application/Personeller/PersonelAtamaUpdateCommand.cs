using GenericRepository;
using MediatR;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using TS.Result;

namespace PersonelYonetim.Server.Application.Personeller;

public sealed record PersonelAtamaUpdateCommand(
    Guid PersonelId,
    Guid SirketId,
    Guid? SubeId,
    Guid? DepartmanId,
    Guid? PozisyonId,
    int YoneticiTipiValue) : IRequest<Result<string>>;

internal sealed class PersonelAtamaUpdateCommandHandler(
    IPersonelAtamaRepository personelAtamaRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<PersonelAtamaUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelAtamaUpdateCommand request, CancellationToken cancellationToken)
    {
        var personelAtama = await personelAtamaRepository.FirstOrDefaultAsync(p => p.PersonelId == request.PersonelId && p.SirketId == request.SirketId);
        if (personelAtama == null)
            return Result<string>.Failure("Mevcut atama bulunamadı");

        personelAtama.SubeId = request.SubeId;
        personelAtama.DepartmanId = request.DepartmanId;
        personelAtama.PozisyonId = request.PozisyonId;
        personelAtama.YoneticiTipi = YoneticiTipiEnum.FromValue(request.YoneticiTipiValue);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Personel atama güncellendi");
    }
}
