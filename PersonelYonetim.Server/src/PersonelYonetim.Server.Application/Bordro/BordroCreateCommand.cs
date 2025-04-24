using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.MaasPusulalar;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.Bordro;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Tenants;
using PersonelYonetim.Server.Domain.UnitOfWork;
using System.Globalization;
using TS.Result;

namespace PersonelYonetim.Server.Application.Bordro;
public sealed record BordroCreateCommand(
    int Yil,
    int Ay,
    List<Guid>? PersonelId,
    bool tekrarHesapla = true
    ) : IRequest<Result<string>>;

internal sealed class BordroCreateCommandHandler(
    ICurrentUserService currentUserService,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    ICalismaTakvimRepository calismaTakvimRepository,
    IBordroDonemRepository bordroDonemRepository,
    IMaasPusulaRepository maasPusulaRepository,
    IKazancBilesenRepository kazancBilesenRepository,
    IKesintiBilesenRepository kesintiBilesenRepository,
    IBildirimService bildirimService,
    ITenantRepository tenantRepository,
    IUnitOfWork unitOfWork,
    ISender sender
    ) : IRequestHandler<BordroCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(BordroCreateCommand request, CancellationToken cancellationToken)
    {
        using(var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                Guid? userId = currentUserService.UserId;
                Guid? tenantId = currentUserService.TenantId;
                List<Guid> pusulaIdler = new List<Guid>();

                bool isReCalculated = true;

                if (!userId.HasValue | !tenantId.HasValue)
                    return Result<string>.Failure("Şirket bulunamadı");

                var tenant = await tenantRepository.FirstOrDefaultAsync(p => p.Id == tenantId);

                if (tenant is null)
                    return Result<string>.Failure("Tenant bulunamamdı");

                var personelGorevlendirmeler = await personelGorevlendirmeRepository.Where(p => p.TenantId == tenantId && !p.IsDeleted && (request.PersonelId != null ? request.PersonelId.Contains(p.Id) : false) && p.IseGirisTarihi.Year <= request.Yil && p.IseGirisTarihi.Month <= request.Ay).ToListAsync();

                var bordroDonem = await bordroDonemRepository.FirstOrDefaultAsync(b => b.TenantId == tenantId && b.Yil == request.Yil && b.Ay == request.Ay);

                if (bordroDonem is null)
                {
                    bordroDonem = new()
                    {
                        Yil = request.Yil,
                        Ay = request.Ay,
                        DonemBaslangic = new DateTimeOffset(request.Yil, request.Ay, 1, 0, 0, 0, TimeSpan.Zero),
                        DonemBitis = new DateTimeOffset(request.Yil, request.Ay, DateTime.DaysInMonth(request.Yil, request.Ay), 23, 59, 59, TimeSpan.Zero),
                        TenantId = tenantId!.Value
                    };
                    bordroDonemRepository.Add(bordroDonem);
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }

                var oncekiBordroDonem = await bordroDonemRepository.Where(b => b.TenantId == tenantId && b.Yil == bordroDonem.Yil && b.Ay == bordroDonem.Ay - 1).Include(p => p.MaasPusulalar).FirstOrDefaultAsync();

                if (request.Ay == 1)
                {
                    oncekiBordroDonem = null;
                }
                decimal? sgkIsciOrani = null;
                decimal? sgkIsverenOrani = null;
                decimal? issizlikIsciOrani = null;
                decimal? issizlikIsverenOrani = null;
                decimal? damgaVergisiOrani = null;
                decimal? gelirVergisiOrani = null;
                foreach (var personelGorevlendirme in personelGorevlendirmeler)
                {
                    isReCalculated = true;
                    var calismaTakvim = await calismaTakvimRepository.Where(p => personelGorevlendirme.CalismaTakvimId == p.Id).Include(p => p.CalismaGunler).FirstOrDefaultAsync();

                    if (calismaTakvim is null)
                        return Result<string>.Failure("Calışma takvim bulunamamdı");

                    var calismaGunler = calismaTakvim.CalismaGunler.ToList();

                    if (!calismaGunler.Any())
                        return Result<string>.Failure("Çalışma günleri bulunamamdı");

                    var maasPusula = await maasPusulaRepository.Where(p => p.BordroDonemId == bordroDonem.Id && p.PersonelId == personelGorevlendirme.PersonelId && !p.IsDeleted).Include(p => p.KazancBilesenleri).Include(p => p.KesintiBilesenleri).FirstOrDefaultAsync();
                    decimal toplamKazanc = 0m;
                    decimal toplamKesinti = 0m;

                    var kazancBilesenleri = maasPusula?.KazancBilesenleri;
                    var kesintiBilesenleri = maasPusula?.KesintiBilesenleri;

                    if (maasPusula is null || request.tekrarHesapla)
                    {
                        if (maasPusula is not null)
                        {
                            if(kazancBilesenleri is not null && kazancBilesenleri.Count() > 0)
                            {
                                foreach(var kazancBilesen in kazancBilesenleri)
                                {
                                    toplamKazanc += kazancBilesen.Tutar;
                                }
                            }

                            if (kesintiBilesenleri is not null && kesintiBilesenleri.Count() > 0)
                            {
                                foreach (var kesintiBilesen in kesintiBilesenleri)
                                {
                                    toplamKesinti += kesintiBilesen.Tutar;
                                }
                            }

                            maasPusula.IsDeleted = true;
                            maasPusula.IsActive = false;
                            maasPusulaRepository.Update(maasPusula);

                            maasPusula.IsDeleted = true;
                            maasPusulaRepository.Update(maasPusula);

                            //if (maasPusula.SGKPrimiIsciOrani != null)
                            //    sgkIsciOrani = maasPusula.SGKPrimiIsciOrani.Value;
                            //else
                            //    sgkIsciOrani = tenant.SGKPrimIsciKesintiOrani;
                            //if (maasPusula.IssizlikPrimiIsciOrani != null)
                            //    issizlikIsciOrani = maasPusula.IssizlikPrimiIsciOrani.Value;
                            //else
                            //    issizlikIsciOrani = tenant.SGKIssizlikPrimIsciKesintiOrani;

                            //if (maasPusula.SGKPrimiIsverenOrani != null)
                            //    sgkIsverenOrani = maasPusula.SGKPrimiIsverenOrani.Value;
                            //else
                            //    sgkIsverenOrani = tenant.SGKPrimIsverenKesintiOrani;
                            //if (maasPusula.IssizlikPrimiIsverenOrani != null)
                            //    issizlikIsverenOrani = maasPusula.IssizlikPrimiIsverenOrani.Value;
                            //else
                            //    issizlikIsverenOrani = tenant.SGKIssizlikPrimIsverenKesintiOrani;

                            //if (maasPusula.DamgaVergisiOrani != null)
                            //    damgaVergisiOrani = maasPusula.DamgaVergisiOrani.Value;
                            //else
                            //    damgaVergisiOrani = tenant.DamgaVergisiOrani;

                            //if (maasPusula.GelirVergisiOrani != null)
                            //    gelirVergisiOrani = maasPusula.GelirVergisiOrani;

                            await unitOfWork.SaveChangesAsync(cancellationToken);
                        }

                        if (maasPusula is null)
                        {
                        }


                        isReCalculated = false;
                        sgkIsciOrani = tenant.SGKPrimIsciKesintiOrani;
                        issizlikIsciOrani = tenant.SGKIssizlikPrimIsciKesintiOrani;
                        sgkIsverenOrani = tenant.SGKPrimIsverenKesintiOrani;
                        issizlikIsverenOrani = tenant.SGKIssizlikPrimIsverenKesintiOrani;
                        damgaVergisiOrani = tenant.DamgaVergisiOrani;

                        int SGKGunSayisi;

                        var iseGirisTarihi = personelGorevlendirme.IseGirisTarihi;
                        var donemBaslangic = new DateTimeOffset(bordroDonem.Yil, bordroDonem.Ay, 1, 0, 0, 0, DateTimeOffset.Now.Offset);
                        var donemBitis = new DateTimeOffset(bordroDonem.Yil, bordroDonem.Ay, DateTime.DaysInMonth(bordroDonem.Yil, bordroDonem.Ay), 0, 0, 0, DateTimeOffset.Now.Offset);

                        if (iseGirisTarihi > donemBaslangic)
                        {
                            SGKGunSayisi = (int)(donemBitis - iseGirisTarihi).Days + 1;
                        }
                        else
                        {
                            SGKGunSayisi = 30;
                        }

                        decimal AsgariUcret = tenant.AsgariUcret;

                        decimal BrutUcret = (personelGorevlendirme.BrutUcret / 30m) * SGKGunSayisi;
                        decimal EkKazancToplam = toplamKazanc;//KazancBilesenleri eklenecek
                        decimal ToplamBrutKazanc = BrutUcret + EkKazancToplam;

                        decimal SGKPrimiIsci = ToplamBrutKazanc * sgkIsciOrani!.Value;
                        decimal IssizlikPrimiIsci = ToplamBrutKazanc * issizlikIsciOrani!.Value;

                        decimal SGKMatrahi = ToplamBrutKazanc;
                        decimal GelirVergisiMatrahi = ToplamBrutKazanc - SGKPrimiIsci - IssizlikPrimiIsci;
                        decimal KumulatifGelirVergisiMatrahiOnceki = oncekiBordroDonem != null ? oncekiBordroDonem.MaasPusulalar.FirstOrDefault(p => p.PersonelId == personelGorevlendirme.PersonelId && p.TenantId == tenantId) != null ? oncekiBordroDonem.MaasPusulalar.FirstOrDefault(p => p.PersonelId == personelGorevlendirme.PersonelId && p.TenantId == tenantId)!.KumulatifGelirVergisiMatrahiDonemSonu : 0:0;
                        decimal KumulatifGelirVergisiMatrahiDonemSonu = KumulatifGelirVergisiMatrahiOnceki + GelirVergisiMatrahi;

                        if(gelirVergisiOrani is null)
                            gelirVergisiOrani = HesaplaGelirVergisiOrani(GelirVergisiMatrahi);

                        decimal HesaplananGelirVergisi = gelirVergisiOrani * GelirVergisiMatrahi ?? HesaplaBuAykiGelirVergisi(KumulatifGelirVergisiMatrahiOnceki, GelirVergisiMatrahi);
                        decimal GelirVergisiIstisnasiUygulanan = AsgariUcret * gelirVergisiOrani!.Value;
                        decimal HesaplananDamgaVergisi = ToplamBrutKazanc * damgaVergisiOrani!.Value;
                        decimal DamgaVergisiIstisnasiUygulanan = AsgariUcret * damgaVergisiOrani!.Value;

                        decimal OdenecekGelirvergisi = (HesaplananGelirVergisi - GelirVergisiIstisnasiUygulanan) < 0 ? 0 : (HesaplananGelirVergisi - GelirVergisiIstisnasiUygulanan);
                        decimal OdenecekDamgaVergisi = (HesaplananDamgaVergisi - DamgaVergisiIstisnasiUygulanan) < 0 ? 0 : (HesaplananDamgaVergisi - DamgaVergisiIstisnasiUygulanan);
                        decimal DigerKesintilerToplam = toplamKesinti; // KesintiBilesenleri eklenebilir
                        decimal Toplamkesinti = SGKPrimiIsci + IssizlikPrimiIsci + OdenecekGelirvergisi + OdenecekDamgaVergisi + DigerKesintilerToplam;
                        decimal NetMaas = ToplamBrutKazanc - Toplamkesinti;

                        decimal SGKPrimiIsveren = SGKMatrahi * sgkIsverenOrani!.Value;
                        decimal IssizlikPrimiIsveren = SGKMatrahi * issizlikIsverenOrani!.Value;
                        decimal ToplamIsverenMaliyeti = ToplamBrutKazanc + SGKPrimiIsveren + IssizlikPrimiIsveren;
                        

                        int FiiliCalismaGunu = HesaplaFiiliCalismaGunuSayisi(new DateTime(bordroDonem.Yil,bordroDonem.Ay,1),calismaGunler,iseGirisTarihi > donemBaslangic ? iseGirisTarihi:donemBaslangic);

                        bool besKesintisiVarMi = personelGorevlendirme.BesKesintisiVarMi;
                        decimal BesKesintiTutari = besKesintisiVarMi ? personelGorevlendirme.BesKesintiOrani ?? 0.05m * BrutUcret : 0m;

                        maasPusula = new MaasPusula()
                        {
                            PersonelId = personelGorevlendirme.PersonelId,
                            BordroDonemId = bordroDonem.Id,
                            SGKDurumu = "Normal",

                            TabiKanunKodu = "5510",

                            // Kazanç Özetleri
                            BrutUcret = BrutUcret,
                            EkKazancToplam = EkKazancToplam,
                            ToplamBrutKazanc = ToplamBrutKazanc,

                            // Hesaplama Matrahları ve Ara Değerler
                            SGKMatrahi = SGKMatrahi,
                            GelirVergisiMatrahi = GelirVergisiMatrahi,
                            KumulatifGelirVergisiMatrahiOnceki = KumulatifGelirVergisiMatrahiOnceki,
                            KumulatifGelirVergisiMatrahiDonemSonu = KumulatifGelirVergisiMatrahiDonemSonu,
                            HesaplananGelirVergisi = HesaplananGelirVergisi,
                            GelirVergisiIstisnasiUygulanan = GelirVergisiIstisnasiUygulanan,
                            HesaplananDamgaVergisi = HesaplananDamgaVergisi,
                            DamgaVergisiIstisnasiUygulanan = DamgaVergisiIstisnasiUygulanan,

                            // Kesinti Özetleri
                            SGKPrimiIsci = SGKPrimiIsci,
                            IssizlikPrimiIsci = IssizlikPrimiIsci,
                            OdenecekGelirVergisi = OdenecekGelirvergisi,
                            OdenecekDamgaVergisi = OdenecekDamgaVergisi,
                            DigerKesintilerToplam = DigerKesintilerToplam,
                            ToplamKesinti = Toplamkesinti,
                            NetMaas = NetMaas,

                            // İşveren Maliyeti ve SGK/MUHSGK Bilgileri
                            SGKPrimiIsveren = SGKPrimiIsveren,
                            IssizlikPrimiIsveren = IssizlikPrimiIsveren,
                            ToplamIsverenMaliyeti = ToplamIsverenMaliyeti,
                            SGKGunSayisi = SGKGunSayisi,
                            FiiliCalismaGunu = FiiliCalismaGunu,
                            BesKesintisiVarMi = besKesintisiVarMi,
                            BesKesintiTutari = BesKesintiTutari,

                            Yil = bordroDonem.Yil,
                            Ay = bordroDonem.Ay,

                            //Oranlar
                            SGKPrimiIsciOrani = sgkIsciOrani,
                            SGKPrimiIsverenOrani = sgkIsverenOrani,
                            IssizlikPrimiIsciOrani = issizlikIsciOrani,
                            IssizlikPrimiIsverenOrani = issizlikIsverenOrani,
                            GelirVergisiOrani  = HesaplaGelirVergisiOrani(KumulatifGelirVergisiMatrahiDonemSonu),
                            DamgaVergisiOrani = damgaVergisiOrani,

                            TenantId = tenantId!.Value
                            };
                        maasPusulaRepository.Add(maasPusula);
                        pusulaIdler.Add(maasPusula.Id);

                        await unitOfWork.SaveChangesAsync(cancellationToken);

                        if (kazancBilesenleri is not null && kazancBilesenleri.Count() > 0)
                        {
                            foreach (var kazancBilesen in kazancBilesenleri)
                            {
                                kazancBilesen.MaasPusulaId = maasPusula.Id;
                                kazancBilesenRepository.Update(kazancBilesen);
                            }
                        }

                        if (kesintiBilesenleri is not null && kesintiBilesenleri.Count() > 0)
                        {
                            foreach (var kesintiBilesen in kesintiBilesenleri)
                            {
                                kesintiBilesen.MaasPusulaId = maasPusula.Id;
                                kesintiBilesenRepository.Update(kesintiBilesen);
                            }
                        }
                        await unitOfWork.SaveChangesAsync();

                        if (isReCalculated)
                        {
                            Bildirim bildirim = new()
                            {
                                Baslik = $"Aylık bordro tekrardan hesaplandı",
                                Aciklama = $"{CultureInfo.GetCultureInfo("tr-TR").DateTimeFormat.GetMonthName(maasPusula.Ay)}-{maasPusula.Yil} maas pusulanız oluşturuldu. Bordrolarım sekmesinden 5 gün içinde değerlendirmezseniz otomatik olarak onaylanacaktır.",
                                CreatedAt = DateTimeOffset.Now,
                                BildirimTipi = BildirimTipiEnum.Onay,
                                AliciTipi = AliciTipiEnum.Personel,
                                AliciId = maasPusula.PersonelId,
                                TenantId = tenantId.Value,
                            };

                            await bildirimService.KullaniciyaBildirimGonderAsync(bildirim, maasPusula.PersonelId);
                        }
                        else
                        {
                            Bildirim bildirim = new()
                            {
                                Baslik = $"Aylık bordro oluşturuldu",
                                Aciklama = $"{CultureInfo.GetCultureInfo("tr-TR").DateTimeFormat.GetMonthName(maasPusula.Ay)}-{maasPusula.Yil} maas pusulanız oluşturuldu. Bordrolarım sekmesinden 5 gün içinde değerlendirmezseniz otomatik olarak onaylanacaktır.",
                                CreatedAt = DateTimeOffset.Now,
                                BildirimTipi = BildirimTipiEnum.Onay,
                                AliciTipi = AliciTipiEnum.Personel,
                                AliciId = maasPusula.PersonelId,
                                TenantId = tenantId.Value,
                            };

                            await bildirimService.KullaniciyaBildirimGonderAsync(bildirim, maasPusula.PersonelId);
                        }
                    }
                }
               if(pusulaIdler.Count > 0)
                {
                    foreach(var pusulaId in pusulaIdler)
                    {
                        MaasPusulaPDFCreateCommand maasPusulaPDFCreateCommand = new(pusulaId);
                        await sender.Send(maasPusulaPDFCreateCommand);
                    }
                }

                await unitOfWork.CommitTransactionAsync(transaction);

                return Result<string>.Succeed(isReCalculated ? "Bordro tekrar hesaplandı" : "Bordro oluşturuldu");

            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure("Hata oluştu " + ex.Message);
            }
        }
    }
    public decimal HesaplaBuAykiGelirVergisi(decimal oncekiMatrah, decimal buAykiMatrah)
    {
        var toplamMatrah = oncekiMatrah + buAykiMatrah;
        return HesaplaGelirVergisi(toplamMatrah) - HesaplaGelirVergisi(oncekiMatrah);
    }
    private decimal HesaplaGelirVergisi(decimal matrah)
    {
        if (matrah <= 110000m)
            return matrah * 0.15m;
        else if (matrah <= 230000m)
            return (110000m * 0.15m) + ((matrah - 110000m) * 0.20m);
        else if (matrah <= 580000m)
            return (110000m * 0.15m) + (120000m * 0.20m) + ((matrah - 230000m) * 0.27m);
        else if (matrah <= 3000000m)
            return (110000m * 0.15m) + (120000m * 0.20m) + (350000m * 0.27m) + ((matrah - 580000m) * 0.35m);
        else
            return (110000m * 0.15m) + (120000m * 0.20m) + (350000m * 0.27m) + (2420000m * 0.35m) + ((matrah - 3000000m) * 0.40m);
    }
    private decimal HesaplaGelirVergisiOrani(decimal matrah)
    {
        if (matrah <= 110000m)
            return 0.15m;
        else if (matrah <= 230000m)
            return 0.20m;
        else if (matrah <= 580000m)
            return 0.27m;
        else if (matrah <= 3000000m)
            return 0.35m;
        else
            return 0.40m;
    }

    public int HesaplaFiiliCalismaGunuSayisi(DateTime ay, List<CalismaGun> takvim, DateTimeOffset baslangicTarihi)
    {
        int yil = ay.Year;
        int ayNo = ay.Month;
        int toplamGun = DateTime.DaysInMonth(yil, ayNo);

        DateTimeOffset ayIlkGunu = new DateTime(yil, ayNo, 1);
        DateTimeOffset baslangic = baslangicTarihi > ayIlkGunu ? baslangicTarihi : ayIlkGunu;

        int fiiliGunSayisi = 0;

        for (DateTimeOffset tarih = baslangic; tarih.Month == ayNo; tarih = tarih.AddDays(1))
        {
            var gunBilgisi = takvim.FirstOrDefault(x => x.Gun == tarih.DayOfWeek);
            if (gunBilgisi != null && gunBilgisi.IsCalismaGunu)
            {
                fiiliGunSayisi++;
            }
        }

        return fiiliGunSayisi;
    }
}
