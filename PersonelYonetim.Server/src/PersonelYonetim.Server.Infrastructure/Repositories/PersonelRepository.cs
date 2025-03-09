using GenericRepository;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
 
internal sealed class PersonelRepository : Repository<Personel, ApplicationDbContext>, IPersonelRepository
{
    public PersonelRepository(ApplicationDbContext context) : base(context)
    {
    }
}

