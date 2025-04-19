using GenericRepository;
using PersonelYonetim.Server.Domain.Bordro;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class MaasPusulaRepository : Repository<MaasPusula, ApplicationDbContext>, IMaasPusulaRepository
{
    public MaasPusulaRepository(ApplicationDbContext context) : base(context)
    {
    }
}
