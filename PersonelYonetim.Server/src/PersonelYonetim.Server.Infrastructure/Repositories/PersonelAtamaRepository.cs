using GenericRepository;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;

internal sealed class PersonelAtamaRepository : Repository<PersonelAtama,ApplicationDbContext>, IPersonelAtamaRepository
{
    public PersonelAtamaRepository(ApplicationDbContext context) : base(context)
    {
    }
}

