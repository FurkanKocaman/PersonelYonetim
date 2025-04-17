using FluentValidation;
using MediatR;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Pozisyonlar;

public sealed record PozisyonUpdateCommand(
    Guid Id,
    string Ad,
    string? Kod,
    string? Aciklama,
    Guid? DepartmanId,
    Guid? tenantId) : IRequest<Result<string>>;

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
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork) : IRequestHandler<PozisyonUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PozisyonUpdateCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if(!tenantId.HasValue)
            return Result<string>.Failure("Tenant bilgisi bulunamadı.");

        Pozisyon pozisyon = await pozisyonRepository.FirstOrDefaultAsync(p => p.Id == request.Id && p.TenantId == tenantId && !p.IsDeleted);
        if (pozisyon == null)
            return Result<string>.Failure("Pozisyon bulunamadı");

        pozisyon.Ad = request.Ad;
        pozisyon.Kod = request.Kod;
        pozisyon.Aciklama = request.Aciklama;
        pozisyonRepository.Update(pozisyon);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Pozisyon başarıyla güncellendi");
    }
}
