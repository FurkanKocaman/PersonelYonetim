using GenericRepository;
using PersonelYonetim.Server.Domain.Duyurular;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class DuyuruRepository : Repository<Duyuru, ApplicationDbContext>, IDuyuruRepository
{
    public DuyuruRepository(ApplicationDbContext context) : base(context)
    {
    }
}
