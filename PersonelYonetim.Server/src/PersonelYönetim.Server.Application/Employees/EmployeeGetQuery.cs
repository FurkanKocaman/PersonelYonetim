using PersonelYönetim.Server.Domain.Employees;
using MediatR;
using TS.Result;

namespace PersonelYönetim.Server.Application.Employees;

public sealed record EmployeeGetQuery(Guid Id) : IRequest<Result<Employee>> ;

internal sealed class EmployeeGetQueryHandler(IEmployeeRespository employeeRespository) : IRequestHandler<EmployeeGetQuery, Result<Employee>>
{
    public async Task<Result<Employee>> Handle(EmployeeGetQuery request, CancellationToken cancellationToken)
    {
        var employee = await employeeRespository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if(employee is null)
        {
            return Result<Employee>.Failure("Employee not found");
        }
        return employee;
    }
}