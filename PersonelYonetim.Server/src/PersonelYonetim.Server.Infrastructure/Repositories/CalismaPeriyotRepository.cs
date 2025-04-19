using GenericRepository;
using PersonelYonetim.Server.Domain.ZamanYonetimler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class CalismaPeriyotRepository : Repository<CalismaPeriyodu, ApplicationDbContext>, ICalismaPeriyotRepository
{
    public CalismaPeriyotRepository(ApplicationDbContext context) : base(context)
    {
    }
}
