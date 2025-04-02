using FluentValidation;
using Mapster;
using MediatR;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Pozisyonlar;

public sealed record PozisyonUpdateCommand(
    Guid Id,
    string Ad,
    string? Aciklama,
    Guid? DepartmanId,
    Guid SirketId) : IRequest<Result<string>>;

public sealed class PozisyonUpdateCommandValidator : AbstractValidator<PozisyonCreateCommand>
{
    public PozisyonUpdateCommandValidator()
    {
        RuleFor(x => x.Ad).NotEmpty().WithMessage("Ad boş olamaz").MaximumLength(100);
        RuleFor(x => x.Aciklama).MaximumLength(250);
    }
}

internal sealed class PozisyonUpdateCommandHandler(
    IPozisyonRepository pozisyonRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<PozisyonUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PozisyonUpdateCommand request, CancellationToken cancellationToken)
    {
        Pozisyon pozisyon = await pozisyonRepository.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted);
        if (pozisyon == null)
            return Result<string>.Failure("Pozisyon bulunamadı");

        pozisyon.Ad = request.Ad;
        pozisyon.Aciklama = request.Aciklama;
        pozisyon.DepartmanId = request.DepartmanId;
        pozisyon.SirketId = request.SirketId;
        pozisyonRepository.Update(pozisyon);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Pozisyon başarıyla güncellendi");
    }
}
