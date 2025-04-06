using GenericRepository;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class PersonelBildirimRepository : Repository<PersonelBildirim, ApplicationDbContext>, IPersonelBildirimRepository
{
    public PersonelBildirimRepository(ApplicationDbContext context) : base(context)
    {
    }
}

