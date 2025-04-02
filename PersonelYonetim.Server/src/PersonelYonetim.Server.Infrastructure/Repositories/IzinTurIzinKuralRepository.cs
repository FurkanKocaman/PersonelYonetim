using GenericRepository;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;

internal sealed class IzinTurIzinKuralRepository : Repository<IzinTurIzinKural, ApplicationDbContext>, IIzinTurIzinKuralRepository
{
    public IzinTurIzinKuralRepository(ApplicationDbContext context) : base(context)
    {
    }
}
