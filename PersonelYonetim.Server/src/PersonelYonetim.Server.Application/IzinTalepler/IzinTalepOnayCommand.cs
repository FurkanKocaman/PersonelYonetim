
using MediatR;
using Microsoft.AspNetCore.Http;
using PersonelYonetim.Server.Domain.Izinler;
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
            .Select(p => new { p.Id })
            .FirstOrDefault();

        if (personel is null)
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
        IzinTalep izinTalep = await izinTalepRepository.FirstOrDefaultAsync(p => p.Id == request.Id);

        izinTalep.DegerlendirmeDurumu = DegerlendirmeDurumEnum.FromValue(request.OnayDurum);
        izinTalep.DegerlendirilmeTarihi = DateTimeOffset.Now;
        izinTalep.DegerlendirenId = personel.Id;
        await unitOfWork.SaveChangesAsync();
        if(request.OnayDurum == 0)
            return Result<string>.Succeed("İzin talebi onaylandı.");
        if (request.OnayDurum == 1)
            return Result<string>.Succeed("İzin talebi reddedildi");
        return Result<string>.Succeed("İzin talebi onay durumu değerlendirildi");
    }
}
