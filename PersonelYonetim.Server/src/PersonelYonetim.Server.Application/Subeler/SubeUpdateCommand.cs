using Mapster;
using MediatR;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Subeler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Subeler;

public sealed record SubeUpdateCommand(
    Guid Id,
    string Ad,
    string? Aciklama,
    Guid SirketId,
    Adres Adres,
    Iletisim Iletisim) : IRequest<Result<string>>;

internal sealed class SubeUpdateCommandHandler(
    ISubeRepository subeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<SubeUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SubeUpdateCommand request, CancellationToken cancellationToken)
    {
        Sube sube = await subeRepository.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted);

        if (sube == null)
            return Result<string>.Failure("Şube bulunamadı");

        sube.Ad = request.Ad;
        sube.Aciklama = request.Aciklama;
        sube.SirketId = request.SirketId;
        sube.Adres = request.Adres;
        sube.Iletisim = request.Iletisim;

        subeRepository.Update(sube);
        await unitOfWork.SaveChangesAsync();

        return Result<string>.Succeed("Şube güncellendi");
    }
}

