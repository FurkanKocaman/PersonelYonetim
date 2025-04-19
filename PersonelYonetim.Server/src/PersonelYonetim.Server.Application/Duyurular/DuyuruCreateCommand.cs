using MediatR;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.Duyurular;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Duyurular;
public sealed record DuyuruCreateCommand(
    string Baslik,
    string? Aciklama,
    Guid? tenantId,
    int AliciTipiValue,
    Guid? AliciId,
    List<Guid>? AliciIdler) : IRequest<Result<string>>;


internal sealed class DuyuruCreateCommandHandler(
    IDuyuruRepository duyuruRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork) : IRequestHandler<DuyuruCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DuyuruCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if(!tenantId.HasValue)
            return Result<string>.Failure("Tenant bilgisi bulunamadı");

        Duyuru duyuru = new()
        {
            Baslik = request.Baslik,
            Aciklama = request.Aciklama,
            TenantId = tenantId.Value,
            AliciTipi = AliciTipiEnum.FromValue(request.AliciTipiValue),
            AliciId = request.AliciId,
            AliciIdler = request.AliciIdler
        };

        duyuruRepository.Add(duyuru);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Duyuru başarıyla oluşturuldu");
    }
}
