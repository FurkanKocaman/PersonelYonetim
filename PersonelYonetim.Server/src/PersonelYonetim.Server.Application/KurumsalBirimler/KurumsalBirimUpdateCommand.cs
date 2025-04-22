using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.KurumsalBirimler;
public sealed record KurumsalBirimUpdateCommand(
    Guid Id,
    string Ad,
    string? Kod,
    Guid? UstBirimId
    ) : IRequest<Result<string>>;

internal sealed class KurumsalBirimUpdateCommandHandler(
    IKurumsalBirimRepository kurumsalBirimRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<KurumsalBirimUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(KurumsalBirimUpdateCommand request, CancellationToken cancellationToken)
    {
        var kurumsalBirim = await kurumsalBirimRepository.WhereWithTracking(p => p.Id == request.Id).FirstOrDefaultAsync();

        if (kurumsalBirim is null)
            return Result<string>.Failure("Birim bulunamamdı");

        kurumsalBirim.Ad = request.Ad;
        kurumsalBirim.Kod = request.Kod;
        kurumsalBirim.UstBirimId = request.UstBirimId;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Birim başarıyla güncellendi");
    }
}
