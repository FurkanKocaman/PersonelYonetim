using GenericRepository;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;

internal sealed class IzinKuralRepository : Repository<IzinKural, ApplicationDbContext>, IIzinKuralRepository
{
    public IzinKuralRepository(ApplicationDbContext context) : base(context)
    {
    }
}
