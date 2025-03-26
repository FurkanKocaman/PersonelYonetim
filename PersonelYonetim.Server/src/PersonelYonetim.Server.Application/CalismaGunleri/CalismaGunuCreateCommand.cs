using Mapster;
using MediatR;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.CalismaGunleri;

public sealed record CalismaGunuCreateCommand(
    DayOfWeek Gun, 
    DateTimeOffset CalismaBaslangic,
    DateTimeOffset CalismaBitis,
    DateTimeOffset MolaBaslangic,
    DateTimeOffset MolaBitis,
    Guid CalismaTakvimId,
    bool IsCalismaGunu = true) : IRequest<Result<string>>;

internal sealed class CalismaGunuCreateCommandHandler(
    ICalismaGunRepository calismaGunRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CalismaGunuCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CalismaGunuCreateCommand request, CancellationToken cancellationToken)
    {
        using(var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                var calismaGunVarMi = await calismaGunRepository.AnyAsync(p => p.Gun == request.Gun && p.CalismaTakvimId == request.CalismaTakvimId);
                if (calismaGunVarMi)
                    return Result<string>.Failure("Bu takvimde bu gün zaten oluşturulmuş");

                CalismaGun calismaGun = request.Adapt<CalismaGun>();
                calismaGunRepository.Add(calismaGun);

                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(transaction);

                return Result<string>.Succeed("Çalışma gunu oluşturuldu");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure("Hata oluştu: " + ex.Message);
            }
        }

    }
}
