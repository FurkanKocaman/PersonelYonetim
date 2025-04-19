using GenericRepository;
using PersonelYonetim.Server.Domain.Tenants;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class TenantRepository : Repository<Tenant, ApplicationDbContext>, ITenantRepository
{
    public TenantRepository(ApplicationDbContext context) : base(context)
    {
    }
}
