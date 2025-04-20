using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.IzinTalepler;
public sealed record IzinTalepDegerlendirmeCommand(
    Guid Id,
    int DegerlendirmeDurum,
    string? Yorum
    ) : IRequest<Result<string>>;

internal sealed class IzinTalepDegerlendirmeCommandHandler(
    ICurrentUserService currentUserService,
    IIzinTalepRepository izinTalepRepository,
    ITalepDegerlendirmeRepository talepDegerlendirmeRepository,
    IBildirimService bildirimService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<IzinTalepDegerlendirmeCommand, Result<string>>
{
    public async Task<Result<string>> Handle(IzinTalepDegerlendirmeCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        Guid? tenantId = currentUserService.TenantId;

        if (!userId.HasValue || !tenantId.HasValue)
            return Result<string>.Failure("User bulunamamdı");

        var talepDegerlendirme = await talepDegerlendirmeRepository.Where(p => p.TalepId == request.Id && p.AtananOnayciPersonel!.UserId == userId).Include(p => p.AtananOnayciPersonel).FirstOrDefaultAsync();

        if (talepDegerlendirme is null)
            return Result<string>.Failure("Talep bulunamamdı");

        var res = talepDegerlendirme.DurumuGuncelle(DegerlendirmeDurumEnum.FromValue(request.DegerlendirmeDurum), talepDegerlendirme.AtananOnayciPersonelId, request.Yorum, DateTimeOffset.Now);

        if(!res.IsSuccessful)
            return Result<string>.Failure(res.ErrorMessages![0]);

        talepDegerlendirmeRepository.Update(talepDegerlendirme);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        var izinTalep = await izinTalepRepository.Where(p => p.Id == talepDegerlendirme.TalepId).FirstOrDefaultAsync();
        if (izinTalep is null)
            return Result<string>.Failure("izin Talep bulunammadı");

        var result = izinTalep.GuncelDegerlendirmeDurumu();

        if(result != DegerlendirmeDurumEnum.Beklemede)
        {
            Bildirim bildirim = new()
            {
                Baslik = $"İzin talebi {result.Name}",
                Aciklama = $"{talepDegerlendirme.AtananOnayciPersonel!.FullName} tarafından izin talebiniz {result.Name}",
                CreatedAt = DateTimeOffset.Now,
                BildirimTipi = BildirimTipiEnum.Onay,
                AliciTipi = AliciTipiEnum.Personel,
                AliciId = izinTalep.PersonelId,
                TenantId = tenantId.Value,
            };

            await bildirimService.KullaniciyaBildirimGonderAsync(bildirim, izinTalep.PersonelId);
        }

        return Result<string>.Succeed("İzin talebi başarıyla değerlendirildi");

    }
}
