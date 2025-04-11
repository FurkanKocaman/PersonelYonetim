using GenericRepository;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class OnaySurecRepository : Repository<OnaySurec, ApplicationDbContext>, IOnaySurecRepository
{
    public OnaySurecRepository(ApplicationDbContext context) : base(context)
    {
    }
}
