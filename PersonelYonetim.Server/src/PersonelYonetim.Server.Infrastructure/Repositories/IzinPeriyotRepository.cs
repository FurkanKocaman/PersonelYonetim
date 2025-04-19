using GenericRepository;
using PersonelYonetim.Server.Domain.ZamanYonetimler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class IzinPeriyotRepository : Repository<IzinPeriyodu, ApplicationDbContext>, IIzinPeriyotRepository
{
    public IzinPeriyotRepository(ApplicationDbContext context) : base(context)
    {
    }
}
