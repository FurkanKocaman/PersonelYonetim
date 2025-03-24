using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Http;
using PersonelYonetim.Server.Domain.Izinler;
using System.Security.Claims;
using TS.Result;

namespace PersonelYonetim.Server.Application.IzinTalepler;

public sealed record IzinTalepOnayCommand(
    Guid Id,
    int OnayDurum) : IRequest<Result<string>> ;

internal sealed class IzinTalepOnayCommandHandler(
    IIzinTalepRepository izinTalepRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<IzinTalepOnayCommand, Result<string>>
{
    public async Task<Result<string>> Handle(IzinTalepOnayCommand request, CancellationToken cancellationToken)
    {
        HttpContextAccessor httpContextAccessor = new();
        string? userIdString = httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userIdString == null)
            return Result<string>.Failure("Kullanıcı bulunamadı");

        var izinTalep = await izinTalepRepository.FirstOrDefaultAsync(p => p.Id == request.Id);
        if (izinTalep is null)
            return Result<string>.Failure("İzin talebi bulunamadı");

        izinTalep.DegerlendirmeDurumu = DegerlendirmeDurumEnum.FromValue(request.OnayDurum);
        izinTalep.DegerlendirilmeTarihi = DateTimeOffset.Now;
        izinTalep.DegerlendirenId = Guid.Parse(userIdString);
        await unitOfWork.SaveChangesAsync();
        if(request.OnayDurum == 0)
            return Result<string>.Succeed("İzin talebi onaylandı.");
        if (request.OnayDurum == 1)
            return Result<string>.Succeed("İzin talebi reddedildi");
        return Result<string>.Succeed("İzin talebi onay durumu değerlendirildi");
    }
}
