using GenericRepository;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal class PersonelIzinKuralRepository : Repository<PersonelIzinKural, ApplicationDbContext>, IPersonelIzinKuralRepository
{
    public PersonelIzinKuralRepository(ApplicationDbContext context) : base(context)
    {
    }
}
