using GenericRepository;
using PersonelYonetim.Server.Domain.ZamanYonetimler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class FazlaMesaiPeriyotRepository : Repository<FazlaMesaiPeriyodu, ApplicationDbContext>, IFazlaMesaiPeriyotRepository
{
    public FazlaMesaiPeriyotRepository(ApplicationDbContext context) : base(context)
    {
    }
}
