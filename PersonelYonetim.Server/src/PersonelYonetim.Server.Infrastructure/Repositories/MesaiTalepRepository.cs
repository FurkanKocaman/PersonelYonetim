using GenericRepository;
using PersonelYonetim.Server.Domain.Mesailer;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class MesaiTalepRepository : Repository<MesaiTalep, ApplicationDbContext>, IMesaiTalepRepository
{
    public MesaiTalepRepository(ApplicationDbContext context) : base(context)
    {
    }
}
