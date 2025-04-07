using MediatR;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.Duyurular;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Duyurular;
public sealed record DuyuruCreateCommand(
    string Baslik,
    string? Aciklama,
    Guid SirketId,
    int AliciTipiValue,
    Guid? AliciId,
    List<Guid>? AliciIdler) : IRequest<Result<string>>;


internal sealed class DuyuruCreateCommandHandler(
    IDuyuruRepository duyuruRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DuyuruCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DuyuruCreateCommand request, CancellationToken cancellationToken)
    {
        Duyuru duyuru = new()
        {
            Baslik = request.Baslik,
            Aciklama = request.Aciklama,
            SirketId = request.SirketId,
            AliciTipi = AliciTipiEnum.FromValue(request.AliciTipiValue),
            AliciId = request.AliciId,
            AliciIdler = request.AliciIdler
        };

        duyuruRepository.Add(duyuru);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Duyuru başarıyla oluşturuldu");
    }
}
