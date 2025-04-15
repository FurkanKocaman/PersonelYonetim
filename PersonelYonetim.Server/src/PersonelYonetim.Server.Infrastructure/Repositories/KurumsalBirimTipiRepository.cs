using GenericRepository;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class KurumsalBirimTipiRepository : Repository<KurumsalBirimTipi, ApplicationDbContext>, IKurumsalBirimTipiRepository
{
    public KurumsalBirimTipiRepository(ApplicationDbContext context) : base(context)
    {
    }
}
