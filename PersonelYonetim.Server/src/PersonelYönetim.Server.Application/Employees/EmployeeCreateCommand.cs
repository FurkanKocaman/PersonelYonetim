using PersonelYönetim.Server.Domain.Employees;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace PersonelYönetim.Server.Application.Employees;
public sealed record EmployeeCreateCommand(
    string FirstName,
    string LastName,
    DateOnly BirtOfDate,
    decimal Salary,
    PersonalInformation PersonalInformation,
    Address? Address,
    bool IsActive) : IRequest<Result<string>>;

public sealed class EmployeeCreateCommandValidator: AbstractValidator<EmployeeCreateCommand>
{
    public EmployeeCreateCommandValidator()
    {
        RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("Firstname must be at least 3 characters");
        RuleFor(x => x.LastName).MinimumLength(3).WithMessage("Lastname must be at least 3 characters");
        RuleFor(x => x.PersonalInformation.TCNo).MinimumLength(11).WithMessage("This TCNo is not valid.")
            .MaximumLength(11).WithMessage("This TCNo is not valid");
    }
}

internal sealed class EmployeeCreateCommandHandler(
    IEmployeeRespository employeeRespository,
    IUnitOfWork unitOfWork) : IRequestHandler<EmployeeCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(EmployeeCreateCommand request, CancellationToken cancellationToken)
    {
        var isEmployeeExist = await employeeRespository.AnyAsync(p => p.PersonalInformation.TCNo == request.PersonalInformation.TCNo, cancellationToken);

        if (isEmployeeExist)
        {
            return Result<string>.Failure("This Employee exist");
        }

        Employee employee = request.Adapt<Employee>();

        employeeRespository.Add(employee);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Employee creation successfully completed";
    }
}



