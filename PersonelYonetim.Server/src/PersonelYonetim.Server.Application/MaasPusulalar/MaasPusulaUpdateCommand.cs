using Mapster;
using MediatR;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bordro;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.MaasPusulalar;
public sealed record MaasPusulaUpdateCommand(
    Guid Id,
    string? TabiKanunKodu,
    string? TesvikKodu,
    string? SGKDurumu,

    decimal? BrutUcret,
    decimal? EkKazancToplam,
    decimal? ToplamBrutKazanc,

    decimal? SGKMatrahi,
    decimal? GelirVergisiMatrahi,
    decimal? KumulatifGelirVergisiMatrahiOnceki,
    decimal? KumulatifGelirVergisiMatrahiDonemSonu,
    decimal? HesaplananGelirVergisi,
    decimal? GelirVergisiIstisnasiUygulanan,
    decimal? HesaplananDamgaVergisi,
    decimal? DamgaVergisiIstisnasiUygulanan,

    decimal? SGKPrimiIsci,
    decimal? IssizlikPrimiIsci,
    decimal? OdenecekGelirVergisi,
    decimal? OdenecekDamgaVergisi,
    decimal? DigerKesintilerToplam,
    decimal? ToplamKesinti,
    decimal? NetMaas,

    decimal? SGKPrimiIsveren,
    decimal? IssizlikPrimiIsveren,
    decimal? ToplamIsverenMaliyeti,
    int? SGKGunSayisi,
    bool? BesKesintisiVarMi,
    decimal? BesKesintiTutari
    ) : IRequest<Result<string>>;

internal sealed class MaasPusulaUpdateCommandHandler(
    IMaasPusulaRepository maasPusulaRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
) : IRequestHandler<MaasPusulaUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(MaasPusulaUpdateCommand request, CancellationToken cancellationToken)
    {
        var tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant bulunamadı");

        var existing = await maasPusulaRepository.FirstOrDefaultAsync(
            p => p.Id == request.Id && p.TenantId == tenantId,
            cancellationToken
        );

        if (existing is null)
            return Result<string>.Failure("Maaş pusulası bulunamadı");

        request.Adapt(existing);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Maaş pusulası başarıyla güncellendi");
    }
}