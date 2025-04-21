using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bordro;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.MaasPusulalar;
public sealed record MaasPusulaDegerlendirCommand(
    Guid Id,
    int DegerlendirmeDurumValue
    ) : IRequest<Result<string>>;

internal sealed class MaasPusulaDegerlendirCommandHandler(
    IMaasPusulaRepository maasPusulaRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<MaasPusulaDegerlendirCommand, Result<string>>
{
    public async Task<Result<string>> Handle(MaasPusulaDegerlendirCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if (!userId.HasValue)
            return Result<string>.Failure("Kullanıcı bulunamadı");

        var maasPusula = await maasPusulaRepository.Where(p => p.Id == request.Id && p.Personel!.UserId == userId).Include(p => p.Personel).FirstOrDefaultAsync();
        if (maasPusula is null)
            return Result<string>.Failure("Pusula bulunamadı");

        maasPusula.Durum = MaasPusulaDurumEnum.FromValue(request.DegerlendirmeDurumValue);
        maasPusulaRepository.Update(maasPusula);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed($"Pusula onay isteği başarıyla {MaasPusulaDurumEnum.FromValue(request.DegerlendirmeDurumValue)}");
    }
}
