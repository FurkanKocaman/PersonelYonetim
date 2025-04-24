
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.Mesailer;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.MesaiTalepler;
public sealed record MesaiTalepCreateCommand(
    Guid? PersonelId,
    string Aciklama,
    DateTimeOffset BaslangicTarihi,
    DateTimeOffset BitisTarihi
    ) : IRequest<Result<string>>;

internal sealed class MesaiTalepCreateCommandHandler(
    IMesaiTalepRepository mesaiTalepRepository,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    ICurrentUserService currentUserService,
    ITalepDegerlendirmeRepository talepDegerlendirmeRepository,
    IBildirimService bildirimService,
    IOnaylayiciResolverService onaylayiciResolverService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<MesaiTalepCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(MesaiTalepCreateCommand request, CancellationToken cancellationToken)
    {
        using (var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                Guid? userId = currentUserService.UserId;
                Guid? tenantId = currentUserService.TenantId;

                if (!userId.HasValue || !tenantId.HasValue)
                {
                    return Result<string>.Failure("Kullanıcı veya şirket kimliği bulunamadı.");
                }

                var personelGorevlendirme = await personelGorevlendirmeRepository.Where(pg => pg.Personel.UserId == userId && pg.TenantId == tenantId && pg.BirincilGorevMi && pg.IsActive && !pg.IsDeleted)
                    .Include(pg => pg.Personel)
                    .Include(pg => pg.KurumsalBirim)
                    .Include(pg => pg.MesaiOnaySurec)
                    .ThenInclude(p => p!.OnayAdimlari)
                    .FirstOrDefaultAsync(cancellationToken);


                if (personelGorevlendirme is null)
                    return Result<string>.Failure("Personel görevlendirme bulunamadı");

                var personel = personelGorevlendirme.Personel;

                OnaySurec? onaySurec = personelGorevlendirme.MesaiOnaySurec;
                if (onaySurec is null || !onaySurec.OnayAdimlari.Any())
                    return Result<string>.Failure("Onay sureci bulunamadı");

                MesaiTalep mesaiTalep = new()
                {
                    PersonelId = personel.Id,
                    Aciklama = request.Aciklama,
                    BaslangicTarihi = request.BaslangicTarihi,
                    BitisTarihi = request.BitisTarihi,
                    ToplamSure = request.BitisTarihi - request.BaslangicTarihi,
                    MesaiDegerlendirmeDurum = MesaiDegerlendirmeDurumEnum.Beklemede,
                    OnaySurecId = onaySurec.Id,
                    TenantId = tenantId.Value,
                };

                await mesaiTalepRepository.AddAsync(mesaiTalep);

                Guid? ilkOnayciPersonelId = null;
                List<TalepDegerlendirme> talepDegerlendirmeler = new();

                if (onaySurec is not null)
                {
                    foreach (var onayAdimi in onaySurec.OnayAdimlari)
                    {
                        var talepDegerlendirme = new TalepDegerlendirme(
                         talepId: mesaiTalep.Id,
                         adimSirasi: onayAdimi.Sira,
                         onaySureciAdimiId: onayAdimi.Id,
                         talepTipi: OnaySurecTuruEnum.Mesai,
                         tenantId: tenantId.Value
                        );
                        var resolverResult = await onaylayiciResolverService.OnaylayiciGetirAsync(
                                onayAdimi, personelGorevlendirme, cancellationToken);

                        if (!resolverResult.IsSuccessful || !resolverResult.Data.HasValue)
                        {
                            await unitOfWork.RollbackTransactionAsync(transaction);
                            return Result<string>.Failure($"İlk onaycı belirlenemedi: {resolverResult.Data}");
                        }
                        
                        talepDegerlendirme.OnayciAta(ilkOnayciPersonelId, onayAdimi.RolId);
                        if (onayAdimi.Sira == 1)
                        {
                            ilkOnayciPersonelId = resolverResult.Data;
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
                        Baslik = "Yeni Mesai Talebi",
                        Aciklama = $"{personel.Ad} {personel.Soyad} tarafından yeni bir mesai talebi oluşturuldu.",
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
                return Result<string>.Succeed("Mesai tabeli oluşturuldu");

            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure("Mesai talebi oluşturulurken hata oluştu: " + ex.Message);
            }

        }
    }
}

