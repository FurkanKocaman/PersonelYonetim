using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using PersonelYonetim.Server.Domain.Izinler;
using TS.Result;

namespace PersonelYonetim.Server.Application.IzinTurler;

public sealed record IzinTurCreateCommand(
    string Ad,
    string? Aciklama,
    bool UcretliMi,
    bool HakEdisDonem,
    bool HakEdisBaslangic,
    DateTimeOffset? DevretmelimitTarih,
    bool HesapSekli,
    bool AciklamaZorunlu,
    bool YerineBakacakZorunlu,
    Guid SirketId,
    int? DevretmeGunLimit,
    int? KidemYil,
    int? KidemArtıGun,
    int? EnAzTalep,
    int? EnCokTalep,
    int? EksiBakiyeLimit,
    int? LimitGunSayisi,
    int LimitTipiValue = -1, //LimitTipiEnum
    int EksiBakiyeHakkıValue = -1,//EksiBakiyeHakkıEnum
    int HakEdisValue = -1,//HakedisEnum
    int DevretmeTipiValue = -1 // DevretmeTipiEnum
) : IRequest<Result<string>>;

public sealed class IzinTurCreateCommandValidator : AbstractValidator<IzinTurCreateCommand>
{
    public IzinTurCreateCommandValidator()
    {

    }
}

internal sealed class IzinTurCreateCommandHandler(
    IIzinTurRepository IzinTurRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<IzinTurCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(IzinTurCreateCommand request, CancellationToken cancellationToken)
    {
       IzinTur izinTur = request.Adapt<IzinTur>();
        if(request.LimitTipiValue != -1)
            izinTur.LimitTipi = LimitTipiEnum.FromValue(request.LimitTipiValue);
        if (request.EksiBakiyeHakkıValue != -1)
            izinTur.EksiBakiyeHakkı = EksiBakiyeHakkıEnum.FromValue(request.EksiBakiyeHakkıValue);
        if(request.HakEdisValue != -1)
            izinTur.HakEdis = HakEdisEnum.FromValue(request.HakEdisValue);
        if (request.DevretmeTipiValue != -1)
            izinTur.DevretmeTipi = DevretmeTipiEnum.FromValue(request.DevretmeTipiValue);

        IzinTurRepository.Add(izinTur);
        await unitOfWork.SaveChangesAsync();

        return Result<string>.Succeed("Izin tür oluşturuldu");
    }
}
