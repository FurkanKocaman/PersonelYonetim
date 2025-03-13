using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.PersonelDepartmanlar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Users;

namespace PersonelYonetim.Server.Application.Personeller;

public sealed record PersonelGetQuery(
    Guid Id) : IRequest<IQueryable<PersonelGetQueryResponse>> ;

public sealed class PersonelGetQueryResponse : EntityDto
{
    public string FullName { get; set; } = default!;
    public DateTimeOffset DogumTarihi { get; set; }
    public string? Cinsiyet { get; set; }
    public string DepartmanAd { get; set; } = default!;
    public string PozisyonAd { get; set; } = default!;
    public string Eposta { get; set; } = default!;
    public string Telefon { get; set; } = default!;
    public string Ulke { get; set; } = default!;
    public string Sehir { get; set; } = default!;
    public string Ilce { get; set; } = default!;
    public string TamAdres { get; set; } = default!;
}

internal sealed class PersonelGetQueryHandler(
    IPersonelRepository personelRepository,
    IDepartmanRepository departmanRepository,
    IPozisyonRepository pozisyonRepository,
    IPersonelDepartmanRepository personelDepartmanRepository,
    UserManager<AppUser> userManager) : IRequestHandler<PersonelGetQuery, IQueryable<PersonelGetQueryResponse>>
{
    public Task<IQueryable<PersonelGetQueryResponse>> Handle(PersonelGetQuery request, CancellationToken cancellationToken)
    {
        var response = (from entity in personelRepository.GetAll()
                        where entity.Id == request.Id
                        join personel_departman in personelDepartmanRepository.GetAll() on entity.Id equals personel_departman.PersonelId
                        join departman in departmanRepository.GetAll() on personel_departman.DepartmanId equals departman.Id
                        join pozisyon in pozisyonRepository.GetAll() on personel_departman.PozisyonId equals pozisyon.Id
                        join create_user in userManager.Users.AsQueryable() on entity.CreateUserId equals create_user.Id
                        join update_user in userManager.Users.AsQueryable() on entity.UpdateUserId equals update_user.Id
                        into update_user
                        from update_users in update_user.DefaultIfEmpty()
                        select new PersonelGetQueryResponse
                        {
                            Id = entity.Id,
                            FullName = entity.FullName,
                            Cinsiyet = entity.Cinsiyet == null ? null : entity.Cinsiyet == true ? "Erkek" : "Kadın",
                            Eposta = entity.Iletisim.Eposta,
                            Telefon = entity.Iletisim.Telefon,
                            DogumTarihi = entity.DogumTarihi,
                            Ulke = entity.Adres.Ulke,
                            Sehir = entity.Adres.Sehir,
                            Ilce = entity.Adres.Ilce,
                            TamAdres = entity.Adres.TamAdres,
                            DepartmanAd = departman.Ad,
                            PozisyonAd = pozisyon.Ad,
                            IsActive = entity.IsActive,
                            CreatedAt = entity.CreatedAt,
                            CreateUserId = create_user.Id,
                            CreateUserName = create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",
                            UpdateAt = entity.UpdateAt,
                            UpdateUserId = update_users.Id,
                            UpdateUserName = entity.UpdateUserId == null ? null : update_users.FirstName + " " + update_users.LastName + " (" + update_users.Email + ")",
                            IsDeleted = entity.IsDeleted,
                            DeleteAt = entity.DeleteAt,
                        });

        return Task.FromResult(response);
    }
}