using GenericRepository;
using MediatR;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using TS.Result;

namespace PersonelYonetim.Server.Application.Personeller;

public sealed record PersonelAtamaCreateCommand(
    Guid PersonelId,
    Guid SirketId,
    Guid? SubeId,
    Guid? DepartmanId,
    Guid? PozisyonId,
    int YoneticiTipiValue) : IRequest<Result<string>>;

internal sealed class PersonelAtamaCreateCommandHandler(
    IPersonelAtamaRepository personelAtamaRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<PersonelAtamaCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelAtamaCreateCommand request, CancellationToken cancellationToken)
    {
        PersonelAtama personelAtama = new()
        {
            PersonelId = request.PersonelId,
            SirketId = request.SirketId,
            SubeId = request.SubeId,
            DepartmanId = request.DepartmanId,
            PozisyonId = request.PozisyonId,
            YoneticiTipi = request.YoneticiTipiValue == -1 ? null : YoneticiTipiEnum.FromValue(request.YoneticiTipiValue)
        };
        personelAtamaRepository.Add(personelAtama);
        await unitOfWork.SaveChangesAsync();
        return Result<string>.Succeed("Personel atama oluşturuldu");
    }
}
