using GenericRepository;
using PersonelYonetim.Server.Domain.ZamanYonetimler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class CalismaCizelgeRepository : Repository<CalismaCizelge, ApplicationDbContext>, ICalismaCizelgeRepository
{
    public CalismaCizelgeRepository(ApplicationDbContext context) : base(context)
    {
    }
}
