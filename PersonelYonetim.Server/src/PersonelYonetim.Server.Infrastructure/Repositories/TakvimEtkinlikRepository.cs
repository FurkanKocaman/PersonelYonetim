using GenericRepository;
using PersonelYonetim.Server.Domain.TakvimEtkinlikler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class TakvimEtkinlikRepository : Repository<TakvimEtkinlik, ApplicationDbContext>, ITakvimEtkinlikRepository
{
    public TakvimEtkinlikRepository(ApplicationDbContext context) : base(context)
    {
    }
}
