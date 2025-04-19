
//using MediatR;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.EntityFrameworkCore;
//using PersonelYonetim.Server.Application.Periyotlar;
//using PersonelYonetim.Server.Domain.Bildirimler;
//using PersonelYonetim.Server.Domain.Izinler;
//using PersonelYonetim.Server.Domain.OnaySurecleri;
//using PersonelYonetim.Server.Domain.PersonelAtamalar;
//using PersonelYonetim.Server.Domain.Personeller;
//using PersonelYonetim.Server.Domain.Rols;
//using PersonelYonetim.Server.Domain.UnitOfWork;
//using System.Security.Claims;
//using TS.Result;

//namespace PersonelYonetim.Server.Application.IzinTalepler;

//public sealed record IzinTalepOnayCommand(
//    Guid Id,
//    int OnayDurum) : IRequest<Result<string>> ;

//internal sealed class IzinTalepOnayCommandHandler(
//    IIzinTalepRepository izinTalepRepository,
//    IHttpContextAccessor httpContextAccessor,
//    IPersonelRepository personelRepository,
//    IPersonelAtamaRepository personelAtamaRepository,
//    //IBildirimService bildirimService,
//    ITalepDegerlendirmeRepository talepDegerlendirmeRepository,
//    IUnitOfWork unitOfWork,
//    ISender sender) : IRequestHandler<IzinTalepOnayCommand, Result<string>>
//{
//    public async Task<Result<string>> Handle(IzinTalepOnayCommand request, CancellationToken cancellationToken)
//    {
//        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//        if (string.IsNullOrEmpty(userIdString))
//        {
//            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
//        }

//        var personel = personelRepository.GetAll()
//            .Where(p => p.UserId == Guid.Parse(userIdString))
//            .Select(p => new { p.Id, p.FullName })
//            .FirstOrDefault();

//        if (personel is null)
//            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");

//        IzinTalep? izinTalep = await izinTalepRepository.Where(p => p.Id == request.Id).Include(p => p.IzinTur).Include(p => p.Personel).ThenInclude(p => p.PersonelAtamalar)
//            .Include(p => p.DegerlendirmeAdimlari).ThenInclude(p => p.OnaySureciAdimi).FirstOrDefaultAsync(cancellationToken);

//        if (izinTalep is null)
//            return Result<string>.Failure("Izin talebi bulunamadı");

//        PersonelAtama personelAtama = await personelAtamaRepository.FirstOrDefaultAsync(p => p.PersonelId == personel.Id && p.SirketId == izinTalep.SirketId && p.IsDeleted == false);
//        if(personelAtama is null)
//            return Result<string>.Failure("Personel ataması bulunamadı");

//        var adimlar = izinTalep.DegerlendirmeAdimlari;

//        var izinTalepPersonelAtama = izinTalep.Personel.PersonelAtamalar.Where(pa => pa.IsDeleted == false && pa.SirketId == izinTalep.SirketId).FirstOrDefault();
//        if(izinTalepPersonelAtama is null)
//            return Result<string>.Failure("İzin talep eden personel ataması bulunamadı");

//        foreach (var adim in adimlar)
//        {
//            if(adim.OnaySureciAdimi != null)
//            {
//                if (!adimlar.Any(a => a.AdimSirasi < adim.AdimSirasi && a.DegerlendirmeDurumu == DegerlendirmeDurumEnum.Beklemede))
//                {
//                    if (adim.OnaySureciAdimi.PersonelId != null && adim.OnaySureciAdimi.PersonelId == personel.Id && !adimlar.Any(a => a.AdimSirasi < adim.AdimSirasi && a.DegerlendirmeDurumu == DegerlendirmeDurumEnum.Beklemede))
//                    {
//                        adim.DegerlendirmeDurumu = DegerlendirmeDurumEnum.FromValue(request.OnayDurum);
//                        adim.DegerlendirilmeTarihi = DateTimeOffset.Now;
//                        adim.DegerlendirenId = personel.Id;
//                        talepDegerlendirmeRepository.Update(adim);

//                        if(!adimlar.Any(p => p.AdimSirasi > adim.AdimSirasi) || DegerlendirmeDurumEnum.FromValue(request.OnayDurum) == DegerlendirmeDurumEnum.Reddedildi)
//                        {
//                            izinTalep.DegerlendirmeDurumu = DegerlendirmeDurumEnum.FromValue(request.OnayDurum);
//                            izinTalepRepository.Update(izinTalep);

//                            if(request.OnayDurum == DegerlendirmeDurumEnum.Onaylandi)
//                            {
//                                IzinPeriyotCreateCommand izinPeriyotCreateCommand = new(izinTalep.PersonelId, izinTalep.BaslangicTarihi, izinTalep.BitisTarihi, izinTalep.IzinTur.Ad);
//                                await sender.Send(izinPeriyotCreateCommand);
//                            }
//                        }
//                        if(adim.AdimSirasi == 1 && request.OnayDurum == DegerlendirmeDurumEnum.Onaylandi && adimlar.Any(p => p.AdimSirasi > adim.AdimSirasi))
//                        {
//                            izinTalep.DegerlendirmeDurumu = DegerlendirmeDurumEnum.Beklemede;
                            
//                        }
//                    }

//                    if ((adim.OnaySureciAdimi.Rol == RolTipiEnum.DepartmanYardimci || adim.OnaySureciAdimi.Rol == RolTipiEnum.DepartmanYonetici) && personelAtama.RolTipi == adim.OnaySureciAdimi.Rol && personelAtama.DepartmanId == izinTalepPersonelAtama.DepartmanId)
//                    {
//                        adim.DegerlendirmeDurumu = DegerlendirmeDurumEnum.FromValue(request.OnayDurum);
//                        adim.DegerlendirilmeTarihi = DateTimeOffset.Now;
//                        adim.DegerlendirenId = personel.Id;
//                        talepDegerlendirmeRepository.Update(adim);

//                        if (!adimlar.Any(p => p.AdimSirasi > adim.AdimSirasi) || DegerlendirmeDurumEnum.FromValue(request.OnayDurum) == DegerlendirmeDurumEnum.Reddedildi)
//                        {
//                            izinTalep.DegerlendirmeDurumu = DegerlendirmeDurumEnum.FromValue(request.OnayDurum);
//                            izinTalepRepository.Update(izinTalep);

//                            if (request.OnayDurum == DegerlendirmeDurumEnum.Onaylandi)
//                            {
//                                IzinPeriyotCreateCommand izinPeriyotCreateCommand = new(izinTalep.PersonelId, izinTalep.BaslangicTarihi, izinTalep.BitisTarihi, izinTalep.IzinTur.Ad);
//                                await sender.Send(izinPeriyotCreateCommand);
//                            }
//                        }
//                        if (adim.AdimSirasi == 1 && request.OnayDurum == DegerlendirmeDurumEnum.Onaylandi && adimlar.Any(p => p.AdimSirasi > adim.AdimSirasi))
//                        {
//                            izinTalep.DegerlendirmeDurumu = DegerlendirmeDurumEnum.Beklemede;

//                        }
//                    }
//                    if ((adim.OnaySureciAdimi.Rol == RolTipiEnum.SubeYardimci || adim.OnaySureciAdimi.Rol == RolTipiEnum.SubeYonetici) && personelAtama.RolTipi == adim.OnaySureciAdimi.Rol && personelAtama.SubeId == izinTalepPersonelAtama.SubeId)
//                    {
//                        adim.DegerlendirmeDurumu = DegerlendirmeDurumEnum.FromValue(request.OnayDurum);
//                        adim.DegerlendirilmeTarihi = DateTimeOffset.Now;
//                        adim.DegerlendirenId = personel.Id;
//                        talepDegerlendirmeRepository.Update(adim);

//                        if (!adimlar.Any(p => p.AdimSirasi > adim.AdimSirasi) || DegerlendirmeDurumEnum.FromValue(request.OnayDurum) == DegerlendirmeDurumEnum.Reddedildi)
//                        {
//                            izinTalep.DegerlendirmeDurumu = DegerlendirmeDurumEnum.FromValue(request.OnayDurum);
//                            izinTalepRepository.Update(izinTalep);

//                            if (request.OnayDurum == DegerlendirmeDurumEnum.Onaylandi)
//                            {
//                                IzinPeriyotCreateCommand izinPeriyotCreateCommand = new(izinTalep.PersonelId, izinTalep.BaslangicTarihi, izinTalep.BitisTarihi, izinTalep.IzinTur.Ad);
//                                await sender.Send(izinPeriyotCreateCommand);
//                            }
//                        }
//                        if (adim.AdimSirasi == 1 && request.OnayDurum == DegerlendirmeDurumEnum.Onaylandi && adimlar.Any(p => p.AdimSirasi > adim.AdimSirasi))
//                        {
//                            izinTalep.DegerlendirmeDurumu = DegerlendirmeDurumEnum.Beklemede;

//                        }
//                    }
//                    if ((adim.OnaySureciAdimi.Rol == RolTipiEnum.SirketYardimci || adim.OnaySureciAdimi.Rol == RolTipiEnum.SirketYonetici) && personelAtama.RolTipi == adim.OnaySureciAdimi.Rol && personelAtama.SirketId == izinTalepPersonelAtama.SirketId)
//                    {
//                        adim.DegerlendirmeDurumu = DegerlendirmeDurumEnum.FromValue(request.OnayDurum);
//                        adim.DegerlendirilmeTarihi = DateTimeOffset.Now;
//                        adim.DegerlendirenId = personel.Id;
//                        talepDegerlendirmeRepository.Update(adim);

//                        if (!adimlar.Any(p => p.AdimSirasi > adim.AdimSirasi) || DegerlendirmeDurumEnum.FromValue(request.OnayDurum) == DegerlendirmeDurumEnum.Reddedildi)
//                        {
//                            izinTalep.DegerlendirmeDurumu = DegerlendirmeDurumEnum.FromValue(request.OnayDurum);
//                            izinTalepRepository.Update(izinTalep);

//                            if (request.OnayDurum == DegerlendirmeDurumEnum.Onaylandi)
//                            {
//                                IzinPeriyotCreateCommand izinPeriyotCreateCommand = new(izinTalep.PersonelId, izinTalep.BaslangicTarihi, izinTalep.BitisTarihi, izinTalep.IzinTur.Ad);
//                                await sender.Send(izinPeriyotCreateCommand);
//                            }
//                        }
//                        if (adim.AdimSirasi == 1 && request.OnayDurum == DegerlendirmeDurumEnum.Onaylandi && adimlar.Any(p => p.AdimSirasi > adim.AdimSirasi))
//                        {
//                            izinTalep.DegerlendirmeDurumu = DegerlendirmeDurumEnum.Beklemede;

//                        }
//                    }
//                }
//                else
//                {
//                    return Result<string>.Failure("İzin talebi degerlendirilemedi çünkü önceki adımlar hala değerlendirilmemiş.");
//                }
               
//            }
            
//        }
//        var affectedColumns = await unitOfWork.SaveChangesAsync();


//        //Bildirim bildirim = new()
//        //{
//        //    Baslik = "İzin talebi değerlendirildi",
//        //    Aciklama = $"{personel.FullName} tarafından, {izinTalep.ToplamSure} günlük izin talebiniz {DegerlendirmeDurumEnum.FromValue(request.OnayDurum)}.",
//        //    CreatedAt = DateTimeOffset.Now,
//        //    BildirimTipi = BildirimTipiEnum.Bilgilendirme,
//        //    AliciTipi = AliciTipiEnum.Personel,
//        //    AliciId = izinTalep.PersonelId,
//        //};

//        //await bildirimService.KullaniciyaBildirimGonderAsync(bildirim, izinTalep.PersonelId);

//        if(affectedColumns != 0)
//        {
//            if (request.OnayDurum == 0)
//                return Result<string>.Succeed("İzin talebi onaylandı.");
//            if (request.OnayDurum == 1)
//                return Result<string>.Succeed("İzin talebi reddedildi");
//        }
//        else
//        {
//            return Result<string>.Failure("İzin talebi değerlendirilemedi");
//        }
//            return Result<string>.Succeed("İzin talebi onay durumu değerlendirildi");
//    }
//}
