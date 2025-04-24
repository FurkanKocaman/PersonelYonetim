using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bordro;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Bordro;
public sealed record BordroKazancEkleCommand(
     Guid MaasPusulaId,
     string KazancTuru ,
     string? Aciklama ,
     decimal Tutar,
     bool SGKMatrahinaDahil,
     bool GelirVergisiMatrahinaDahil ,
     bool DamgaVergisiMatrahinaDahil 
    ):IRequest<Result<string>>;

internal sealed class BordroKazancEkleCommandHandler(
    ICurrentUserService currentUserService,
    IMaasPusulaRepository maasPusulaRepository,
    IKazancBilesenRepository kazancBilesenRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<BordroKazancEkleCommand, Result<string>>
{
    public async Task<Result<string>> Handle(BordroKazancEkleCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant bulunamamdı");

        var maasPusula = await maasPusulaRepository.Where(p => p.Id == request.MaasPusulaId && p.TenantId == tenantId).FirstOrDefaultAsync();

        if (maasPusula is null)
            return Result<string>.Failure("Pusula bulunamadı");

        KazancBilesen kazancBilesen = new()
        {
            MaasPusulaId = request.MaasPusulaId,
            KazancTuru = request.KazancTuru,
            Aciklama = request.Aciklama,
            Tutar = request.Tutar,
            SGKMatrahinaDahil = request.SGKMatrahinaDahil,
            GelirVergisiMatrahinaDahil = request.SGKMatrahinaDahil,
            DamgaVergisiMatrahinaDahil = request.DamgaVergisiMatrahinaDahil,
            TenantId = tenantId.Value,
        };

        kazancBilesenRepository.Add(kazancBilesen);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Kazanç bileşeni oluşturuldu tekrardan bordro hesaplayınca uyulanacaktır.");
    }
}
