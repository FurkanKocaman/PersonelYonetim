using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Rols;

namespace PersonelYonetim.Server.Application.Services;
public interface IDataSeeder
{
    Task<(AppRole role, Pozisyon pozisyon)> SeedAsync(Guid tenantId);
}
