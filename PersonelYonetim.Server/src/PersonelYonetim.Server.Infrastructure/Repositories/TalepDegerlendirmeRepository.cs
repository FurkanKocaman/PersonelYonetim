using GenericRepository;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class TalepDegerlendirmeRepository : Repository<TalepDegerlendirme, ApplicationDbContext>, ITalepDegerlendirmeRepository 
{
    public TalepDegerlendirmeRepository(ApplicationDbContext context) : base(context)
    {
    }
}
