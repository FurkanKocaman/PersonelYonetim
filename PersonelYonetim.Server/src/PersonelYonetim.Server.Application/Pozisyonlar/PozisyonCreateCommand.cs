using FluentValidation;
using Mapster;
using MediatR;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Pozisyonlar;

public sealed record PozisyonCreateCommand(
    string Ad,
    string? Kod,
    string? Aciklama,
    Guid? tenantId) : IRequest<Result<string>> ;

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
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork) : IRequestHandler<PozisyonCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PozisyonCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if(!tenantId.HasValue)
            return Result<string>.Failure("Tenant bilgisi bulunamadı.");

        var pozisyonVarMi = await pozisyonRepository.AnyAsync(p => p.Ad == request.Ad && !p.IsDeleted && p.TenantId == tenantId);
        if (pozisyonVarMi)
            return Result<string>.Failure("Pozisyon zaten mevcut");

        Pozisyon pozisyon = request.Adapt<Pozisyon>();
        pozisyonRepository.Add(pozisyon);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Pozisyon oluşturuldu");
    }
}
