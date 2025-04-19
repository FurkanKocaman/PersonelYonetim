using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Personeller;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.IzinTurler;
public sealed record IzinTurGetAllQuery () : IRequest<IQueryable<IzinTurGetAllQueryResponse>>;

public sealed class IzinTurGetAllQueryResponse
{
    public Guid Id { get; set; }
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public bool UcretliMi { get; set; }
    public string LimitTipi { get; set; } = default!;
    public int KalanGunSayisi { get; set; } = 0;
}

internal sealed class IzinTurGetAllQueryHandler(
    IIzinTurRepository izinTurRepository,
    IPersonelRepository personelRepository,
    ICurrentUserService currentUserService) : IRequestHandler<IzinTurGetAllQuery, IQueryable<IzinTurGetAllQueryResponse>>
{
    public Task<IQueryable<IzinTurGetAllQueryResponse>> Handle(IzinTurGetAllQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        Guid? tenantId = currentUserService.TenantId;
        if (!userId.HasValue || !tenantId.HasValue)
            throw new UnauthorizedAccessException("User bilgisi bulunamadı.");

        var personel = personelRepository.GetAll()
            .Where(p => p.UserId == userId && p.TenantId == tenantId && !p.IsDeleted)
            .Include(p => p.PersonelGorevlendirmeler).ThenInclude(p => p.GorevlendirmeIzinKurali)
            .Select(p => new { p.Id, p.PersonelGorevlendirmeler })
            .FirstOrDefault();

        if (personel is null)
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");

        var personelGorevlendirme = personel.PersonelGorevlendirmeler.Where(p => p.TenantId == tenantId && p.IsDeleted == false && p.IsActive).FirstOrDefault();
        if(personelGorevlendirme is null)
            throw new UnauthorizedAccessException("Personel görevlendirme bilgisi bulunamadı.");

        var izinKural = personelGorevlendirme.GorevlendirmeIzinKurali;
        if(izinKural is null)
            throw new UnauthorizedAccessException("İzin kuralı bilgisi bulunamadı.");

        var response = izinTurRepository.GetAll()
                .Where(p => !p.IsDeleted && p.IzinKuralId == izinKural.IzinKuralId)
                .Select(ip => new IzinTurGetAllQueryResponse
                {
                    Id = ip.Id,
                    Ad = ip.Ad,
                    Aciklama = ip.Aciklama,
                    UcretliMi = ip.UcretliMi,
                    LimitTipi = ip.LimitTipi == LimitTipiEnum.Limitsiz ? ip.LimitTipi.Name : ip.LimitTipi.Name + " " + ip.LimitGunSayisi +" gün",
                });

        return Task.FromResult(response);
    }
}
