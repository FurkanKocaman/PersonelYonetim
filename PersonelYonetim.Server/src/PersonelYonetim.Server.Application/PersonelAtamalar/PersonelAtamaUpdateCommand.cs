using GenericRepository;
using MediatR;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Rols;
using TS.Result;

namespace PersonelYonetim.Server.Application.PersonelAtamalar;

public sealed record PersonelAtamaUpdateCommand(
    Guid PersonelId,
    Guid SirketId,
    Guid? SubeId,
    Guid? DepartmanId,
    Guid? PozisyonId,
    int RolTipiValue) : IRequest<Result<string>>;

internal sealed class PersonelAtamaUpdateCommandHandler(
    IPersonelAtamaRepository personelAtamaRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<PersonelAtamaUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelAtamaUpdateCommand request, CancellationToken cancellationToken)
    {
        var personelAtama = await personelAtamaRepository.FirstOrDefaultAsync(p => p.PersonelId == request.PersonelId && p.SirketId == request.SirketId && p.IsActive);
        if (personelAtama == null)
            return Result<string>.Failure("Mevcut atama bulunamadı");

        if(request.SubeId is not null)
            personelAtama.SubeId = request.SubeId;

        if(request.DepartmanId is not null)
            personelAtama.DepartmanId = request.DepartmanId;

        if(request.PozisyonId is not null)
            personelAtama.PozisyonId = request.PozisyonId;

        personelAtama.RolTipi = RolTipiEnum.FromValue(request.RolTipiValue);

        personelAtamaRepository.Update(personelAtama);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Personel atama güncellendi");
    }
}
