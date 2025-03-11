using GenericRepository;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;

internal sealed class DepartmanRepository : Repository<Departman, ApplicationDbContext>, IDepartmanRepository
{
    public DepartmanRepository(ApplicationDbContext context) : base(context)
    {
    }
}

