using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.IzinTalepler;

public sealed record IzinTalepCreateCommand(
    DateTimeOffset BaslangicTarihi,
    DateTimeOffset BitisTarihi,
    Guid IzinTurId,
    string? Aciklama) : IRequest<Result<Guid>>;

public sealed class IzinTalepCreateCommandValidator : AbstractValidator<IzinTalepCreateCommand>
{
    public IzinTalepCreateCommandValidator()
    {
        RuleFor(x => x.BaslangicTarihi).NotEmpty().WithMessage("Başlangıç tarihi boş olamaz");
        RuleFor(x => x.BitisTarihi).NotEmpty().WithMessage("Bitiş tarihi boş olamaz")
            .GreaterThan(x => x.BaslangicTarihi).WithMessage("Bitiş tarihi başlangıç tarihinden sonra olmalıdır.");
    }
}

internal sealed class IzinTalepCreateCommandHandler(
    ICurrentUserService currentUserService,
    IIzinHesaplamaService izinHesaplamaService,
    IIzinHakSorgulamaService izinHakSorgulamaService,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    IGorevlendirmeIzinKuraliRepository gorevlendirmeIzinKuraliRepository,
    IOnaySurecRepository onaySurecRepository,
    IOnaylayiciResolverService onaylayiciResolverService,
    IIzinTalepRepository izinTalepRepository,
    IIzinTurRepository izinTurRepository,
    IUnitOfWork unitOfWork,
    ITalepDegerlendirmeRepository talepDegerlendirmeRepository,
    IBildirimService bildirimService
    ) : IRequestHandler<IzinTalepCreateCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(IzinTalepCreateCommand request, CancellationToken cancellationToken)
    {
        using (var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                Guid? userId = currentUserService.UserId;
                Guid? tenantId = currentUserService.TenantId;

                if (!userId.HasValue || !tenantId.HasValue)
                {
                    return Result<Guid>.Failure("Kullanıcı veya şirket kimliği bulunamadı.");
                }

                var personelGorevlendirme = await personelGorevlendirmeRepository.Where(pg => pg.Personel.UserId == userId && pg.TenantId == tenantId && pg.BirincilGorevMi && pg.IsActive && !pg.IsDeleted)
                    .Include(pg => pg.Personel)
                    .Include(pg => pg.KurumsalBirim)
                    .FirstOrDefaultAsync(cancellationToken);


                if (personelGorevlendirme is null)
                    return Result<Guid>.Failure("Personel görevlendirme bulunamadı");

                var personel = personelGorevlendirme.Personel;

                var izinTur = await izinTurRepository.FirstOrDefaultAsync(p => p.Id == request.IzinTurId && p.TenantId == tenantId, cancellationToken);
                if(izinTur is null)
                    return Result<Guid>.Failure("İzin tür bulunamadı");

                var sureHesaplamaResult = await izinHesaplamaService.HesaplaIzinSuresiAsync(personelGorevlendirme.CalismaTakvimId,request.BaslangicTarihi, request.BitisTarihi, izinTur, cancellationToken);

                if (!sureHesaplamaResult.IsSuccessful)
                {
                    return Result<Guid>.Failure($"İzin süresi hesaplanamadı: {sureHesaplamaResult.Data}");
                }

                decimal hesaplananToplamGun = sureHesaplamaResult.Data!.ToplamGun;
                DateTimeOffset mesaiBaslangicTarihi = sureHesaplamaResult.Data!.MesaiBaslangicTarihi;

                if (hesaplananToplamGun <= 0)
                {
                    return Result<Guid>.Failure("Hesaplanan izin süresi geçersiz.");
                }

                var hakKontrolResult = await izinHakSorgulamaService.TalepHakkiKontrolEtAsync(
                    personelGorevlendirme.Id,
                    request.IzinTurId,
                    hesaplananToplamGun,
                    request.BaslangicTarihi,
                    cancellationToken);

                if(!hakKontrolResult.IsSuccessful)
                    return Result<Guid>.Failure($"İzin hakkı kontrolü başarısız: {hakKontrolResult.ErrorMessages!.FirstOrDefault()}");

                var gorevlendirmeKural = await gorevlendirmeIzinKuraliRepository.Where(p => p.PersonelGorevlendirmeId == personelGorevlendirme.Id).Include(p => p.IzinKural).FirstOrDefaultAsync();
                if (gorevlendirmeKural is null)
                    return Result<Guid>.Failure("Kural ataması bulunamadı");

                OnaySurec? onaySurec = null;

                if (gorevlendirmeKural.OzelOnaySurecId.HasValue)
                {
                    onaySurec = await onaySurecRepository.Where(p => p.Id == gorevlendirmeKural.OzelOnaySurecId).Include(p => p.OnayAdimlari).FirstOrDefaultAsync();
                    if (onaySurec is null || !onaySurec.OnayAdimlari.Any())
                        return Result<Guid>.Failure("Onay sureci bulunamadı");
                }
                else if (gorevlendirmeKural.IzinKural.VarsayilanOnaySurecId.HasValue)
                {
                    onaySurec = await onaySurecRepository.Where(p => p.Id == gorevlendirmeKural.IzinKural.VarsayilanOnaySurecId).Include(p => p.OnayAdimlari).FirstOrDefaultAsync();
                    if (onaySurec is null || !onaySurec.OnayAdimlari.Any())
                        return Result<Guid>.Failure("Onay sureci bulunamadı");
                }
                else
                {
                    return Result<Guid>.Failure("Gecerli onay sureci bulunamadı");
                }

                var izinTalep = new IzinTalep( 
                   personelId: personel.Id,
                   baslangicTarihi: request.BaslangicTarihi,
                   bitisTarihi: request.BitisTarihi,  
                   mesaiBaslangicTarihi: mesaiBaslangicTarihi,
                   toplamSure: hesaplananToplamGun,
                   izinTurId: request.IzinTurId,
                   onaySurecId: onaySurec.Id,
                   aciklama: request.Aciklama,
                   tenantId: tenantId.Value
               );

                await izinTalepRepository.AddAsync(izinTalep, cancellationToken);

                Guid? ilkOnayciPersonelId = null;
                List<TalepDegerlendirme> talepDegerlendirmeler = new();

                if(onaySurec is not null)
                {
                    foreach(var onayAdimi in onaySurec.OnayAdimlari)
                    {
                        var talepDegerlendirme = new TalepDegerlendirme(
                         talepId: izinTalep.Id,
                         adimSirasi: onayAdimi.Sira,
                         onaySureciAdimiId: onayAdimi.Id,
                         talepTipi: OnaySurecTuruEnum.Izin,
                         tenantId: tenantId.Value
                         );

                        if(onayAdimi.Sira == 1)
                        {
                            var resolverResult = await onaylayiciResolverService.OnaylayiciGetirAsync(
                                onayAdimi, personelGorevlendirme, cancellationToken);

                            if (!resolverResult.IsSuccessful || !resolverResult.Data.HasValue)
                            {
                                await unitOfWork.RollbackTransactionAsync(transaction);
                                return Result<Guid>.Failure($"İlk onaycı belirlenemedi: {resolverResult.Data}");
                            }
                            ilkOnayciPersonelId = resolverResult.Data;
                            talepDegerlendirme.OnayciAta(ilkOnayciPersonelId, onayAdimi.RolId);
                        }
                        talepDegerlendirmeler.Add(talepDegerlendirme);
                    }
                    await talepDegerlendirmeRepository.AddRangeAsync(talepDegerlendirmeler);
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);

                if (ilkOnayciPersonelId.HasValue)
                {
                    Bildirim bildirim = new()
                    {
                        Baslik = "Yeni İzin Talebi",
                        Aciklama = $"{personel.Ad} {personel.Soyad} tarafından yeni bir izin talebi oluşturuldu.",
                        CreatedAt = DateTimeOffset.Now,
                        BildirimTipi = BildirimTipiEnum.Onay,
                        AliciTipi = AliciTipiEnum.Personel,
                        AliciId = ilkOnayciPersonelId,
                        TenantId = tenantId.Value,
                    };

                    await bildirimService.KullaniciyaBildirimGonderAsync(bildirim, ilkOnayciPersonelId.Value);
                }
             
                await unitOfWork.SaveChangesAsync();
                await unitOfWork.CommitTransactionAsync(transaction);
                return Result<Guid>.Succeed(izinTalep.Id);

            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<Guid>.Failure("İzin talebi oluşturulurken hata oluştu: " + ex.Message);
            }

        }
    }
}

