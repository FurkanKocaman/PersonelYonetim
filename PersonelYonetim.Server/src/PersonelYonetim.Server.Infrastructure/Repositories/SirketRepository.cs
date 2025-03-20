using GenericRepository;
using PersonelYonetim.Server.Domain.Sirketler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;

internal sealed class SirketRepository : Repository<Sirket, ApplicationDbContext>, ISirketRepository
{
    public SirketRepository(ApplicationDbContext context) : base(context)
    {
    }
}
