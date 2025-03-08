namespace PersonelYönetim.Server.Domain.Employees;

public sealed record PersonalInformation
{
    public string TCNo { get; set; } = default!;
    public string? Email { get; set; }
    public string? Phone1 { get; set; }
    public string? Phone2 { get; set; }
}
    
