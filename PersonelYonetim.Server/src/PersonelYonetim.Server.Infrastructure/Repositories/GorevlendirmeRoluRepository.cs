using GenericRepository;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class GorevlendirmeRoluRepository : Repository<GorevlendirmeRolu, ApplicationDbContext>, IGorevlendirmeRoluRepository
{
    public GorevlendirmeRoluRepository(ApplicationDbContext context) : base(context)
    {
    }
}
