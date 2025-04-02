using MediatR;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Sirketler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Sirketler;

public sealed record SirketUpdateCommand(
    Guid Id,
    string Ad,
    string? Aciklama,
    string? LogoUrl,
    Adres Adres,
    Iletisim Iletisim) : IRequest<Result<string>>;

internal sealed class SirketUpdateCommandHandler(
    ISirketRepository sirketRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<SirketUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SirketUpdateCommand request, CancellationToken cancellationToken)
    {
        Sirket sirket = await sirketRepository.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted);
        if (sirket == null)
            return Result<string>.Failure("Şirket bulunamadı");

        sirket.Ad = request.Ad;
        sirket.Aciklama = request.Aciklama;
        sirket.LogoUrl = request.LogoUrl;
        sirket.Adres = request.Adres;
        sirket.Iletisim = request.Iletisim;

        sirketRepository.Update(sirket);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Şirket güncellendi");
    }
}