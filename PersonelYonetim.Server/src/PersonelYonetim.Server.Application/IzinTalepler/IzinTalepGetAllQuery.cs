using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Personeller;
using PersonelYonetim.Server.Domain.IzinTalepler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Users;

namespace PersonelYonetim.Server.Application.IzinTalepler;

public sealed record IzinTalepGetAllQuery(Guid Id) : IRequest<IQueryable<IzinTalepGetAllQueryResponse>>;

public sealed class IzinTalepGetAllQueryResponse
{
    public Guid Id { get; set; }
    public DateTimeOffset BaslangicTarihi { get; set; }
    public DateTimeOffset BitisTarihi { get; set; }
    public string IzinTipi { get; set; } = default!;
    public string Aciklama { get; set; } = default!;
    public string OnayDurumu { get; set; } = default!;
    public string Onaylayan { get; set; } = default!;
}

internal sealed class IzinTalepGetAllQueryHandler(
    IIzinTalepRepository izinTalepRepository,
    UserManager<AppUser> userManager) : IRequestHandler<IzinTalepGetAllQuery, IQueryable<IzinTalepGetAllQueryResponse>>
{
    public Task<IQueryable<IzinTalepGetAllQueryResponse>> Handle(IzinTalepGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = (from entity in izinTalepRepository.GetAll() where entity.DepartmanId == request.Id
                        join onay_user in userManager.Users.AsQueryable() on entity.OnaylayanId equals onay_user.Id
                        into onay_user
                        from onay_users in onay_user.DefaultIfEmpty()
                        select new IzinTalepGetAllQueryResponse
                        {
                            Id = entity.Id,
                            BaslangicTarihi = entity.BaslangicTarihi,
                            BitisTarihi = entity.BitisTarihi,
                            IzinTipi = entity.IzinTipi.Name,
                            Aciklama = entity.Aciklama!,
                            OnayDurumu = entity.OnayDurumu.Name,
                            Onaylayan = entity.OnaylayanId == null ? "" : onay_users.FirstName + " " + onay_users.LastName + " (" + onay_users.Email + ")",
                        });

        return Task.FromResult(response);
    }
}
