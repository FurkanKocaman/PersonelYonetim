using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Users;

namespace PersonelYonetim.Server.Application.Personeller;

public sealed record PersonelGetQuery(
    Guid Id) : IRequest<IQueryable<PersonelGetQueryResponse>> ;

public sealed class PersonelGetQueryResponse : EntityDto
{
    public string FullName { get; set; } = default!;
    public DateTimeOffset DogumTarihi { get; set; }
    public bool? Cinsiyet { get; set; }
    public string? DepartmanAd { get; set; }
    public string? PozisyonAd { get; set; }
    public string Eposta { get; set; } = default!;
    public string Telefon { get; set; } = default!;
    public string Ulke { get; set; } = default!;
    public string Sehir { get; set; } = default!;
    public string Ilce { get; set; } = default!;
    public string TamAdres { get; set; } = default!;
}

internal sealed class PersonelGetQueryHandler(
    IPersonelRepository personelRepository,
    UserManager<AppUser> userManager) : IRequestHandler<PersonelGetQuery, IQueryable<PersonelGetQueryResponse>>
{
    public Task<IQueryable<PersonelGetQueryResponse>> Handle(PersonelGetQuery request, CancellationToken cancellationToken)
    {
        var response = (from entity in personelRepository.GetAll()
                        where entity.Id == request.Id
                        join create_user in userManager.Users.AsQueryable() on entity.CreateUserId equals create_user.Id
                        join update_user in userManager.Users.AsQueryable() on entity.UpdateUserId equals update_user.Id
                        into update_user
                        from update_users in update_user.DefaultIfEmpty()
                        select new PersonelGetQueryResponse
                        {
                            Id = entity.Id,
                            FullName = entity.FullName,
                            Cinsiyet = entity.Cinsiyet,
                            Eposta = entity.Iletisim.Eposta,
                            Telefon = entity.Iletisim.Telefon,
                            DogumTarihi = entity.DogumTarihi,
                            Ulke = entity.Adres.Ulke,
                            Sehir = entity.Adres.Sehir,
                            Ilce = entity.Adres.Ilce,
                            TamAdres = entity.Adres.TamAdres,
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