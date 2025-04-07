using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Users;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

namespace PersonelYonetim.Server.Application.Pozisyonlar;

public sealed record PozisyonGetAllQuery(
    Guid? SirketId) :IRequest<IQueryable<PozisyonGetAllQueryResponse>> ;

public sealed class PozisyonGetAllQueryResponse : EntityDto
{
    public string Ad { get; set; } = default!;
    public string? Aciklama { get; set; }
    public Guid SirketId { get; set; } = default!;
    public string SirketAd { get; set; } = default!;
}

internal sealed class PozisyonGetAllQueryHandler(
    IPozisyonRepository pozisyonRepository,
    IHttpContextAccessor httpContextAccessor,
    IPersonelRepository personelRepository,
    IPersonelAtamaRepository personelAtamaRepository,
    UserManager<AppUser> userManager) : IRequestHandler<PozisyonGetAllQuery, IQueryable<PozisyonGetAllQueryResponse>>
{
    public Task<IQueryable<PozisyonGetAllQueryResponse>> Handle(PozisyonGetAllQuery request, CancellationToken cancellationToken)
    {
        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString))
        {
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
        }

        var personel = personelRepository.GetAll()
            .Where(p => p.UserId == Guid.Parse(userIdString) && !p.IsDeleted)
            .Select(p => new { p.Id })
            .FirstOrDefault();

        if (personel == null)
        {
            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");
        }

        var pozisyonlar = pozisyonRepository.GetAll().Where(p => !p.IsDeleted);
        if(request.SirketId is not null)
        {
            pozisyonlar = pozisyonlar.Where(p => p.SirketId == request.SirketId);
        }
        var response = pozisyonlar
            .Join(personelAtamaRepository.GetAll(),
                    pozisyon => pozisyon.SirketId,
                    personelAtama => personelAtama.SirketId,
                    (pozisyon, personelAtama) => new { pozisyon, personelAtama })
            .Where(pp => pp.personelAtama.PersonelId == personel.Id && pp.personelAtama.IsDeleted == false && pp.personelAtama.IsActive)
            .Join(userManager.Users,
                  dp => dp.pozisyon.CreateUserId,
                  createUser => createUser.Id,
                  (dp, createUser) => new { dp.pozisyon, createUser })
            .GroupJoin(userManager.Users,
                  dp => dp.pozisyon.UpdateUserId,
                  updateUser => updateUser.Id,
                  (dp, updateUsers) => new { dp.pozisyon, dp.createUser, updateUsers })
            .SelectMany(
                    ppu => ppu.updateUsers.DefaultIfEmpty(),
                    (ppu, updateUser) => new PozisyonGetAllQueryResponse
                    {
                        Id = ppu.pozisyon.Id,
                        Ad = ppu.pozisyon.Ad,
                        Aciklama = ppu.pozisyon.Aciklama,
                        SirketId = ppu.pozisyon.SirketId,
                        SirketAd = ppu.pozisyon.Sirket.Ad,
                        IsActive = ppu.pozisyon.IsActive,
                        CreatedAt = ppu.pozisyon.CreatedAt,
                        CreateUserId = ppu.createUser.Id,
                        CreateUserName = ppu.createUser.FirstName + " " + ppu.createUser.LastName + " (" + ppu.createUser.Email + ")",
                        UpdateAt = ppu.pozisyon.UpdateAt,
                        UpdateUserId = updateUser != null ? updateUser.Id : null,
                        UpdateUserName = updateUser != null
                          ? updateUser.FirstName + " " + updateUser.LastName + " (" + updateUser.Email + ")"
                          : null,
                        IsDeleted = ppu.pozisyon.IsDeleted,
                        DeleteAt = ppu.pozisyon.DeleteAt
                    });
        return Task.FromResult(response);
    }
}
