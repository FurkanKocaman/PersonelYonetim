using GenericRepository;
using PersonelYonetim.Server.Domain.PersonelDepartmanlar;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;

internal sealed class PersonelDepartmanRepository : Repository<PersonelDepartman,ApplicationDbContext>, IPersonelDepartmanRepository
{
    public PersonelDepartmanRepository(ApplicationDbContext context) : base(context)
    {
    }
}

