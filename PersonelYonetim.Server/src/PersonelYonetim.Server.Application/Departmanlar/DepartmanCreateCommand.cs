using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.Subeler;
using TS.Result;

namespace PersonelYonetim.Server.Application.Departmanlar;

public sealed record DepartmanCreateCommand(
    string Ad,
    string? Aciklama,
    Guid SubeId) :IRequest<Result<string>> ;

public sealed class DepartmanCreateCommandValidator : AbstractValidator<DepartmanCreateCommand>
{
    public DepartmanCreateCommandValidator()
    {
        RuleFor(x => x.Ad).NotEmpty().WithMessage("Ad boş olamaz").MaximumLength(100);
        RuleFor(x => x.Aciklama).MaximumLength(250);
    }
}

internal sealed class DepartmanCreateCommandHandler(
    IDepartmanRepository departmanRepository,
    ISubeRepository subeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DepartmanCreateCommand, Result<string>>
{

    public async Task<Result<string>> Handle(DepartmanCreateCommand request, CancellationToken cancellationToken)
    {
        var departmanVarMi = await departmanRepository.AnyAsync(p => p.Ad == request.Ad);
        if (departmanVarMi)
            return Result<string>.Failure("Bu isme sahip departman zaten mevcut");

        var subeVarMi = await subeRepository.AnyAsync(p => p.Id == request.SubeId);
        if (!subeVarMi)
            return Result<string>.Failure("Şube bulunamadı");

        Departman departman = request.Adapt<Departman>();
        departmanRepository.Add(departman);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Departman başarıyla oluşturuldu");
    }
}
