using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Services;
internal sealed class DataSeeder(
    ApplicationDbContext context,
    RoleManager<AppRole> roleManager) : IDataSeeder
{
    public async Task<(AppRole role, Pozisyon pozisyon)> SeedAsync(Guid tenantId)
    {
        Pozisyon genelMudur = new();
        AppRole yonetici = new();
        if (!context.Pozisyonlar.Any())
        {
            var (pozisyonlar, roller, pozisyon ,rol) = PozisyonVeRolSeedData.OlusturDefaultPozisyonVeRoller(tenantId);
            genelMudur = pozisyon;
            yonetici = rol;
            await context.Pozisyonlar.AddRangeAsync(pozisyonlar);
            foreach(var role in roller)
            {
                role.CreatedAt = DateTimeOffset.Now;
                await roleManager.CreateAsync(role);
            }
            await context.SaveChangesAsync();
        }
        if (!context.KurumsalBirimTipleri.Any())
        {
            var tipler = KurumsalYapiSeedData.OlusturDefaultYapi(tenantId);

            await context.KurumsalBirimTipleri.AddRangeAsync(tipler);

            await context.SaveChangesAsync();
        }

        return (yonetici, genelMudur);
    }

}
