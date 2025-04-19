using GenericRepository;
using PersonelYonetim.Server.Domain.Bordro;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class BordroDonemRepository : Repository<BordroDonem, ApplicationDbContext>, IBordroDonemRepository
{
    public BordroDonemRepository(ApplicationDbContext context) : base(context)
    {
    }
}
