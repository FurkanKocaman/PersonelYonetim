using FluentValidation;
using Mapster;
using MediatR;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.IzinKurallar;

public sealed record IzinKuralCreateCommand(
    string Ad,
    string? Aciklama,
    Guid SirketId,
    List<Guid> IzinTurler) : IRequest<Result<string>>;

public sealed class IzinKuralCreateCommandValidator : AbstractValidator<IzinKuralCreateCommand>
{
    public IzinKuralCreateCommandValidator()
    {
        RuleFor(x => x.Ad).NotNull().NotEmpty().WithMessage("Izin kural ad kısmı boş olamaz");
    }
}

internal sealed class IzinKuralCreateCommandHandler(
    IIzinKuralRepository izinKuralRepository,
    IIzinTurIzinKuralRepository izinturIzinKuralRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<IzinKuralCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(IzinKuralCreateCommand request, CancellationToken cancellationToken)
    {
        using(var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                var isKuralExist = await izinKuralRepository.AnyAsync(p => p.Ad == request.Ad && p.SirketId == request.SirketId);
                if (isKuralExist)
                    return Result<string>.Failure("Bu isimde bir kural bu şirkette zaten var");
                IzinKural izinKural = request.Adapt<IzinKural>();
                izinKuralRepository.Add(izinKural);
                var affectedRows = await unitOfWork.SaveChangesAsync(cancellationToken);
                if (affectedRows == 0)
                {
                    return Result<string>.Failure("Hiçbir değişiklik yapılmadı");
                }

                if (request.IzinTurler.Any())
                {
                    foreach (var izinTur in request.IzinTurler)
                    {
                        IzinTurIzinKural izinTurkural = new()
                        {
                            IzinTurId = izinTur,
                            IzinKuralId = izinKural.Id,
                        };
                        izinturIzinKuralRepository.Add(izinTurkural);
                    }
                    var affectedRows_ = await unitOfWork.SaveChangesAsync(cancellationToken);
                    if (affectedRows_ == 0)
                    {
                        return Result<string>.Failure("Hiçbir değişiklik yapılmadı");
                    }

                }

                await unitOfWork.CommitTransactionAsync(transaction);

                return Result<string>.Succeed("Izin kural oluşturuldu");
            }catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure("Bir hata oluştu: " + ex.Message);
            }
        }
    }
}
