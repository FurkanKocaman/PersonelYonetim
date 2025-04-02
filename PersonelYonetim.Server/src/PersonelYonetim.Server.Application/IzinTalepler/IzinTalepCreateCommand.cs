using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.UnitOfWork;
using System.Security.Claims;
using TS.Result;

namespace PersonelYonetim.Server.Application.IzinTalepler;

public sealed record IzinTalepCreateCommand(
    DateTimeOffset BaslangicTarihi,
    DateTimeOffset BitisTarihi,
    Guid IzinTurId,
    string? Aciklama) : IRequest<Result<string>>;

public sealed class IzinTalepCreateCommandValidator : AbstractValidator<IzinTalepCreateCommand>
{
    public IzinTalepCreateCommandValidator()
    {
        RuleFor(x => x.BaslangicTarihi).NotEmpty().WithMessage("Başlangıç tarihi boş olamaz");
        RuleFor(x => x.BitisTarihi).NotEmpty().WithMessage("Bitiş tarihi boş olamaz");
    }
}

internal sealed class IzinTalepCreateCommandHandler(
    IIzinTalepRepository izinTalepRepository,
    ICalismaGunRepository calismaGunRepository,
    IPersonelAtamaRepository personelAtamaRepository,
    IPersonelRepository personelRepository,
    IIzinTurRepository izinTurRepository,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<IzinTalepCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(IzinTalepCreateCommand request, CancellationToken cancellationToken)
    {

        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }

        var personel = personelRepository.GetAll()
            .Where(p => p.UserId == Guid.Parse(userIdString))
            .Select(p => new { p.Id })
            .FirstOrDefault();

        if (personel == null)
        {
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
        }

        var personelAtama = await personelAtamaRepository.FirstOrDefaultAsync(p => p.PersonelId == personel.Id && p.IsActive);
        if (personelAtama is null)
            return Result<string>.Failure("Personel departmanı bulunamadı");

        List<CalismaGun> calismaGunler = calismaGunRepository.Where(g => g.CalismaTakvimId == personelAtama.CalismaTakvimId).ToList();

        IzinTur izinTur = await izinTurRepository.FirstOrDefaultAsync(p => p.Id == request.IzinTurId);
        if (izinTur is null)
            return Result<string>.Failure("Izin tur bulunamadı");

        List<IzinTalep> izinTalepler = izinTalepRepository.Where(p => p.PersonelId == personel.Id && p.IzinTurId == izinTur.Id && p.BitisTarihi.Year == DateTime.Now.Year).ToList();
        decimal toplamIzinGun = 0;

        foreach(var izin in izinTalepler)
        {
            toplamIzinGun += izin.ToplamSure;
        }
        
        decimal toplamGun = 0;

        DateTimeOffset baslangic = request.BaslangicTarihi.ToLocalTime();
        DateTimeOffset bitis = request.BitisTarihi.ToLocalTime();

        DateTimeOffset baslangicGun = baslangic.Date;
        DateTimeOffset bitisGun = bitis.Date;

        while (baslangicGun <= bitisGun)
        {
            var calismaGun = calismaGunler.FirstOrDefault(p => p.Gun == baslangicGun.DayOfWeek);

            if (calismaGun is not null && calismaGun.IsCalismaGunu)
            {

                if (baslangicGun == baslangic.Date && baslangic.TimeOfDay > (calismaGun.CalismaBaslangic ?? TimeSpan.Zero))
                {
                    toplamGun += (decimal)((calismaGun.CalismaBitis - baslangic.TimeOfDay)?.TotalHours ?? 0) / 8;
                }
                else if (baslangicGun == bitis.Date && bitis.TimeOfDay < (calismaGun.CalismaBitis ?? TimeSpan.FromHours(18)))
                {
                    toplamGun += (decimal)((bitis.TimeOfDay - calismaGun.CalismaBaslangic)?.TotalHours ?? 0) / 8;
                }
                else
                {
                    toplamGun += 1;
                }
            }
            baslangicGun = baslangicGun.AddDays(1);
        }

        if ((((DateTimeOffset.Now.Year - personelAtama.PozisyonBaslamaTarihi.Year) == 0 ? 1 : (DateTimeOffset.Now.Year - personelAtama.PozisyonBaslamaTarihi.Year)) * izinTur.LimitGunSayisi) < toplamGun + toplamIzinGun && izinTur.LimitTipi != LimitTipiEnum.Limitsiz && izinTur.EksiBakiyeHakkı != EksiBakiyeHakkıEnum.Limitsiz && !(izinTur.EksiBakiyeHakkı == EksiBakiyeHakkıEnum.Limitli && izinTur.EksiBakiyeHakkı > (toplamGun - toplamIzinGun)-izinTur.LimitGunSayisi) )
            return Result<string>.Failure("İzin hakkı dolmuştur");

        var izinBitisGun = calismaGunler.FirstOrDefault(g => g.Gun == bitis.DayOfWeek);
        DateTimeOffset mesaibaslangic = bitis;
        
        if(izinBitisGun != null && izinBitisGun.IsCalismaGunu)
        {
            if(bitis.TimeOfDay < (izinBitisGun.CalismaBitis ?? TimeSpan.FromHours(18)))
            {
                mesaibaslangic = bitis;
            }
            else
            {
                CalismaGun sonrakiCalismaGun = calismaGunler.FirstOrDefault(g => g.IsCalismaGunu && g.Gun > bitis.DayOfWeek)!;
                if (sonrakiCalismaGun != null)
                {
                    mesaibaslangic = bitis.AddDays((int)(sonrakiCalismaGun.Gun - bitis.DayOfWeek))
                        .Date.Add(sonrakiCalismaGun.CalismaBaslangic ?? TimeSpan.FromHours(9));
                }
            }
        }
        else
        {
            CalismaGun sonrakiCalismaGun = calismaGunler.FirstOrDefault(g => g.IsCalismaGunu && g.Gun > bitis.DayOfWeek)!;
            if (sonrakiCalismaGun != null)
            {
                mesaibaslangic = bitis.AddDays((int)(sonrakiCalismaGun.Gun - bitis.DayOfWeek))
                    .Date.Add(sonrakiCalismaGun.CalismaBaslangic ?? TimeSpan.FromHours(9));
            }
        }


        IzinTalep izinTalep = new()
        {
            IzinTurId = request.IzinTurId,
            BaslangicTarihi = baslangic,
            BitisTarihi = bitis,
            PersonelId = personel.Id,
            ToplamSure = toplamGun,
            MesaiBaslangicTarihi = mesaibaslangic,
            DegerlendirmeDurumu = DegerlendirmeDurumEnum.Beklemede
        };

        izinTalepRepository.Add(izinTalep);
        await unitOfWork.SaveChangesAsync();

        return Result<string>.Succeed("İzin talebi oluşturuldu. Toplam "+toplamGun+" gün");
    }
}

