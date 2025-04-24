using GenericRepository;
using PersonelYonetim.Server.Domain.Bordro;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class KesintiBilesenRepository : Repository<KesintiBilesen, ApplicationDbContext>, IKesintiBilesenRepository
{
    public KesintiBilesenRepository(ApplicationDbContext context) : base(context)
    {
    }
}
