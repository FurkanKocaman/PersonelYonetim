using GenericRepository;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonelYonetim.Server.Infrastructure.Repositories;

internal sealed class IzinTurIzinKuralRepository : Repository<IzinTurIzinKural, ApplicationDbContext>, IIzinTurIzinKuralRepository
{
    public IzinTurIzinKuralRepository(ApplicationDbContext context) : base(context)
    {
    }
}
