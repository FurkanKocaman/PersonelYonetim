using GenericRepository;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class PersonelGorevlendirmeRepository : Repository<PersonelGorevlendirme, ApplicationDbContext>, IPersonelGorevlendirmeRepository
{
    public PersonelGorevlendirmeRepository(ApplicationDbContext context) : base(context)
    {
    }
}
