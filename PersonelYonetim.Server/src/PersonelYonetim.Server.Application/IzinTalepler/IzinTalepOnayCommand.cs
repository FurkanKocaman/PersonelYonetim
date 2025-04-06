
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.UnitOfWork;
using System.Security.Claims;
using TS.Result;

namespace PersonelYonetim.Server.Application.IzinTalepler;

public sealed record IzinTalepOnayCommand(
    Guid Id,
    int OnayDurum) : IRequest<Result<string>> ;

internal sealed class IzinTalepOnayCommandHandler(
    IIzinTalepRepository izinTalepRepository,
    IHttpContextAccessor httpContextAccessor,
    IPersonelRepository personelRepository,
    IBildirimService bildirimService,
    IUnitOfWork unitOfWork) : IRequestHandler<IzinTalepOnayCommand, Result<string>>
{
    public async Task<Result<string>> Handle(IzinTalepOnayCommand request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }

        var personel = personelRepository.GetAll()
            .Where(p => p.UserId == Guid.Parse(userIdString))
            .Select(p => new { p.Id, p.FullName })
            .FirstOrDefault();

        if (personel is null)
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
        IzinTalep izinTalep = await izinTalepRepository.FirstOrDefaultAsync(p => p.Id == request.Id);

        izinTalep.DegerlendirmeDurumu = DegerlendirmeDurumEnum.FromValue(request.OnayDurum);
        izinTalep.DegerlendirilmeTarihi = DateTimeOffset.Now;
        izinTalep.DegerlendirenId = personel.Id; 
        await unitOfWork.SaveChangesAsync();


        Bildirim bildirim = new()
        {
            Baslik = "İzin talebi değerlendirildi",
            Aciklama = $"{personel.FullName} tarafından, {izinTalep.ToplamSure} günlük izin talebiniz {DegerlendirmeDurumEnum.FromValue(request.OnayDurum)}.",
            CreatedAt = DateTimeOffset.Now,
            BildirimTipi = BildirimTipiEnum.Bilgilendirme,
            AliciTipi = AliciTipiEnum.Personel,
            AliciId = izinTalep.PersonelId,
        };

        await bildirimService.KullaniciyaBildirimGonderAsync(bildirim, izinTalep.PersonelId);



        if (request.OnayDurum == 0)
            return Result<string>.Succeed("İzin talebi onaylandı.");
        if (request.OnayDurum == 1)
            return Result<string>.Succeed("İzin talebi reddedildi");
        return Result<string>.Succeed("İzin talebi onay durumu değerlendirildi");
    }
}
