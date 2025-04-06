using GenericRepository;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class BildirimRepository : Repository<Bildirim, ApplicationDbContext>, IBildirimRepository
{
    public BildirimRepository(ApplicationDbContext context) : base(context)
    {
    }
}
