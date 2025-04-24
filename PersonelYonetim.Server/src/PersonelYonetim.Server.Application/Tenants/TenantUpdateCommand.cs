using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Tenants;
using PersonelYonetim.Server.Domain.UnitOfWork;
using TS.Result;

namespace PersonelYonetim.Server.Application.Tenants;
public sealed record TenantUpdateCommand(
    Guid Id,
    string Name,
    string DisplayName ,
    string? LogoUrl ,

    string? SGKIsyeri ,
    string? SGKNumarasi ,
    string? VergiDairesiAdi ,
    string? VergiNumarasi,
    string? TabiOlduguKanun,

    string? Address ,
    string? City ,
    string? PostalCode,
    string? Phone ,
    string? Email,

    //Bordro için gerekli değişkenler
    decimal AsgariUcret,
    decimal SGKPrimIsciKesintiOrani ,
    decimal SGKIssizlikPrimIsciKesintiOrani ,
    decimal SGKPrimIsverenKesintiOrani ,
    decimal SGKIssizlikPrimIsverenKesintiOrani,
    decimal DamgaVergisiOrani,

    decimal FazlaMesaiKatsayi,
    decimal HaftasonuFazlaMesaiKatsayi ,
    decimal ResmiTatilFazlaMesaiKatsayi 
    ) : IRequest<Result<string>>;

internal sealed class TenantUpdateCommandHandler(
    ITenantRepository tenantRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<TenantUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(TenantUpdateCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant bulunamadı");

        if (request.Id != tenantId)
            return Result<string>.Failure("Hata oluştu");

        var tenant = await tenantRepository.WhereWithTracking(p  => p.Id == tenantId).FirstOrDefaultAsync();

        request.Adapt(tenant);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Başarıyla güncellendi");
    }
}
