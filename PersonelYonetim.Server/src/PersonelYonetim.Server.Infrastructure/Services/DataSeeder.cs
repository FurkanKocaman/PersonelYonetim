using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.RoleClaim;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Infrastructure.Context;
using System.Security.Claims;

namespace PersonelYonetim.Server.Infrastructure.Services;
internal sealed class DataSeeder(
    ApplicationDbContext context,
    RoleManager<AppRole> roleManager) : IDataSeeder
{
    public async Task<(AppRole role, Pozisyon pozisyon)> SeedAsync(Guid tenantId)
    {
        Pozisyon genelMudur = new();
        AppRole yonetici = new();

            var (pozisyonlar, roller, pozisyon ,rol) = PozisyonVeRolSeedData.OlusturDefaultPozisyonVeRoller(tenantId);
            genelMudur = pozisyon;
            yonetici = rol;
            await context.Pozisyonlar.AddRangeAsync(pozisyonlar);
            var allPermissions = new List<string>
            {
            Permissions.ViewPersonel, Permissions.CreatePersonel, Permissions.EditPersonel, Permissions.DeletePersonel,
            Permissions.ViewKurumsalYapi, Permissions.CreateKurumsalYapi, Permissions.EditKurumsalYapi, Permissions.DeleteKurumsalYapi,
            Permissions.ViewIzinler, Permissions.CreateIzinler, Permissions.ApproveIzinler,
            Permissions.ViewRaporlar
            };
            foreach (var role in roller)
            {
                role.CreatedAt = DateTimeOffset.Now;
                await roleManager.CreateAsync(role);
                if(role.Name == "Birim Yöneticisi" || role.Name == "Sistem Yöneticisi (Admin)")
                {
                    foreach (var permission in allPermissions)
                    {
                        await roleManager.AddClaimAsync(yonetici, new Claim("permission", permission));
                    }
                }
            }
            

          

            await context.SaveChangesAsync();
        

        return (yonetici, genelMudur);
    }

}
