using GenericRepository;
using PersonelYonetim.Server.Domain.ZamanYonetimler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class GunlukCalismaRepository : Repository<GunlukCalisma, ApplicationDbContext>, IGunlukCalismaRepository
{
    public GunlukCalismaRepository(ApplicationDbContext context) : base(context)
    {
    }
}
