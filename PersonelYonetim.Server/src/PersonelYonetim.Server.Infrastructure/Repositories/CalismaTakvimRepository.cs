using GenericRepository;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;

internal sealed class CalismaTakvimRepository : Repository<CalismaTakvimi, ApplicationDbContext>, ICalismaTakvimRepository
{
    public CalismaTakvimRepository(ApplicationDbContext context) : base(context)
    {
    }
}
