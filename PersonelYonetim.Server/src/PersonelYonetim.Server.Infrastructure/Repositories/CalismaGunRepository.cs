using GenericRepository;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;

internal sealed class CalismaGunRepository : Repository<CalismaGun, ApplicationDbContext>, ICalismaGunRepository
{
    public CalismaGunRepository(ApplicationDbContext context) : base(context)
    {
    }
}
