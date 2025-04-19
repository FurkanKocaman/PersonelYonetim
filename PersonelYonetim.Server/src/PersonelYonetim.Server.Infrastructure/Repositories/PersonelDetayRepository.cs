using GenericRepository;
using PersonelYonetim.Server.Domain.PersonelDetaylar;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class PersonelDetayRepository : Repository<PersonelDetay, ApplicationDbContext>, IPersonelDetayRepository
{
    public PersonelDetayRepository(ApplicationDbContext context) : base(context)
    {
    }
}
