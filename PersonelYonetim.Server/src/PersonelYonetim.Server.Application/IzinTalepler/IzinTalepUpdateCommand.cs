using GenericRepository;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using PersonelYonetim.Server.Domain.IzinTalepler;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using System.Security.Claims;
using TS.Result;

namespace PersonelYonetim.Server.Application.IzinTalepler;

public sealed record IzinTalepUpdateCommand(
    Guid Id,
    DateTimeOffset BaslangicTarihi,
    DateTimeOffset BitisTarihi,
    int IzinTipiValue,
    string? Aciklama) : IRequest<Result<string>>;

internal sealed class IzinTalepUpdateCommandHandler(
    IIzinTalepRepository izinTalepRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<IzinTalepUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(IzinTalepUpdateCommand request, CancellationToken cancellationToken)
    {
        
        IzinTalep izinTalep = await izinTalepRepository.FirstOrDefaultAsync(p => p.Id == request.Id);
        if (izinTalep is null)
            return Result<string>.Failure("İzin talebi bulunamadı");

        izinTalep.BaslangicTarihi = request.BaslangicTarihi;
        izinTalep.BitisTarihi = request.BitisTarihi;
        izinTalep.IzinTipi = IzinTipiEnum.FromValue(request.IzinTipiValue);
        izinTalep.Aciklama = request.Aciklama;
        
        await unitOfWork.SaveChangesAsync();

        return Result<string>.Succeed("İzin talebi Güncellendi");
    }
}