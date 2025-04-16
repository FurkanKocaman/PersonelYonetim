using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Personeller;
using TS.Result;

namespace PersonelYonetim.Server.Infrastructure.Services;
internal sealed class OnaylayiciResolverService(
    IPersonelRepository personelRepository,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository
    ) : IOnaylayiciResolverService
{
    public async Task<Result<Guid?>> OnaylayiciGetirAsync(OnaySureciAdimi adim, PersonelGorevlendirme requesterAssignment, CancellationToken cancellationToken)
    {
        try
        {
            switch (adim.OnaylayiciTanimTipi)
            {
                case var value when value == OnaylayiciTanimTipiEnum.BelirliKisi:
                    if (!adim.PersonelId.HasValue)
                    {
                        return Result<Guid?>.Failure("Onay adımında onaylayıcı personel belirtilmemiş.");
                    }
                    var targetPersonel = await personelRepository.FirstOrDefaultAsync(p => p.Id == adim.PersonelId.Value && p.IsActive && p.TenantId == adim.TenantId, cancellationToken);
                    if (targetPersonel != null)
                    {
                        return Result<Guid?>.Succeed(adim.PersonelId.Value);
                    }
                    else
                    {
                        return Result<Guid?>.Failure($"Atanan onaylayıcı ({adim.PersonelId.Value}) bulunamadı veya aktif değil.");
                    }

                case var value when value == OnaylayiciTanimTipiEnum.GenelRol:
                    if (!adim.RolId.HasValue)
                    {
                        return Result<Guid?>.Failure("Onay adımında onaylayıcı rol belirtilmemiş.");
                    }

                    var tenantId = requesterAssignment.TenantId;

                    var gorevlendirmeGenelRol = await personelGorevlendirmeRepository
                        .Where(pg => pg.TenantId == tenantId && pg.IsActive && pg.GorevlendirmeRolleri.Any(gr => gr.RolId == adim.RolId.Value))
                        .Include(pg => pg.Personel).ToListAsync(cancellationToken);
                    if(gorevlendirmeGenelRol.Count > 1)
                    {
                        //Birden fazla kişi bu role sahip
                    }
                    var secilenGorevlendirmeRol = gorevlendirmeGenelRol.First();
                    return Result<Guid?>.Succeed(secilenGorevlendirmeRol.PersonelId);

                case var value when value == OnaylayiciTanimTipiEnum.YapisalRol_TalepEdenBirimi:

                    if (!adim.RolId.HasValue)
                    {
                        return Result<Guid?>.Failure("Onay adımında onaylayıcı rol belirtilmemiş.");
                    }

                    var talepEdenBirimId = requesterAssignment.KurumsalBirimId;

                    var yoneticiGorevlendirme = await personelGorevlendirmeRepository
                            .Where(pg => pg.KurumsalBirimId == talepEdenBirimId && pg.IsActive && pg.TenantId == requesterAssignment.TenantId && pg.GorevlendirmeRolleri.Any(gr => gr.RolId == adim.RolId.Value))
                            .FirstOrDefaultAsync(cancellationToken);
                    if (yoneticiGorevlendirme == null)
                    {
                        return Result<Guid?>.Failure($"Talep edenin birim yöneticisi bulunamadı (Rol: {adim.Role?.Name ?? adim.RolId.ToString()}).");
                    }

                    return Result<Guid?>.Succeed(yoneticiGorevlendirme.PersonelId);

                case var value when value == OnaylayiciTanimTipiEnum.YapisalRol_HedefBirim:

                    if (!adim.RolId.HasValue)
                    {
                        return Result<Guid?>.Failure("Onay adımında yapısal rol belirtilmemiş.");
                    }

                    if (!adim.HedefKurumsalBirimId.HasValue)
                    {
                        return Result<Guid?>.Failure("Onay adımında hedef birim belirtilmemiş.");
                    }

                    var hedefBirimId = adim.HedefKurumsalBirimId.Value;

                    var hedefYoneticiGorevlendirme = await personelGorevlendirmeRepository
                        .Where(pg => pg.KurumsalBirimId == hedefBirimId &&
                                     pg.IsActive &&
                                     pg.TenantId == requesterAssignment.TenantId &&
                                     pg.GorevlendirmeRolleri.Any(gr => gr.RolId == adim.RolId.Value))
                        .FirstOrDefaultAsync(cancellationToken);

                    if (hedefYoneticiGorevlendirme == null)
                    {
                        return Result<Guid?>.Failure($"Hedef birim yöneticisi bulunamadı (Birim: {hedefBirimId}, Rol: {adim.Role?.Name ?? adim.RolId.ToString()}).");
                    }

                    return Result<Guid?>.Succeed(hedefYoneticiGorevlendirme.PersonelId);

                default:
                    return Result<Guid?>.Failure($"Geçersiz onaylayıcı tanım tipi: {adim.OnaylayiciTanimTipi.Name}");
            }
        }
        catch (Exception ex)
        {
            return Result<Guid?>.Failure($"Onaylayıcı çözümlenirken hata: {ex.Message}");
        }
    }
}
