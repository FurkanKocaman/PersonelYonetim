using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.UnitOfWork;
using PersonelYonetim.Server.Domain.ZamanYonetimler;
using TS.Result;

namespace PersonelYonetim.Server.Application.Periyotlar;
public sealed record IzinPeriyotCreateCommand(
    Guid PersonelId,
    DateTimeOffset Baslangic,
    DateTimeOffset Bitis,
    string IzinTipi
    ) : IRequest<Result<string>>;

internal sealed class IzinPeriyotCreateCommandHandler(
    IPersonelAtamaRepository personelAtamaRepository,
    IIzinPeriyotRepository izinPeriyotRepository,
    IGunlukCalismaRepository gunlukCalismaRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<IzinPeriyotCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(IzinPeriyotCreateCommand request, CancellationToken cancellationToken)
    {
        var personelAtama = personelAtamaRepository.Where(p => p.PersonelId == request.PersonelId && p.IsActive & !p.IsDeleted).Include(p => p.CalismaTakvimi).ThenInclude(p => p!.CalismaGunler).FirstOrDefault();

        if (personelAtama is null)
            return Result<string>.Failure("Personel ataması bulunamadı.");

        Guid GunlukCalismaId = Guid.Empty;

        var gun = request.Baslangic.Date;

        var gunlukCalismalar = gunlukCalismaRepository.Where(g => g.Tarih <= request.Bitis.Date && g.Tarih >= request.Baslangic.Date && g.PersonelId == personelAtama.PersonelId).ToList();

        List<IzinPeriyodu> izinPeriyotlari = new();

        while (gun <= request.Bitis.Date)
        {
            TimeOnly basSaat;
            TimeOnly bitSaat;

            if (gun == request.Baslangic.Date && gun == request.Bitis.Date)
            {
                basSaat = TimeOnly.FromDateTime(request.Baslangic.DateTime);
                bitSaat = TimeOnly.FromDateTime(request.Bitis.DateTime);
            }
            else if (gun == request.Baslangic.Date)
            {
                basSaat = TimeOnly.FromDateTime(request.Baslangic.DateTime);
                bitSaat = new TimeOnly(23, 59);
            }
            else if (gun == request.Bitis.Date)
            {
                basSaat = new TimeOnly(0, 0);
                bitSaat = TimeOnly.FromDateTime(request.Bitis.DateTime);
            }
            else
            {
                basSaat = new TimeOnly(0, 0);
                bitSaat = new TimeOnly(23, 59);
            }
            IzinPeriyodu izinPeriyodu = new()
            {
                GunlukCalismaId = gunlukCalismalar.FirstOrDefault(p => p.Tarih == gun)!.Id,
                BaslangicSaati = basSaat,
                BitisSaati = bitSaat,
                IzinTipi = request.IzinTipi
            };
            izinPeriyotlari.Add(izinPeriyodu);
            gun = gun.AddDays(1);
        }

        izinPeriyotRepository.AddRange(izinPeriyotlari);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Izin periyodu oluşturuldu");
    }
}
