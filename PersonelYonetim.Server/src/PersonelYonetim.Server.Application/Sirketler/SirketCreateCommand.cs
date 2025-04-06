using Mapster;
using MediatR;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Sirketler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Sirketler;

public sealed record SirketCreateCommand(
    string Ad,
    string? Aciklama,
    string? LogoUrl,
    Adres Adres,
    Iletisim Iletisim) : IRequest<Result<string>>;

internal sealed class SirketCreateCommandHandler(
    ISirketRepository sirketRepository,
    IIzinKuralRepository izinKuralRepository,
    IIzinTurIzinKuralRepository izinturIzinKuralRepository,
    IIzinTurRepository izinTurRepository,
    ICalismaTakvimRepository calismaTakvimRepository,
    ICalismaGunRepository calismaGunRepository,
    //IPersonelRepository personelRepository,
    //IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork) : IRequestHandler<SirketCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SirketCreateCommand request, CancellationToken cancellationToken)
    {
        var sirketVarMi = await sirketRepository.AnyAsync(p => p.Ad == request.Ad && !p.IsDeleted);
        if (sirketVarMi)
            return Result<string>.Failure("Bu isme sahip şirket zaten mevcut.");

        Sirket sirket = request.Adapt<Sirket>();
        sirketRepository.Add(sirket);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var defaultIzinTurler = DefaultIzinTurler.GetDefaultIzinTurler(sirket.Id);
        foreach(var izinTur in defaultIzinTurler)
        {
            izinTurRepository.Add(izinTur);
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);

        IzinKural defaultIzinKural = new()
        {
            Ad = "Default İzin Kural",
            Aciklama = "Şirket oluşturulduğunda otomatik oluşan default izin kuralı",
            SirketId = sirket.Id,
        };
        izinKuralRepository.Add(defaultIzinKural);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        foreach (var izinTurId in defaultIzinTurler.Select(p => p.Id))
        {
            IzinTurIzinKural izinTurkural = new()
            {
                IzinTurId = izinTurId,
                IzinKuralId = defaultIzinKural.Id,
            };
            izinturIzinKuralRepository.Add(izinTurkural);
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);

        CalismaTakvimi defaultCalismaTakvim = DefaultCalismaTakvim.GetDefaultCalismaTakvim(sirket.Id);
        List<CalismaGun> defaultCalismaGunler = DefaultCalismaTakvim.GetDefaultCalismaGunler(defaultCalismaTakvim.Id);

        calismaTakvimRepository.Add(defaultCalismaTakvim);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        foreach(CalismaGun gun in defaultCalismaGunler)
        {
            calismaGunRepository.Add(gun);
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed(sirket.Id.ToString());
    }
}
