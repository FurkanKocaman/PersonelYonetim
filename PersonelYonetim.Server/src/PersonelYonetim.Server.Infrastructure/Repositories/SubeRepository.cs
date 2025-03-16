using GenericRepository;
using PersonelYonetim.Server.Domain.Subeler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;

internal sealed class SubeRepository : Repository<Sube, ApplicationDbContext>, ISubeRepository
{
    public SubeRepository(ApplicationDbContext context) : base(context)
    {
    }
}
