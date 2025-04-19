using GenericRepository;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class GorevlendirmeIzinKuraliRepository : Repository<GorevlendirmeIzinKurali, ApplicationDbContext>, IGorevlendirmeIzinKuraliRepository
{
    public GorevlendirmeIzinKuraliRepository(ApplicationDbContext context) : base(context)
    {
    }
}
