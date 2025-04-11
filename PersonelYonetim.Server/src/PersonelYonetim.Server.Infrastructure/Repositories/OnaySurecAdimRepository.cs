using GenericRepository;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class OnaySurecAdimRepository : Repository<OnaySureciAdimi, ApplicationDbContext>, IOnaySurecAdimRepository
{
    public OnaySurecAdimRepository(ApplicationDbContext context) : base(context)
    {
    }
}
