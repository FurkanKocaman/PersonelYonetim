using PersonelYönetim.Server.Domain.Employees;
using PersonelYönetim.Server.Infrastructure.Context;
using GenericRepository;

namespace PersonelYönetim.Server.Infrastructure.Repositories;

internal sealed class EmployeeRepository : Repository<Employee, ApplicationDbContext>, IEmployeeRespository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }
}


