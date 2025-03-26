using GenericRepository;
using PersonelYonetim.Server.Domain.PersonelIzinler;
using PersonelYonetim.Server.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonelYonetim.Server.Infrastructure.Repositories;
internal sealed class PersonelIzinRepository : Repository<PersonelIzin, ApplicationDbContext>, IPersonelIzinRepository
{
    public PersonelIzinRepository(ApplicationDbContext context) : base(context)
    {
    }
}
