﻿//using MediatR;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using PersonelYonetim.Server.Domain.Abstractions;
//using PersonelYonetim.Server.Domain.Izinler;
//using PersonelYonetim.Server.Domain.PersonelAtamalar;
//using PersonelYonetim.Server.Domain.Personeller;
//using PersonelYonetim.Server.Domain.Users;
//using System.Security.Claims;

//namespace PersonelYonetim.Server.Application.IzinKurallar;

//public sealed record IzinKuralGetAllQuery() : IRequest<IQueryable<IzinKuralGetAllResponse>>;

//public sealed class IzinKuralGetAllResponse : EntityDto
//{
//    public string Ad { get; set; } = default!;
//    public string? Aciklama { get; set; }
//    public Guid SirketId { get; set; } = default;
//    public string SirketAd { get; set; } = default!;
//    public IEnumerable<IzinTurResponse> IzinTurler { get; set; } = new List<IzinTurResponse>();
//}
//public sealed class IzinTurResponse
//{
//    public Guid Id { get; set; }
//    public string Ad { get; set; } = default!;
//    public string? Aciklama { get; set; }
//    public bool UcretliMi { get; set; } = false;
//    public string LimitTipiName { get; set; } = default!;
//    public int LimitGunSayisi { get; set; }
//}

//internal sealed class IzinKuralGetAllQueryHandler(
//    IIzinKuralRepository izinKuralRepository,
//    //IIzinTurIzinKuralRepository izinTurIzinKuralRepository,
//    UserManager<AppUser> userManager,
//    IHttpContextAccessor httpContextAccessor,
//    IPersonelAtamaRepository personelAtamaRepository,
//    IPersonelRepository personelRepository) : IRequestHandler<IzinKuralGetAllQuery, IQueryable<IzinKuralGetAllResponse>>
//{
//    public Task<IQueryable<IzinKuralGetAllResponse>> Handle(IzinKuralGetAllQuery request, CancellationToken cancellationToken)
//    {
//        var userIdString = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//        if (string.IsNullOrEmpty(userIdString))
//        {
//            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");
//        }

//        var personel = personelRepository.GetAll()
//            .Where(p => p.UserId == Guid.Parse(userIdString) && !p.IsDeleted)
//            .Select(p => new { p.Id })
//            .FirstOrDefault();

//        if (personel is null)
//            throw new UnauthorizedAccessException("Personel bilgisi bulunamadı.");

//        var izinKurallar = izinKuralRepository.GetAll()
//            .Where(p => !p.IsDeleted)
//            .Include(p => p.IzinTurler);


//        var response = izinKurallar
//                .Join(personelAtamaRepository.GetAll(),
//                    //izinKural => izinKural.SirketId,
//                    //personelAtama => personelAtama.SirketId,
//                    (izinKural, personelAtama) => new { izinKural, personelAtama })
//                .Where(ip => ip.personelAtama.PersonelId == personel.Id && ip.personelAtama.IsActive)
//                .GroupJoin(userManager.Users,
//                    ip => ip.izinKural.CreateUserId,
//                    createUser => createUser.Id,
//                    (ip, createUsers) => new { ip.izinKural,ip.personelAtama, createUsers })
//                .SelectMany(
//                    iuu => iuu.createUsers.DefaultIfEmpty(),
//                    (iuu, createUser) => new { iuu.izinKural, iuu.personelAtama, createUser })
//                .GroupJoin(userManager.Users,
//                      ip => ip.izinKural.UpdateUserId,
//                      updateUser => updateUser.Id,
//                      (ip, updateUsers) => new { ip.izinKural, ip.personelAtama, ip.createUser, updateUsers })
//                .SelectMany(
//                        iuu => iuu.updateUsers.DefaultIfEmpty(),
//                        (iuu, updateUser) => new IzinKuralGetAllResponse
//                        {
//                            Id = iuu.izinKural.Id,
//                            Ad = iuu.izinKural.Ad,
//                            Aciklama = iuu.izinKural.Aciklama,
//                            SirketId = iuu.izinKural.SirketId,
//                            SirketAd = iuu.izinKural.Sirket.Ad,
//                            IsActive = iuu.izinKural.IsActive,
//                            CreatedAt = iuu.izinKural.CreatedAt,
//                            CreateUserId = iuu.createUser == null ? null : iuu.createUser.Id,
//                            CreateUserName = iuu.createUser == null
//                                    ? null
//                                    : iuu.createUser.FirstName + " " + iuu.createUser.LastName + " (" + iuu.createUser.Email + ")",
//                            UpdateAt = iuu.izinKural.UpdateAt,
//                            UpdateUserId = updateUser != null ? updateUser.Id : null,
//                            UpdateUserName = updateUser != null
//                          ? updateUser.FirstName + " " + updateUser.LastName + " (" + updateUser.Email + ")"
//                          : null,
//                            IsDeleted = iuu.izinKural.IsDeleted,
//                            DeleteAt = iuu.izinKural.DeleteAt,
//                            IzinTurler = iuu.izinKural.IzinTurler.Select(it => new IzinTurResponse
//                            {
//                                Id = it.Id,
//                                Ad = it.Ad,
//                                Aciklama = it.Aciklama,
//                                UcretliMi = it.UcretliMi,
//                                LimitTipiName = it.LimitTipi == LimitTipiEnum.Limitsiz ? LimitTipiEnum.Limitsiz.Name : $"{it.LimitTipi.Name} {it.LimitGunSayisi} gün",
//                                LimitGunSayisi = it.LimitGunSayisi
                                
//                            }).ToList(),
//                        });
//        return Task.FromResult(response);
//    }
//}
