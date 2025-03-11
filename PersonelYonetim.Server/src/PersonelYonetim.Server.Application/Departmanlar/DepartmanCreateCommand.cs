using FluentValidation;
using GenericRepository;
using MediatR;
using PersonelYonetim.Server.Domain.Departmanlar;
using TS.Result;

namespace PersonelYonetim.Server.Application.Departmanlar;

public sealed record DepartmanCreateCommand(
    string Ad,
    string? Aciklama) :IRequest<Result<string>> ;

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
    IUnitOfWork unitOfWork) : IRequestHandler<DepartmanCreateCommand, Result<string>>
{

    public async Task<Result<string>> Handle(DepartmanCreateCommand request, CancellationToken cancellationToken)
    {
        var departmanVarMi = await departmanRepository.AnyAsync(p => p.Ad == request.Ad);
        if (departmanVarMi)
            return Result<string>.Failure("Departman zaten mevcut");
        Departman departman = new Departman
        {
            Ad = request.Ad,
            Aciklama = request.Aciklama
        };
        departmanRepository.Add(departman);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Departman başarıyla oluşturuldu";
    }
}
