using GenericRepository;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class KurumsalBirimRepository : Repository<KurumsalBirim, ApplicationDbContext>, IKurumsalBirimRepository
{
    public KurumsalBirimRepository(ApplicationDbContext context) : base(context)
    {
    }
}
