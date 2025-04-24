using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Tenants;
using PersonelYonetim.Server.Domain.Users;

namespace PersonelYonetim.Server.Application.Tenants;
public sealed record TenantGetQuery(
    ) : IRequest<IQueryable<TenantGetQueryResponse>>;

public sealed class TenantGetQueryResponse : EntityDto
{
    // Temel Bilgiler
    public string Name { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string? LogoUrl { get; set; }

    public string? SGKIsyeri { get; set; }
    public string? SGKNumarasi { get; set; }
    public string? VergiDairesiAdi { get; set; }
    public string? VergiNumarasi { get; set; }
    public string? TabiOlduguKanun { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }

    //Bordro için gerekli değişkenler
    public decimal AsgariUcret { get; set; }
    public decimal SGKPrimIsciKesintiOrani { get; set; }
    public decimal SGKIssizlikPrimIsciKesintiOrani { get; set; }
    public decimal SGKPrimIsverenKesintiOrani { get; set; }
    public decimal SGKIssizlikPrimIsverenKesintiOrani { get; set; }
    public decimal DamgaVergisiOrani { get; set; }

    public decimal FazlaMesaiKatsayi { get; set; }
    public decimal HaftasonuFazlaMesaiKatsayi { get; set; }
    public decimal ResmiTatilFazlaMesaiKatsayi { get; set; }
}

internal sealed class TenantGetQueryHandler(
    ITenantRepository tenantRepository,
    ICurrentUserService currentUserService,
    UserManager<AppUser> userManager
    ) : IRequestHandler<TenantGetQuery, IQueryable<TenantGetQueryResponse>>
{
    public Task<IQueryable<TenantGetQueryResponse>> Handle(TenantGetQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if(!tenantId.HasValue)
            return Task.FromResult( Enumerable.Empty<TenantGetQueryResponse>().AsQueryable());

        var tenant = tenantRepository.Where(p => p.Id == tenantId);

        if(tenant is null)
            return Task.FromResult(Enumerable.Empty<TenantGetQueryResponse>().AsQueryable());

        var response = tenant
            .GroupJoin(userManager.Users,
                tenant => tenant.CreateUserId,
                createUser => createUser.Id,
                (tenant, createUsers) => new {tenant, createUsers})
            .SelectMany(
                tu => tu.createUsers.DefaultIfEmpty(),
                (tu, createUser) => new { tu.tenant, createUser})
            .GroupJoin(userManager.Users,
                tu => tu.tenant.UpdateUserId,
                updateUser => updateUser.Id,
                (tu, updateUsers) => new { tu.tenant,tu.createUser, updateUsers})
            .SelectMany(
                tu => tu.updateUsers.DefaultIfEmpty(),
                (tu, updateUser) => new TenantGetQueryResponse
                {
                    Id = tu.tenant.Id,
                    Name = tu.tenant.Name,
                    DisplayName = tu.tenant.DisplayName,
                    LogoUrl = tu.tenant.LogoUrl,

                    SGKIsyeri = tu.tenant.SGKIsyeri,
                    SGKNumarasi = tu.tenant.SGKNumarasi,
                    VergiDairesiAdi = tu.tenant.VergiDairesiAdi,
                    VergiNumarasi = tu.tenant.VergiNumarasi,
                    TabiOlduguKanun = tu.tenant.TabiOlduguKanun,
                    Address = tu.tenant.Address,
                    City = tu.tenant.City,
                    PostalCode = tu.tenant.PostalCode,
                    Phone = tu.tenant.PostalCode,
                    Email = tu.tenant.Email,

                    AsgariUcret = tu.tenant.AsgariUcret,
                    SGKPrimIsciKesintiOrani = tu.tenant.SGKPrimIsciKesintiOrani,
                    SGKIssizlikPrimIsciKesintiOrani = tu.tenant.SGKIssizlikPrimIsciKesintiOrani,
                    SGKPrimIsverenKesintiOrani = tu.tenant.SGKPrimIsverenKesintiOrani,
                    SGKIssizlikPrimIsverenKesintiOrani = tu.tenant.SGKIssizlikPrimIsverenKesintiOrani,
                    DamgaVergisiOrani = tu.tenant.DamgaVergisiOrani,

                    FazlaMesaiKatsayi = tu.tenant.FazlaMesaiKatsayi,
                    HaftasonuFazlaMesaiKatsayi = tu.tenant.HaftasonuFazlaMesaiKatsayi,
                    ResmiTatilFazlaMesaiKatsayi = tu.tenant.ResmiTatilFazlaMesaiKatsayi,
                    
                    IsActive = tu.tenant.IsActive,
                    CreatedAt = tu.tenant.CreatedAt,
                    CreateUserId = tu.createUser != null ? tu.createUser.Id : null,
                    CreateUserName = tu.createUser != null ? tu.createUser.FirstName + " " + tu.createUser.LastName + " (" + tu.createUser.Email + ")" : null,
                    UpdateAt = tu.tenant.UpdateAt,
                    UpdateUserId = updateUser != null ? updateUser.Id : null,
                    UpdateUserName = updateUser != null ? updateUser.FirstName + " " + updateUser.LastName + " (" + updateUser.Email + ")" : null,
                    IsDeleted = tu.tenant.IsDeleted,
                    DeleteAt  = tu.tenant.DeleteAt,
                }).AsQueryable();

        return Task.FromResult( response );
    }
}
