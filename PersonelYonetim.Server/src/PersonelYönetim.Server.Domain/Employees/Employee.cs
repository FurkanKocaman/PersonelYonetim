using PersonelYönetim.Server.Domain.Abstractions;

namespace PersonelYönetim.Server.Domain.Employees;

public sealed class Employee:Entity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => String.Join(" ", FirstName, LastName);
    public DateOnly BirtOfDate { get; set; }
    public decimal Salary { get; set; }
    public PersonalInformation PersonalInformation { get; set; } = default!;
    public Address Address { get; set; } = default!;
}
    
