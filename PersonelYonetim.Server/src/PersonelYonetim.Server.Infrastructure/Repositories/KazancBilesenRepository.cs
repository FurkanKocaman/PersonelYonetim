using GenericRepository;
using PersonelYonetim.Server.Domain.Bordro;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class KazancBilesenRepository : Repository<KazancBilesen, ApplicationDbContext>, IKazancBilesenRepository
{
    public KazancBilesenRepository(ApplicationDbContext context) : base(context)
    {
    }
}
