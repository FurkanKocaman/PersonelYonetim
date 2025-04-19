using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.UnitOfWork;
using PersonelYonetim.Server.Domain.ZamanYonetimler;
using System.Security.Claims;
using TS.Result;

namespace PersonelYonetim.Server.Application.CalismaCizelgeleri;
public sealed record CalismaPeriyotCreateCommand(
    Guid? PersonelId,
    Guid? GunlukCalismaId,
    TimeOnly BaslangicSaati,
    TimeOnly BitisSaati) : IRequest<Result<string>>;


internal sealed class CalismaPeriyotCreateCommandHandler(
    IHttpContextAccessor httpContextAccessor,
    IPersonelRepository personelRepository,
    ICalismaPeriyotRepository calismaPeriyotRepository,
    ICalismaCizelgeRepository calismaCizelgeRepository,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CalismaPeriyotCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CalismaPeriyotCreateCommand request, CancellationToken cancellationToken)
    {
        Guid personelId;

        if (request.PersonelId is null)
        {
            var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
            }

            var personel = await personelRepository.FirstOrDefaultAsync(p => p.UserId == Guid.Parse(userIdString), cancellationToken: cancellationToken);

            if (personel is null)
                throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");

            personelId = personel.Id;
        }
        else
        {
            var personel = await personelRepository.FirstOrDefaultAsync(p => p.Id == request.PersonelId.Value);
            if (personel is null)
                return Result<string>.Failure("Belirtilen personel bulunamadı.");

            personelId = personel.Id;
        }

        var cizelge = calismaCizelgeRepository
          .Where(p => p.PersonelId == personelId && p.IsDeleted == false)
          .Include(c => c.GunlukCalismalar)
              .ThenInclude(g => g.CalismaPeriyotlari)
          .Include(c => c.GunlukCalismalar)
              .ThenInclude(g => g.MolaPeriyotlari)
          .Include(c => c.GunlukCalismalar)
              .ThenInclude(g => g.IzinPeriyotlari)
          .Include(c => c.GunlukCalismalar)
              .ThenInclude(g => g.FazlaMesaiPeriyotlari)
          .FirstOrDefault();

        if (cizelge is null)
            return Result<string>.Failure("Çalışma çizelgesi bulunamadı");

        var personelAtama = personelGorevlendirmeRepository.Where(pa => pa.PersonelId == personelId && pa.IsDeleted == false && pa.IsActive == true).Include(pa => pa.CalismaTakvimi).ThenInclude(c => c!.CalismaGunler).FirstOrDefault();

        var calismaTakvim = personelAtama!.CalismaTakvimi;

        List<GunlukCalisma> GunlukCalismalar = cizelge.GunlukCalismalar.ToList();

        foreach(var gunlukCalisma in GunlukCalismalar)
        {
            var tarih = gunlukCalisma.Tarih;
            var gun = calismaTakvim!.CalismaGunler.FirstOrDefault(g => g.Gun == tarih.DayOfWeek);
            if (gun!= null && gun.IsCalismaGunu)
            {
                CalismaPeriyodu calismaPeriyodu = new()
                {
                    GunlukCalismaId = gunlukCalisma.Id,
                    BaslangicSaati = gun.CalismaBaslangic ?? new TimeOnly(9, 0),
                    BitisSaati = gun.MolaBaslangic ?? new TimeOnly(12, 30),
                };
                calismaPeriyodu.ToplamCalismaSuresi = calismaPeriyodu.BitisSaati - calismaPeriyodu.BaslangicSaati;
                calismaPeriyotRepository.Add(calismaPeriyodu);

                MolaPeriyodu molaPeriyodu = new()
                {
                    GunlukCalismaId = gunlukCalisma.Id,
                    BaslangicSaati = gun.MolaBaslangic ?? new TimeOnly(12, 30),
                    BitisSaati = gun.MolaBitis ?? new TimeOnly(13, 30),
                };
                molaPeriyodu.ToplamMolaSuresi = molaPeriyodu.BitisSaati.ToTimeSpan() - molaPeriyodu.BaslangicSaati.ToTimeSpan();
                gunlukCalisma.MolaPeriyotlari.Add(molaPeriyodu);

                CalismaPeriyodu calismaPeriyodu1 = new()
                {
                    GunlukCalismaId = gunlukCalisma.Id,
                    BaslangicSaati = gun.MolaBitis ?? new TimeOnly(13, 30),
                    BitisSaati = gun.CalismaBitis ?? new TimeOnly(18, 00),
                };
                calismaPeriyodu1.ToplamCalismaSuresi = calismaPeriyodu.BitisSaati.ToTimeSpan() - calismaPeriyodu.BaslangicSaati.ToTimeSpan();
                calismaPeriyotRepository.Add(calismaPeriyodu1);
            }
        }

        //CalismaPeriyodu calismaPeriyodu = request.Adapt<CalismaPeriyodu>();
        //calismaPeriyodu.ToplamCalismaSuresi = request.BitisSaati - request.BaslangicSaati;

        //calismaPeriyotRepository.Add(calismaPeriyodu);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Çalışma periyodu başarıyla eklendi");
    }
}
