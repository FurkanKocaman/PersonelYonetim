using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Sirketler;
using TS.Result;

namespace PersonelYonetim.Server.Application.Pozisyonlar;

public sealed record PozisyonCreateCommand(
    string Ad,
    string? Aciklama,
    Guid SirketId) : IRequest<Result<string>> ;

public sealed class PozisyonCreateCommandValidator : AbstractValidator<PozisyonCreateCommand>
{
    public PozisyonCreateCommandValidator()
    {
        RuleFor(x => x.Ad).NotEmpty().WithMessage("Ad boş olamaz").MaximumLength(100);
        RuleFor(x => x.Aciklama).MaximumLength(250);
    }
}

internal sealed class PozisyonCreateCommandHandler(
    IPozisyonRepository pozisyonRepository,
    ISirketRepository sirketRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<PozisyonCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PozisyonCreateCommand request, CancellationToken cancellationToken)
    {
        var pozisyonVarMi = await pozisyonRepository.AnyAsync(p => p.Ad == request.Ad);
        if (pozisyonVarMi)
            return Result<string>.Failure("Pozisyon zaten mevcut");

        var sirketVarMi = await sirketRepository.AnyAsync(p => p.Id == request.SirketId);
        if(!sirketVarMi)
        {
            return Result<string>.Failure("Şirket bulunamadı");
        }

        Pozisyon pozisyon = request.Adapt<Pozisyon>();
        pozisyonRepository.Add(pozisyon);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Pozisyon oluşturuldu");
    }
}
