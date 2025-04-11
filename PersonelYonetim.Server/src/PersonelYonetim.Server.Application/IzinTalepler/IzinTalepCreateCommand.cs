using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.OnaySurecleri;
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
    IHttpContextAccessor httpContextAccessor,
    IPersonelIzinKuralRepository personelIzinKuralRepository,
    ITalepDegerlendirmeRepository talepDegerlendirmeRepository,
    IOnaySurecAdimRepository onaySurecAdimRepository,
    IBildirimService bildirimService
    ) : IRequestHandler<IzinTalepCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(IzinTalepCreateCommand request, CancellationToken cancellationToken)
    {
        using (var transaction = unitOfWork.BeginTransaction())
        {
            try
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

                var personelAtama = personelAtamaRepository.Where(p => p.PersonelId == personel.Id && p.IsActive && p.IsDeleted == false).Include(p => p.Personel).FirstOrDefault();
                if (personelAtama is null)
                    return Result<string>.Failure("Personel departmanı bulunamadı");

                List<CalismaGun> calismaGunler = calismaGunRepository.Where(g => g.CalismaTakvimId == personelAtama.CalismaTakvimId).ToList();

                IzinTur izinTur = await izinTurRepository.FirstOrDefaultAsync(p => p.Id == request.IzinTurId);
                if (izinTur is null)
                    return Result<string>.Failure("Izin tur bulunamadı");

                List<IzinTalep> izinTalepler = izinTalepRepository.Where(p => p.PersonelId == personel.Id && p.IzinTurId == izinTur.Id && p.BitisTarihi.Year == DateTime.Now.Year).ToList();
                decimal toplamIzinGun = 0;

                foreach (var izin in izinTalepler)
                {
                    if (izin.DegerlendirmeDurumu != DegerlendirmeDurumEnum.Reddedildi)
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
                            if (baslangic.TimeOfDay <= (calismaGun.CalismaBitis ?? TimeSpan.FromHours(18)))
                            {
                                toplamGun += (decimal)((calismaGun.CalismaBitis - baslangic.TimeOfDay)?.TotalHours ?? 0) / 9;
                            }
                        }
                        else if (baslangicGun == bitis.Date && bitis.TimeOfDay < (calismaGun.CalismaBitis ?? TimeSpan.FromHours(18)))
                        {
                            toplamGun += (decimal)((bitis.TimeOfDay - calismaGun.CalismaBaslangic)?.TotalHours ?? 0) / 9;
                        }
                        else
                        {
                            toplamGun += 1;
                        }
                    }
                    baslangicGun = baslangicGun.AddDays(1);
                }

                if ((((DateTimeOffset.Now.Year - personelAtama.PozisyonBaslamaTarihi.Year) == 0 ? 1 : (DateTimeOffset.Now.Year - personelAtama.PozisyonBaslamaTarihi.Year)) * izinTur.LimitGunSayisi) < toplamGun + toplamIzinGun && izinTur.LimitTipi != LimitTipiEnum.Limitsiz && izinTur.EksiBakiyeHakkı != EksiBakiyeHakkıEnum.Limitsiz && !(izinTur.EksiBakiyeHakkı == EksiBakiyeHakkıEnum.Limitli && izinTur.EksiBakiyeHakkı > (toplamGun - toplamIzinGun) - izinTur.LimitGunSayisi))
                    return Result<string>.Failure("İzin hakkı dolmuştur");

                var izinBitisGun = calismaGunler.FirstOrDefault(g => g.Gun == bitis.DayOfWeek);
                DateTimeOffset mesaibaslangic = bitis;

                if (izinBitisGun != null && izinBitisGun.IsCalismaGunu)
                {
                    if (bitis.TimeOfDay < (izinBitisGun.CalismaBitis ?? TimeSpan.FromHours(18)))
                    {
                        mesaibaslangic = bitis;
                    }
                    else
                    {
                        CalismaGun sonrakiCalismaGun = calismaGunler.Where(g => g.IsCalismaGunu).OrderBy(p => p.Gun).FirstOrDefault()!;
                        if (sonrakiCalismaGun != null)
                        {
                            while (mesaibaslangic.DayOfWeek != sonrakiCalismaGun.Gun)
                            {
                                mesaibaslangic = mesaibaslangic.Date.AddDays(1);
                            }
                            mesaibaslangic = mesaibaslangic.DateTime.Add(sonrakiCalismaGun.CalismaBaslangic ?? TimeSpan.FromHours(9));
                        }
                    }
                }
                else
                {
                    CalismaGun sonrakiCalismaGun = calismaGunler.Where(g => g.IsCalismaGunu).OrderBy(p => p.Gun).FirstOrDefault()!;
                    if (sonrakiCalismaGun != null)
                    {
                        while (mesaibaslangic.DayOfWeek != sonrakiCalismaGun.Gun)
                        {
                            mesaibaslangic = mesaibaslangic.Date.AddDays(1);
                        }
                        mesaibaslangic = mesaibaslangic.DateTime.Add(sonrakiCalismaGun.CalismaBaslangic ?? TimeSpan.FromHours(9));
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
                    DegerlendirmeDurumu = DegerlendirmeDurumEnum.Beklemede,
                    SirketId = personelAtama.SirketId
                };
                izinTalepRepository.Add(izinTalep);

                var personelIzinKural = await personelIzinKuralRepository.GetAll().Include(p => p.OnaySurec).FirstOrDefaultAsync(p => p.PersonelId == personel.Id && p.IzinKuralId == izinTur.IzinKuralId && p.IsDeleted == false && p.IsActive);
                if (personelIzinKural is null)
                    return Result<string>.Failure("Personel izin kuralı bulunamadı");

                var onaySurec = personelIzinKural.OnaySurec;
                if (onaySurec is null)
                    return Result<string>.Failure("Onay süreci bulunamadı");

                List<TalepDegerlendirme> talepDegerlendirmeler = new();
                var onayAdimlari = onaySurecAdimRepository.Where(p => p.OnaySurecId == onaySurec.Id).ToList();
                if (onayAdimlari.Any())
                {
                    foreach (var onayAdimi in onayAdimlari)
                    {
                        TalepDegerlendirme talep = new()
                        {
                            TalepId = izinTalep.Id,
                            TalepTuru = OnaySurecTuruEnum.Izin,
                            AdimSirasi = onayAdimi.Sira,
                            OnaySureciAdimiId = onayAdimi.Id,
                            DegerlendirenId = null,
                            Degerlendiren = null,
                        };
                        talepDegerlendirmeler.Add(talep);
                    }
                }
                talepDegerlendirmeRepository.AddRange(talepDegerlendirmeler);
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                Bildirim bildirim = new()
                {
                    Baslik = "Yeni İzin Talebi",
                    Aciklama = $"{personelAtama.Personel!.Ad} {personelAtama.Personel.Soyad} tarafından yeni bir izin talebi oluşturuldu.",
                    CreatedAt = DateTimeOffset.Now,
                    BildirimTipi = BildirimTipiEnum.Onay,
                    AliciTipi = AliciTipiEnum.Personel,
                    AliciId = personelAtama.YoneticiId ?? personel.Id,
                };

                await bildirimService.KullaniciyaBildirimGonderAsync(bildirim, personelAtama.YoneticiId ?? personel.Id);


                await unitOfWork.SaveChangesAsync();
                await unitOfWork.CommitTransactionAsync(transaction);
                return Result<string>.Succeed("İzin talebi oluşturuldu. Toplam " + toplamGun + " gün");

            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure("İzin talebi oluşturulurken hata oluştu: " + ex.Message);
            }

        }
    }
}

