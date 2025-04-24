using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bordro;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Bordro;
public sealed record BordroKesintiEkleCommand(
     Guid MaasPusulaId,
     string KesintiTuru,
     string? Aciklama,
     decimal Tutar,
     bool SGKMatrahinaDahil,
     bool GelirVergisiMatrahinaDahil,
     bool DamgaVergisiMatrahinaDahil
    ):IRequest<Result<string>>;

internal sealed class BordroKesintiEkleCommandHandler(
    ICurrentUserService currentUserService,
    IMaasPusulaRepository maasPusulaRepository,
    IKesintiBilesenRepository kesintiBilesenRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<BordroKesintiEkleCommand, Result<string>>
{
    public async Task<Result<string>> Handle(BordroKesintiEkleCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant bulunamamdı");

        var maasPusula = await maasPusulaRepository.Where(p => p.Id == request.MaasPusulaId && p.TenantId == tenantId).FirstOrDefaultAsync();

        if (maasPusula is null)
            return Result<string>.Failure("Pusula bulunamadı");

        KesintiBilesen kesintiBilesen = new()
        {
            MaasPusulaId = request.MaasPusulaId,
            KesintiTuru = request.KesintiTuru,
            Aciklama = request.Aciklama,
            Tutar = request.Tutar,
            SGKMatrahinaDahil = request.SGKMatrahinaDahil,
            GelirVergisiMatrahinaDahil = request.SGKMatrahinaDahil,
            DamgaVergisiMatrahinaDahil = request.DamgaVergisiMatrahinaDahil,
            TenantId = tenantId.Value,
        };

        kesintiBilesenRepository.Add(kesintiBilesen);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Kesinti bileşeni oluşturuldu tekrardan bordro hesaplayınca uyulanacaktır.");
    }
}
