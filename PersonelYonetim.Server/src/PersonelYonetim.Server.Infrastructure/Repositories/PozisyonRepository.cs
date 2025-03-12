using GenericRepository;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;

internal sealed class PozisyonRepository : Repository<Pozisyon, ApplicationDbContext>, IPozisyonRepository
{
    public PozisyonRepository(ApplicationDbContext context) : base(context)
    {
    }
}
