using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bordro;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.UnitOfWork;
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
    IBordroDonemRepository bordroDonemRepository,
    IMaasPusulaRepository maasPusulaRepository,
    IUnitOfWork unitOfWork
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

                if (!userId.HasValue | !tenantId.HasValue)
                    return Result<string>.Failure("Şirket bulunamadı");

                var personelGorevlendirmeler = await personelGorevlendirmeRepository.Where(p => p.TenantId == tenantId && !p.IsDeleted && p.BaslangicTarihi.Year <= request.Yil && p.BaslangicTarihi.Month <= request.Ay).ToListAsync();

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

                foreach (var personelGorevlendirme in personelGorevlendirmeler)
                {
                    var maasPusula = await maasPusulaRepository.FirstOrDefaultAsync(p => p.BordroDonemId == bordroDonem.Id && p.PersonelId == personelGorevlendirme.PersonelId);
                    if (maasPusula is null || request.tekrarHesapla)
                    {
                        if (maasPusula is not null)
                        {
                            maasPusulaRepository.Delete(maasPusula);
                            await unitOfWork.SaveChangesAsync(cancellationToken);
                        }


                        decimal AsgariUcret = 22003;

                        decimal BrutUcret = personelGorevlendirme.BrutUcret;
                        decimal EkKazancToplam = 0;
                        decimal ToplamBrutKazanc = BrutUcret + EkKazancToplam;

                        decimal SGKPrimiIsci = ToplamBrutKazanc * 14 / 100;
                        decimal IssizlikPrimiIsci = ToplamBrutKazanc * 1 / 100;

                        decimal SGKMatrahi = ToplamBrutKazanc;
                        decimal GelirVergisiMatrahi = ToplamBrutKazanc - SGKPrimiIsci - IssizlikPrimiIsci;
                        decimal KumulatifGelirVergisiMatrahiOnceki = 0;
                        decimal KumulatifGelirVergisiMatrahiDonemSonu = KumulatifGelirVergisiMatrahiOnceki + GelirVergisiMatrahi;
                        decimal HesaplananGelirVergisi = GelirVergisiMatrahi * 20 / 100; // gelir vergisi %20 olarak düşünüldü
                        decimal GelirVergisiIstisnasiUygulanan = 130;
                        decimal HesaplananDamgaVergisi = ToplamBrutKazanc * 759 / 100000;
                        decimal DamgaVergisiIstisnasiUygulanan = AsgariUcret * 759 / 100000;

                        decimal OdenecekGelirvergisi = HesaplananGelirVergisi - GelirVergisiIstisnasiUygulanan;
                        decimal OdenecekDamgaVergisi = HesaplananDamgaVergisi - DamgaVergisiIstisnasiUygulanan;
                        decimal DigerKesintilerToplam = 0;
                        decimal Toplamkesinti = SGKPrimiIsci + IssizlikPrimiIsci + OdenecekGelirvergisi + OdenecekDamgaVergisi + DigerKesintilerToplam;
                        decimal NetMaas = ToplamBrutKazanc - Toplamkesinti;

                        decimal SGKPrimiIsveren = SGKMatrahi * 20 / 100;
                        decimal IssizlikPrimiIsveren = SGKMatrahi * 2 / 100;
                        decimal ToplamIsverenMaliyeti = ToplamBrutKazanc + SGKPrimiIsveren + IssizlikPrimiIsveren;
                        int SGKGunSayisi = 30;
                        bool BesKesintisiVarMi = false;
                        decimal BesKesintiTutari = 0;

                        maasPusula = new MaasPusula()
                        {
                            PersonelId = personelGorevlendirme.PersonelId,
                            BordroDonemId = bordroDonem.Id,

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
                            BesKesintisiVarMi = BesKesintisiVarMi,
                            BesKesintiTutari = BesKesintiTutari,

                            TenantId = tenantId!.Value,
                        };

                        maasPusulaRepository.Add(maasPusula);

                    }
                }
                await unitOfWork.SaveChangesAsync();
                await unitOfWork.CommitTransactionAsync(transaction);

                return Result<string>.Succeed("Bordro oluşturuldu");

            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure("Hata oluştu " + ex.Message);
            }
        }
    }
}
