using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.Users;

namespace PersonelYonetim.Server.Application.IzinKurallar;

public sealed record IzinKuralGetAllQuery() : IRequest<IQueryable<IzinKuralGetAllResponse>>;

public sealed class IzinKuralGetAllResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public int PersonelCount { get; set; }
    public List<IzinTurDto> IzinTurler { get; set; } = new List<IzinTurDto>();
}
public sealed class IzinTurDto
{
    public Guid Id { get; set; }
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public bool UcretliMi { get; set; } = false;
    public string LimitTipiName { get; set; } = default!;
    public int LimitGunSayisi { get; set; }
}

internal sealed class IzinKuralGetAllQueryHandler(
    IIzinKuralRepository izinKuralRepository,
    //IGorevlendirmeIzinKuraliRepository gorevlendirmeIzinKuraliRepository,
    UserManager<AppUser> userManager,
    ICurrentUserService currentUserService
    ) : IRequestHandler<IzinKuralGetAllQuery, IQueryable<IzinKuralGetAllResponse>>
{
    public Task<IQueryable<IzinKuralGetAllResponse>> Handle(IzinKuralGetAllQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        Guid? tenantId = currentUserService.TenantId;

        if (!userId.HasValue || !tenantId.HasValue)
            return Task.FromResult(Enumerable.Empty<IzinKuralGetAllResponse>().AsQueryable());

        var izinKurallar = izinKuralRepository.GetAll()
            .Where(p => !p.IsDeleted && p.TenantId == tenantId.Value)
            .Include(p => p.IzinTurler)
            .Include(p => p.GorevlendirmeIzinKurallar);

        var x = izinKurallar.ToList();

        var response = izinKurallar
                .GroupJoin(userManager.Users,
                    izinKural => izinKural.CreateUserId,
                    createUser => createUser.Id,
                    (izinKural, createUsers) => new { izinKural, createUsers })
                .SelectMany(
                    iu => iu.createUsers.DefaultIfEmpty(),
                    (iu, createUser) => new { iu.izinKural, createUser })
                .GroupJoin(userManager.Users,
                      ip => ip.izinKural.UpdateUserId,
                      updateUser => updateUser.Id,
                      (ip, updateUsers) => new { ip.izinKural, ip.createUser, updateUsers })
                .SelectMany(
                        iuu => iuu.updateUsers.DefaultIfEmpty(),
                        (iuu, updateUser) => new IzinKuralGetAllResponse
                        {
                            Id = iuu.izinKural.Id,
                            Ad = iuu.izinKural.Ad,
                            Aciklama = iuu.izinKural.Aciklama,
                            PersonelCount = iuu.izinKural.GorevlendirmeIzinKurallar.Where(p => !p.IsDeleted).Count(),
                            IsActive = iuu.izinKural.IsActive,
                            CreatedAt = iuu.izinKural.CreatedAt,
                            CreateUserId = iuu.createUser == null ? null : iuu.createUser.Id,
                            CreateUserName = iuu.createUser == null
                                    ? null
                                    : iuu.createUser.FirstName + " " + iuu.createUser.LastName + " (" + iuu.createUser.Email + ")",
                            UpdateAt = iuu.izinKural.UpdateAt,
                            UpdateUserId = updateUser != null ? updateUser.Id : null,
                            UpdateUserName = updateUser != null
                          ? updateUser.FirstName + " " + updateUser.LastName + " (" + updateUser.Email + ")"
                          : null,
                            IsDeleted = iuu.izinKural.IsDeleted,
                            DeleteAt = iuu.izinKural.DeleteAt,
                            IzinTurler = iuu.izinKural.IzinTurler.Select(it => new IzinTurDto
                            {
                                Id = it.Id,
                                Ad = it.Ad,
                                Aciklama = it.Aciklama,
                                UcretliMi = it.UcretliMi,
                                LimitTipiName = it.LimitTipi == LimitTipiEnum.Limitsiz ? LimitTipiEnum.Limitsiz.Name : $"{it.LimitTipi.Name} {it.LimitGunSayisi} gün",
                                LimitGunSayisi = it.LimitGunSayisi

                            }).ToList(),
                        });
        return Task.FromResult(response);
    }
}
