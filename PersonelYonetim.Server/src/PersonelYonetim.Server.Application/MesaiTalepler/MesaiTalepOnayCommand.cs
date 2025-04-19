//using MediatR;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using PersonelYonetim.Server.Application.CalismaCizelgeleri;
//using PersonelYonetim.Server.Domain.Bildirimler;
//using PersonelYonetim.Server.Domain.Izinler;
//using PersonelYonetim.Server.Domain.Mesailer;
//using PersonelYonetim.Server.Domain.OnaySurecleri;
//using PersonelYonetim.Server.Domain.PersonelAtamalar;
//using PersonelYonetim.Server.Domain.Personeller;
//using PersonelYonetim.Server.Domain.Rols;
//using PersonelYonetim.Server.Domain.UnitOfWork;
//using PersonelYonetim.Server.Domain.ZamanYonetimler;
//using System.Security.Claims;


//namespace PersonelYonetim.Server.Application.MesaiTalepler;
//public sealed record MesaiTalepOnayCommand(
//    Guid Id,
//    int OnayDurum) : IRequest<Result<string>>;

//internal sealed class MesaiTalepOnayCommandHandler(
//    IHttpContextAccessor httpContextAccessor,
//    IPersonelRepository personelRepository,
//    IMesaiTalepRepository mesaiTalepRepository,
//    IPersonelAtamaRepository personelAtamaRepository,
//    ITalepDegerlendirmeRepository talepDegerlendirmeRepository,
//    IBildirimService bildirimService,
//    ISender sender,
//    IUnitOfWork unitOfWork
//    ) : IRequestHandler<MesaiTalepOnayCommand, Result<string>>
//{
//    public async Task<Result<string>> Handle(MesaiTalepOnayCommand request, CancellationToken cancellationToken)
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

//        MesaiTalep? mesaiTalep = await mesaiTalepRepository.Where(p => p.Id == request.Id).Include(p => p.Personel).ThenInclude(p => p!.PersonelAtamalar)
//            .Include(p => p.DegerlendirmeAdimlari).ThenInclude(p => p.OnaySureciAdimi).FirstOrDefaultAsync(cancellationToken);

//        if (mesaiTalep is null)
//            return Result<string>.Failure("Mesai talebi bulunamadı");

//        PersonelAtama personelAtama = await personelAtamaRepository.FirstOrDefaultAsync(p => p.PersonelId == personel.Id && p.SirketId == mesaiTalep.SirketId && p.IsDeleted == false);
//        if (personelAtama is null)
//            return Result<string>.Failure("Personel ataması bulunamadı");

//        var adimlar = mesaiTalep.DegerlendirmeAdimlari;

//        if (mesaiTalep.Personel is null)
//            return Result<string>.Failure("Mesai talebi personel bulunamadı");

//        var mesaiTalepPersonelAtama = mesaiTalep.Personel!.PersonelAtamalar.Where(pa => pa.IsDeleted == false && pa.SirketId == mesaiTalep.SirketId).FirstOrDefault();
//        if (mesaiTalepPersonelAtama is null)
//            return Result<string>.Failure("İzin talep eden personel ataması bulunamadı");

//        foreach (var adim in adimlar)
//        {
//            if (adim.OnaySureciAdimi != null)
//            {
//                if (!adimlar.Any(a => a.AdimSirasi < adim.AdimSirasi && a.DegerlendirmeDurumu == DegerlendirmeDurumEnum.Beklemede))
//                {
//                    if (adim.OnaySureciAdimi.PersonelId != null && adim.OnaySureciAdimi.PersonelId == personel.Id && !adimlar.Any(a => a.AdimSirasi < adim.AdimSirasi && a.DegerlendirmeDurumu == DegerlendirmeDurumEnum.Beklemede))
//                    {
//                        adim.DegerlendirmeDurumu = DegerlendirmeDurumEnum.FromValue(request.OnayDurum);
//                        adim.DegerlendirilmeTarihi = DateTimeOffset.Now;
//                        adim.DegerlendirenId = personel.Id;
//                        talepDegerlendirmeRepository.Update(adim);

//                        if (!adimlar.Any(p => p.AdimSirasi > adim.AdimSirasi) || DegerlendirmeDurumEnum.FromValue(request.OnayDurum) == DegerlendirmeDurumEnum.Reddedildi)
//                        {
//                            mesaiTalep.MesaiDegerlendirmeDurum = MesaiDegerlendirmeDurumEnum.FromValue(request.OnayDurum);
//                            mesaiTalepRepository.Update(mesaiTalep);

//                            if (request.OnayDurum == MesaiDegerlendirmeDurumEnum.Onaylandi)
//                            {
//                                GunlukCalismaPeriyotCreateCommand calismaPeriyotCreateCommand = new(mesaiTalep.PersonelId,null,CalismaPeriyoduTipi.FazlaMesai, new TimeOnly(mesaiTalep.BaslangicTarihi.Hour, mesaiTalep.BaslangicTarihi.Minute), new TimeOnly(mesaiTalep.BitisTarihi.Hour, mesaiTalep.BitisTarihi.Minute));
//                                await sender.Send(calismaPeriyotCreateCommand);
//                            }
//                        }
//                        if (adim.AdimSirasi == 1 && request.OnayDurum == DegerlendirmeDurumEnum.Onaylandi && adimlar.Any(p => p.AdimSirasi > adim.AdimSirasi))
//                        {
//                            mesaiTalep.MesaiDegerlendirmeDurum = MesaiDegerlendirmeDurumEnum.Beklemede;

//                        }
//                    }

//                    if ((adim.OnaySureciAdimi.Rol == RolTipiEnum.DepartmanYardimci || adim.OnaySureciAdimi.Rol == RolTipiEnum.DepartmanYonetici) && personelAtama.RolTipi == adim.OnaySureciAdimi.Rol && personelAtama.DepartmanId == mesaiTalepPersonelAtama.DepartmanId)
//                    {
//                        adim.DegerlendirmeDurumu = DegerlendirmeDurumEnum.FromValue(request.OnayDurum);
//                        adim.DegerlendirilmeTarihi = DateTimeOffset.Now;
//                        adim.DegerlendirenId = personel.Id;
//                        talepDegerlendirmeRepository.Update(adim);

//                        if (!adimlar.Any(p => p.AdimSirasi > adim.AdimSirasi) || DegerlendirmeDurumEnum.FromValue(request.OnayDurum) == DegerlendirmeDurumEnum.Reddedildi)
//                        {
//                            mesaiTalep.MesaiDegerlendirmeDurum = MesaiDegerlendirmeDurumEnum.FromValue(request.OnayDurum);
//                            mesaiTalepRepository.Update(mesaiTalep);

//                            if (request.OnayDurum == MesaiDegerlendirmeDurumEnum.Onaylandi)
//                            {
//                                GunlukCalismaPeriyotCreateCommand calismaPeriyotCreateCommand = new(mesaiTalep.PersonelId, null, CalismaPeriyoduTipi.FazlaMesai, new TimeOnly(mesaiTalep.BaslangicTarihi.Hour, mesaiTalep.BaslangicTarihi.Minute), new TimeOnly(mesaiTalep.BitisTarihi.Hour, mesaiTalep.BitisTarihi.Minute));
//                                await sender.Send(calismaPeriyotCreateCommand);
//                            }
//                        }
//                        if (adim.AdimSirasi == 1 && request.OnayDurum == DegerlendirmeDurumEnum.Onaylandi && adimlar.Any(p => p.AdimSirasi > adim.AdimSirasi))
//                        {
//                            mesaiTalep.MesaiDegerlendirmeDurum = MesaiDegerlendirmeDurumEnum.Beklemede;

//                        }
//                    }
//                    if ((adim.OnaySureciAdimi.Rol == RolTipiEnum.SubeYardimci || adim.OnaySureciAdimi.Rol == RolTipiEnum.SubeYonetici) && personelAtama.RolTipi == adim.OnaySureciAdimi.Rol && personelAtama.SubeId == mesaiTalepPersonelAtama.SubeId)
//                    {
//                        adim.DegerlendirmeDurumu = DegerlendirmeDurumEnum.FromValue(request.OnayDurum);
//                        adim.DegerlendirilmeTarihi = DateTimeOffset.Now;
//                        adim.DegerlendirenId = personel.Id;
//                        talepDegerlendirmeRepository.Update(adim);

//                        if (!adimlar.Any(p => p.AdimSirasi > adim.AdimSirasi) || DegerlendirmeDurumEnum.FromValue(request.OnayDurum) == DegerlendirmeDurumEnum.Reddedildi)
//                        {
//                            mesaiTalep.MesaiDegerlendirmeDurum = MesaiDegerlendirmeDurumEnum.FromValue(request.OnayDurum);
//                            mesaiTalepRepository.Update(mesaiTalep);


//                            if (request.OnayDurum == MesaiDegerlendirmeDurumEnum.Onaylandi)
//                            {
//                                GunlukCalismaPeriyotCreateCommand calismaPeriyotCreateCommand = new(mesaiTalep.PersonelId, null, CalismaPeriyoduTipi.FazlaMesai, new TimeOnly(mesaiTalep.BaslangicTarihi.Hour, mesaiTalep.BaslangicTarihi.Minute), new TimeOnly(mesaiTalep.BitisTarihi.Hour, mesaiTalep.BitisTarihi.Minute));
//                                await sender.Send(calismaPeriyotCreateCommand);
//                            }
//                        }
//                        if (adim.AdimSirasi == 1 && request.OnayDurum == DegerlendirmeDurumEnum.Onaylandi && adimlar.Any(p => p.AdimSirasi > adim.AdimSirasi))
//                        {
//                            mesaiTalep.MesaiDegerlendirmeDurum = MesaiDegerlendirmeDurumEnum.Beklemede;

//                        }
//                    }
//                    if ((adim.OnaySureciAdimi.Rol == RolTipiEnum.SirketYardimci || adim.OnaySureciAdimi.Rol == RolTipiEnum.SirketYonetici) && personelAtama.RolTipi == adim.OnaySureciAdimi.Rol && personelAtama.SirketId == mesaiTalepPersonelAtama.SirketId)
//                    {
//                        adim.DegerlendirmeDurumu = DegerlendirmeDurumEnum.FromValue(request.OnayDurum);
//                        adim.DegerlendirilmeTarihi = DateTimeOffset.Now;
//                        adim.DegerlendirenId = personel.Id;
//                        talepDegerlendirmeRepository.Update(adim);

//                        if (!adimlar.Any(p => p.AdimSirasi > adim.AdimSirasi) || DegerlendirmeDurumEnum.FromValue(request.OnayDurum) == DegerlendirmeDurumEnum.Reddedildi)
//                        {
//                            mesaiTalep.MesaiDegerlendirmeDurum = MesaiDegerlendirmeDurumEnum.FromValue(request.OnayDurum);
//                            mesaiTalepRepository.Update(mesaiTalep);


//                            if (request.OnayDurum == MesaiDegerlendirmeDurumEnum.Onaylandi)
//                            {
//                                GunlukCalismaPeriyotCreateCommand calismaPeriyotCreateCommand = new(mesaiTalep.PersonelId, null, CalismaPeriyoduTipi.FazlaMesai, new TimeOnly(mesaiTalep.BaslangicTarihi.Hour, mesaiTalep.BaslangicTarihi.Minute), new TimeOnly(mesaiTalep.BitisTarihi.Hour, mesaiTalep.BitisTarihi.Minute));
//                                await sender.Send(calismaPeriyotCreateCommand);
//                            }
//                        }
//                        if (adim.AdimSirasi == 1 && request.OnayDurum == DegerlendirmeDurumEnum.Onaylandi && adimlar.Any(p => p.AdimSirasi > adim.AdimSirasi))
//                        {
//                            mesaiTalep.MesaiDegerlendirmeDurum = MesaiDegerlendirmeDurumEnum.Beklemede;

//                        }
//                    }
//                }
//                else
//                {
//                    return Result<string>.Failure("Mesai talebi degerlendirilemedi çünkü önceki adımlar hala değerlendirilmemiş.");
//                }

//            }

//        }
//        var affectedColumns = await unitOfWork.SaveChangesAsync();


//        Bildirim bildirim = new()
//        {
//            Baslik = "Mesai talebi değerlendirildi",
//            Aciklama = $"{personel.FullName} tarafından, {mesaiTalep.ToplamSure} saatlik mesai talebiniz {DegerlendirmeDurumEnum.FromValue(request.OnayDurum)}.",
//            CreatedAt = DateTimeOffset.Now,
//            BildirimTipi = BildirimTipiEnum.Bilgilendirme,
//            AliciTipi = AliciTipiEnum.Personel,
//            AliciId = mesaiTalep.PersonelId,
//        };

//        await bildirimService.KullaniciyaBildirimGonderAsync(bildirim, mesaiTalep.PersonelId);

//        if (affectedColumns != 0)
//        {
//            if (request.OnayDurum == 0)
//                return Result<string>.Succeed("Mesai talebi onaylandı.");
//            if (request.OnayDurum == 1)
//                return Result<string>.Succeed("Mesai talebi reddedildi");
//        }
//        else
//        {
//            return Result<string>.Failure("Mesai talebi değerlendirilemedi");
//        }
//        return Result<string>.Succeed("Mesai talebi onay durumu değerlendirildi");
//    }
//}

