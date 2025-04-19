using FluentValidation;
using Mapster;
using MediatR;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.TakvimEtkinlikler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.TakvimEtkinlikler;
public sealed record TakvimEtkinlikCreateCommand(
    string Baslik,
    string? Aciklama,
    DateTimeOffset BaslangicTarihi,
    DateTimeOffset? BitisTarihi,
    bool IsPublic,
    Guid? tenantId,
    IEnumerable<Guid>? PersonelIdler) : IRequest<Result<string>>;

public sealed class TakvimEtkinlikCreateCommandValidator : AbstractValidator<TakvimEtkinlikCreateCommand>
{
    public TakvimEtkinlikCreateCommandValidator()
    {
        RuleFor(x => x.Baslik).NotEmpty().WithMessage("Baslik boş olamaz");
        RuleFor(x => x.BaslangicTarihi).NotEmpty().WithMessage("Başlangıç tarihi boş olamaz");
    }
}

internal sealed class TakvimEtkinlikCreateCommandHandler(
    ITakvimEtkinlikRepository takvimEtkinlikRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService
    ) : IRequestHandler<TakvimEtkinlikCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(TakvimEtkinlikCreateCommand request, CancellationToken cancellationToken)
    {
        TakvimEtkinlik takvimEtkinlik = request.Adapt<TakvimEtkinlik>();
        if(request.tenantId == null)
        {
            Guid? tenantId = currentUserService.TenantId;

            if(!tenantId.HasValue)
                return Result<string>.Failure("TenantId bulunamadı");

            takvimEtkinlik.TenantId = tenantId.Value;
        }
       
        takvimEtkinlikRepository.Add(takvimEtkinlik);
        await unitOfWork.SaveChangesAsync();

        return Result<string>.Succeed(takvimEtkinlik.Id.ToString());
    }
}
