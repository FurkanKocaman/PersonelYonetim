using FluentValidation;
using Mapster;
using MediatR;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Departmanlar;
public sealed record DepartmanUpdateCommand(
    Guid Id,
    string Ad,
    string? Aciklama,
    Guid SubeId) : IRequest<Result<string>>;

public sealed class DepartmanUpdateCommandValidator : AbstractValidator<DepartmanCreateCommand>
{
    public DepartmanUpdateCommandValidator()
    {
        RuleFor(x => x.Ad).NotEmpty().WithMessage("Ad boş olamaz").MaximumLength(100);
        RuleFor(x => x.Aciklama).MaximumLength(250);
    }
}

internal sealed class DepartmanUpdateCommandHandler(
    IDepartmanRepository departmanRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DepartmanUpdateCommand, Result<string>>
{

    public async Task<Result<string>> Handle(DepartmanUpdateCommand request, CancellationToken cancellationToken)
    {
        using (var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                Departman departman = await departmanRepository.FirstOrDefaultAsync(p => p.Id == request.Id);
                if (departman == null)
                    return Result<string>.Failure("Departman bulunamadı");

                departman.Ad = request.Ad;
                departman.Aciklama = request.Aciklama;
                departman.SubeId = request.SubeId;

                departmanRepository.Update(departman);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(transaction);

                return Result<string>.Succeed("Departman başarıyla güncellendi");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure("Bir hata oluştu: " + ex.Message);
            }
        }

    }
}
