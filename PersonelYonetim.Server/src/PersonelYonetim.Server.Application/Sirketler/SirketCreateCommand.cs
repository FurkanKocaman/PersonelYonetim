using Mapster;
using MediatR;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Rols;
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
    IIzinTurRepository izinTurRepository,
    ICalismaTakvimRepository calismaTakvimRepository,
    ICalismaGunRepository calismaGunRepository,
    IOnaySurecRepository onaySurecRepository,
    IOnaySurecAdimRepository onaySurecAdimRepository,
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

        IzinKural defaultIzinKural = new()
        {
            Ad = "Default İzin Kural",
            Aciklama = "Şirket oluşturulduğunda otomatik oluşan default izin kuralı",
            SirketId = sirket.Id,
        };

        izinKuralRepository.Add(defaultIzinKural);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var defaultIzinTurler = DefaultIzinTurler.GetDefaultIzinTurler(sirket.Id, defaultIzinKural.Id);
        izinTurRepository.AddRange(defaultIzinTurler);

        //await unitOfWork.SaveChangesAsync(cancellationToken);

        var defaultOnaySurecler = DefaultOnaySurec.GetDefaultOnaySurecleri(sirket.Id);

        onaySurecRepository.AddRange(defaultOnaySurecler);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        List<OnaySureciAdimi> defaultOnayAdimlari = new();

        foreach(var onaySurec in defaultOnaySurecler)
        {
            defaultOnayAdimlari.Add(DefaultOnaySurec.GetDefaultOnayAdim(onaySurec.Id, RolTipiEnum.DepartmanYonetici));
        }

        onaySurecAdimRepository.AddRange(defaultOnayAdimlari);
        //await unitOfWork.SaveChangesAsync(cancellationToken);

        CalismaTakvimi tamCalismaTakvim = DefaultCalismaTakvim.GetDefaultTamCalismaTakvim(sirket.Id);
        CalismaTakvimi yariCalismaTakvim = DefaultCalismaTakvim.GetDefaultYarimCalismaTakvim(sirket.Id);

        List<CalismaGun> tamCalismaGunler = DefaultCalismaTakvim.GetDefaultTamCalismaGunler(tamCalismaTakvim.Id);
        List<CalismaGun> yariCalismaGunler = DefaultCalismaTakvim.GetDefaultYariCalismaGunler(yariCalismaTakvim.Id);

        calismaTakvimRepository.AddRange([tamCalismaTakvim, yariCalismaTakvim]);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        calismaGunRepository.AddRange(tamCalismaGunler);
        calismaGunRepository.AddRange(yariCalismaGunler);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed(sirket.Id.ToString());
    }
}
