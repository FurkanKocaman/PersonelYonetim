using GenericRepository;
using Mapster;
using MediatR;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Sirketler;
using TS.Result;

namespace PersonelYonetim.Server.Application.Sirketler;

public sealed record SirketCreateCommand(
    string Ad,
    string? Aciklama,
    string? LogoUrl,
    Adres Adres,
    Iletisim Iletisim) : IRequest<Result<string>>;

internal sealed class SirketCreateCommandHandler(
    ISirketRepository sirketRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<SirketCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SirketCreateCommand request, CancellationToken cancellationToken)
    {
        var sirketVarMi = await sirketRepository.AnyAsync(p => p.Ad == request.Ad);
        if (sirketVarMi)
            return Result<string>.Failure("Bu isme sahip şirket zaten mevcut");

        Sirket sirket = request.Adapt<Sirket>();
        sirketRepository.Add(sirket);
        await unitOfWork.SaveChangesAsync();

        return Result<string>.Succeed("Şirket oluşturuldu");
    }
}
