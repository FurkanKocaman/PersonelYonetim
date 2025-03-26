using GenericRepository;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;

internal sealed class IzinTurRepository : Repository<IzinTur, ApplicationDbContext>, IIzinTurRepository
{
    public IzinTurRepository(ApplicationDbContext context) : base(context)
    {
    }

}
