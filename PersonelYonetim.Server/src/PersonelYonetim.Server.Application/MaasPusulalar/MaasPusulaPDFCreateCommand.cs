using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Bordro;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Tenants;
using TS.Result;

namespace PersonelYonetim.Server.Application.MaasPusulalar;
public sealed record MaasPusulaPDFCreateCommand(
    Guid Id
    ) : IRequest<Result<string>>;

internal sealed class MaasPusulaPDFCreateCommandHandler(
    ICurrentUserService currentUserService,
    IMaasPusulaRepository maasPusulaRepository,
    IPersonelGorevlendirmeRepository personelGorevlendirmeRepository,
    ITenantRepository tenantRepository,
    IPDFService PDFService
    ) : IRequestHandler<MaasPusulaPDFCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(MaasPusulaPDFCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<string>.Failure("İşlem başarısız oldu");

        var maasPusula = await maasPusulaRepository.Where(p => p.Id == request.Id && p.TenantId == tenantId && !p.IsDeleted).Include(p => p.Personel).FirstOrDefaultAsync();

        if (maasPusula is null)
            return Result<string>.Failure("Pusula bulunamamdı");

        var personelGorevlendirme = await personelGorevlendirmeRepository.Where(p => p.PersonelId == maasPusula.PersonelId && p.TenantId == tenantId && !p.IsDeleted && p.IsActive).Include(p => p.Personel).ThenInclude(p => p.PersonelDetay).FirstOrDefaultAsync();

        if (personelGorevlendirme is null)
            return Result<string>.Failure("Personel bilgileri bulunamamdı");

        var personelDetay = personelGorevlendirme.Personel.PersonelDetay;

        if (personelDetay is null)
            return Result<string>.Failure("Personel bilgileri bulunamamdı");

        var tenant = await tenantRepository.FirstOrDefaultAsync(p => p.Id == tenantId);

        if (personelGorevlendirme is null)
            return Result<string>.Failure("Şirket bilgileri bulunamamdı");



        var pdfBytes = await PDFService.CreateBordroPdf(personelGorevlendirme,personelDetay,tenant,maasPusula);

        var fileName = $"{personelGorevlendirme.PersonelId}_Bordro_{maasPusula.Yil}-{maasPusula.Ay}.pdf";

        var filePath = Path.Combine("wwwroot", "pdf");



        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        filePath = Path.Combine(filePath, fileName);

        if (File.Exists(filePath))
            File.Delete(filePath);

        await File.WriteAllBytesAsync(filePath, pdfBytes, cancellationToken);

        var downloadUrl = $"/pdf/{fileName}";
        return Result<string>.Succeed(downloadUrl);
    }
}
