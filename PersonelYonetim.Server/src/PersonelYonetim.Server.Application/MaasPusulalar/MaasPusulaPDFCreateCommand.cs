using MediatR;
using Microsoft.AspNetCore.Hosting;
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

public sealed class MaasPusulaPDFDto
{
    public string? Tarih { get; set; }
    public string? FullName { get; set; }
    public string? TCKN { get; set; }
    public string? IsBaslangicTarihi { get; set; }
    public string? IsBitisTarihi { get; set; }
    public string? UcretTipi { get; set; }
    public int BrutUcret { get; set; }
}

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
        Guid? userId = currentUserService.UserId;
        Guid? tenantId = currentUserService.TenantId;

        if (!userId.HasValue | !tenantId.HasValue)
            return Result<string>.Failure("İşlem başarısız oldu");

        var personelGorevlendirme = await personelGorevlendirmeRepository.Where(p => p.Personel.UserId == userId && p.TenantId == tenantId && !p.IsDeleted && p.IsActive).Include(p => p.Personel).ThenInclude(p => p.PersonelDetay).FirstOrDefaultAsync();

        if (personelGorevlendirme is null)
            return Result<string>.Failure("Personel bilgileri bulunamamdı");

        var personelDetay = personelGorevlendirme.Personel.PersonelDetay;

        if (personelDetay is null)
            return Result<string>.Failure("Personel bilgileri bulunamamdı");

        var tenant = await tenantRepository.FirstOrDefaultAsync(p => p.Id == tenantId);

        if (personelGorevlendirme is null)
            return Result<string>.Failure("Şirket bilgileri bulunamamdı");

        var maasPusula = await maasPusulaRepository.Where(p => p.Id == request.Id && p.Personel!.UserId == userId).Include(p => p.Personel).FirstOrDefaultAsync();

        if (maasPusula is null)
            return Result<string>.Failure("Pusula bulunamamdı");

        var pdfBytes = await PDFService.CreateBordroPdf(personelGorevlendirme,personelDetay,tenant,maasPusula);

        var fileName = $"{personelGorevlendirme.Personel.FullName}_Bordro_{DateTime.Now.Ticks}.pdf";

        var filePath = Path.Combine("wwwroot", "pdf");

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        filePath = Path.Combine(filePath, fileName);
        await File.WriteAllBytesAsync(filePath, pdfBytes, cancellationToken);

        var downloadUrl = $"/pdf/{fileName}";
        return Result<string>.Succeed(downloadUrl);
    }
}
