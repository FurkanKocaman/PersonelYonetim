using PersonelYonetim.Server.Domain.Abstractions;

namespace PersonelYonetim.Server.Domain.Tenants;
public sealed class Tenant : Entity
{
    public string Name { get; set; } = string.Empty;
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; } 
    public Tenant(string name, string? connectionString, string? databaseName)
    {
        Name = name;
        ConnectionString = connectionString;
        DatabaseName = databaseName ;
    }
}
