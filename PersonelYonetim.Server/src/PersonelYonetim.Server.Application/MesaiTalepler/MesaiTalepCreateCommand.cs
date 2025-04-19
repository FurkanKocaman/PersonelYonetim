
//using MediatR;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using PersonelYonetim.Server.Domain.Bildirimler;
//using PersonelYonetim.Server.Domain.CalismaTakvimleri;
//using PersonelYonetim.Server.Domain.Izinler;
//using PersonelYonetim.Server.Domain.Mesailer;
//using PersonelYonetim.Server.Domain.OnaySurecleri;
//using PersonelYonetim.Server.Domain.PersonelAtamalar;
//using PersonelYonetim.Server.Domain.Personeller;
//using PersonelYonetim.Server.Domain.Rols;
//using PersonelYonetim.Server.Domain.UnitOfWork;
//using System.Security.Claims;
//using TS.Result;

//namespace PersonelYonetim.Server.Application.MesaiTalepler;
//public sealed record MesaiTalepCreateCommand(
//    Guid? PersonelId,
//    string Aciklama,
//    DateTimeOffset BaslangicTarihi,
//    DateTimeOffset BitisTarihi,
//    Guid? SirketId) : IRequest<Result<string>>;

//internal sealed class MesaiTalepCreateCommandHandler(
//    IMesaiTalepRepository mesaiTalepRepository,
//    IPersonelAtamaRepository personelAtamaRepository,
//    IHttpContextAccessor httpContextAccessor,
//    IPersonelRepository personelRepository,
//    ITalepDegerlendirmeRepository talepDegerlendirmeRepository,
//    IBildirimService bildirimService,
//    IUnitOfWork unitOfWork
//    ) : IRequestHandler<MesaiTalepCreateCommand, Result<string>>
//{
//    public async Task<Result<string>> Handle(MesaiTalepCreateCommand request, CancellationToken cancellationToken)
//    {
//        using (var transaction = unitOfWork.BeginTransaction())
//        {
//            try
//            {
//                Guid PersonelId = Guid.Empty;

//                if(request.PersonelId is null)
//                {
//                    var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//                    if (string.IsNullOrEmpty(userIdString))
//                    {
//                        throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
//                    }

//                    var personel = personelRepository.GetAll()
//                        .Where(p => p.UserId == Guid.Parse(userIdString))
//                        .Select(p => new { p.Id })
//                        .FirstOrDefault();

//                    if (personel == null)
//                    {
//                        throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
//                    }

//                    PersonelId = personel.Id;
//                }
//                else
//                {
//                    PersonelId = request.PersonelId.Value;
//                }

//                var personelAtama = personelAtamaRepository.Where(p => p.PersonelId == PersonelId && p.IsActive && p.IsDeleted == false).Include(p => p.MesaiOnaySurec).ThenInclude(m => m!.OnayAdimlari).Include(p => p.Personel).Include(p => p.CalismaTakvimi).ThenInclude(p => p!.CalismaGunler).FirstOrDefault();
//                if (personelAtama is null)
//                    return Result<string>.Failure("Personel departmanı bulunamadı");

//                if(personelAtama.CalismaTakvimi is null)
//                    return Result<string>.Failure("Personel çalışma takvimi bulunamadı");

//                var calismaGunler = personelAtama.CalismaTakvimi.CalismaGunler;
//                var baslangic = request.BaslangicTarihi;
//                var bitis = request.BitisTarihi;

//                for (var tarih = baslangic.Date; tarih <= bitis.Date; tarih = tarih.AddDays(1))
//                {
//                    var gun = calismaGunler.FirstOrDefault(g => g.Gun == tarih.DayOfWeek && g.IsCalismaGunu);

//                    if (gun is null || gun.CalismaBaslangic is null || gun.CalismaBitis is null)
//                        continue;

//                    var gunBaslangic = new DateTimeOffset(tarih.Year, tarih.Month, tarih.Day,
//                        gun.CalismaBaslangic.Value.Hour, gun.CalismaBaslangic.Value.Minute, 0, baslangic.Offset);

//                    var gunBitis = new DateTimeOffset(tarih.Year, tarih.Month, tarih.Day,
//                        gun.CalismaBitis.Value.Hour, gun.CalismaBitis.Value.Minute, 0, bitis.Offset);

//                    if (baslangic < gunBitis && bitis > gunBaslangic)
//                    {
//                        return Result<string>.Failure("Mesai talebinde bulunduğunuz süre mevcut çalışma takviminde mesai saatleri içinde yer almakta.");
//                    }
//                }

//                MesaiTalep mesaiTalep = new()
//                {
//                    PersonelId = personelAtama.PersonelId,
//                    Aciklama = request.Aciklama,
//                    BaslangicTarihi = baslangic,
//                    BitisTarihi = bitis,
//                    ToplamSure = (decimal)(bitis - baslangic).TotalHours,
//                    MesaiDegerlendirmeDurum = MesaiDegerlendirmeDurumEnum.Beklemede,
//                    SirketId = personelAtama.SirketId
//                };

//                mesaiTalepRepository.Add(mesaiTalep);

//                var mesaiOnaySurec = personelAtama.MesaiOnaySurec;
//                if (mesaiOnaySurec is null)
//                    return Result<string>.Failure("Onay süreci bulunamadı");
                
//                List<TalepDegerlendirme> talepDegerlendirmeler = new();

//                var onayAdimlari = mesaiOnaySurec.OnayAdimlari.ToList();

//                if (onayAdimlari.Any())
//                {
//                    foreach (var onayAdimi in onayAdimlari)
//                    {
//                        TalepDegerlendirme talep = new()
//                        {
//                            TalepId = mesaiTalep.Id,
//                            TalepTuru = OnaySurecTuruEnum.Mesai,
//                            AdimSirasi = onayAdimi.Sira,
//                            OnaySureciAdimiId = onayAdimi.Id,
//                            DegerlendirenId = null,
//                            Degerlendiren = null,
//                        };
//                        talepDegerlendirmeler.Add(talep);
//                    }
//                }

//                talepDegerlendirmeRepository.AddRange(talepDegerlendirmeler);
//                var ilkAdim = onayAdimlari.OrderBy(p => p.Sira).First();

//                var aliciPersonelAtama = personelAtamaRepository.Where(p => ilkAdim.PersonelId == p.PersonelId ||
//                (ilkAdim.Rol == RolTipiEnum.DepartmanYardimci || ilkAdim.Rol == RolTipiEnum.DepartmanYonetici) ? p.DepartmanId == personelAtama.DepartmanId && p.RolTipi == 1 || p.RolTipi == 2
//                : (ilkAdim.Rol == RolTipiEnum.SubeYardimci || ilkAdim.Rol == RolTipiEnum.SubeYonetici) ? p.SubeId == personelAtama.SubeId && p.RolTipi == 3 || p.RolTipi == 4
//                : (ilkAdim.Rol == RolTipiEnum.SirketYardimci || ilkAdim.Rol == RolTipiEnum.SirketYonetici) ? p.SirketId == personelAtama.SirketId && p.RolTipi == 5 || p.RolTipi == 6 : false).FirstOrDefault();

//                Bildirim bildirim = new()
//                {
//                    Baslik = "Yeni Mesai Talebi",
//                    Aciklama = $"{personelAtama.Personel!.Ad} {personelAtama.Personel.Soyad} tarafından yeni bir mesai talebi oluşturuldu.",
//                    CreatedAt = DateTimeOffset.Now,
//                    BildirimTipi = BildirimTipiEnum.Onay,
//                    AliciTipi = AliciTipiEnum.Personel,
//                    AliciId = aliciPersonelAtama!.PersonelId
//                };

//                await bildirimService.KullaniciyaBildirimGonderAsync(bildirim, personelAtama.YoneticiId ?? aliciPersonelAtama.PersonelId);


//                await unitOfWork.SaveChangesAsync();
//                await unitOfWork.CommitTransactionAsync(transaction);
//                return Result<string>.Succeed("Mesai talebi oluşturuldu. Toplam " + (decimal)(request.BitisTarihi - request.BaslangicTarihi).TotalHours + " saat");

//            }
//            catch (Exception ex)
//            {
//                await unitOfWork.RollbackTransactionAsync(transaction);
//                return Result<string>.Failure("Mesai talebi oluşturulurken hata oluştu: " + ex.Message);
//            }

//        }
//    }
//}

