using MediatR;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Bildirimler;
public sealed record BildirimUpdateCommand(
    Guid BildirimId) : IRequest<Result<string>>;


internal sealed class BildirimUpdateCommandhandler(
    IPersonelBildirimRepository personelBildirimRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<BildirimUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(BildirimUpdateCommand request, CancellationToken cancellationToken)
    {
        PersonelBildirim personelBildirim = await personelBildirimRepository.FirstOrDefaultAsync(p => p.BildirimId == request.BildirimId);
        if (personelBildirim == null)
            return Result<string>.Failure("Bildirim bulunamadı");

        personelBildirim.OkunduMu = true;
        personelBildirim.OkunmaTarihi = DateTimeOffset.Now;

        //personelBildirimRepository.Update(personelBildirim);

        await unitOfWork.SaveChangesAsync();

        return Result<string>.Succeed("Bildirim okundu");
    }
}

