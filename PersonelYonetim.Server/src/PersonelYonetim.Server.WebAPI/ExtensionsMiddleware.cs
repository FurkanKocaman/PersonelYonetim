using PersonelYonetim.Server.Domain.Users;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Rols;
using System.Threading.Tasks;
using PersonelYonetim.Server.Domain.RoleClaim;
using System.Security.Claims;
using PersonelYonetim.Server.Domain.Roller;
using GenericRepository;

namespace PersonelYonetim.Server.WebAPI;

public static class ExtensionsMiddleware
{
    public static async Task CreateFirstUser(WebApplication app)
    {
        using(var scoped = app.Services.CreateScope())
        {
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            IUserRoleRepository userRoleRepository = scoped.ServiceProvider.GetRequiredService<IUserRoleRepository>();
            IUnitOfWork unitOfWork = scoped.ServiceProvider.GetRequiredService<IUnitOfWork>();

            if (!userManager.Users.Any(p => p.UserName == "admin"))
            {
                AppUser user = new()
                {
                    Id = Guid.Parse("3023f17b-df7f-4720-83b1-5334ec87cd13"),
                    UserName = "admin",
                    Email = "admin@admin.com",
                    FirstName = "Admin",
                    LastName = " ",
                    EmailConfirmed = true,
                    CreatedAt = DateTimeOffset.Now,
                };
                user.CreateUserId = user.Id;
                userManager.CreateAsync(user, "1").Wait();
            }
                var roles = new List<AppRole>
            {
                new AppRole {Id = Guid.CreateVersion7(), Name = RoleClaims.Admin, CreatedAt = DateTimeOffset.Now, CreateUserId = Guid.Parse("3023f17b-df7f-4720-83b1-5334ec87cd13") },
                new AppRole {Id = Guid.CreateVersion7(), Name = RoleClaims.SirketSahibi, CreatedAt = DateTimeOffset.Now, CreateUserId = Guid.Parse("3023f17b-df7f-4720-83b1-5334ec87cd13") },
                new AppRole {Id = Guid.CreateVersion7(), Name = RoleClaims.Yonetici, CreatedAt = DateTimeOffset.Now, CreateUserId = Guid.Parse("3023f17b-df7f-4720-83b1-5334ec87cd13") },
                new AppRole {Id = Guid.CreateVersion7(), Name = RoleClaims.Calisan, CreatedAt = DateTimeOffset.Now, CreateUserId = Guid.Parse("3023f17b-df7f-4720-83b1-5334ec87cd13") }
            };

            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role.Name);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(role);
                }
            }
            var allPermissions = new List<string>
            {
                Permissions.ViewPersonel, Permissions.CreatePersonel, Permissions.EditPersonel, Permissions.DeletePersonel,
                Permissions.ViewSirket, Permissions.CreateSirket, Permissions.EditSirket, Permissions.DeleteSirket,
                Permissions.ViewSube, Permissions.CreateSube, Permissions.EditSube, Permissions.DeleteSube,
                Permissions.ViewDepartman, Permissions.CreateDepartman, Permissions.EditDepartman, Permissions.DeleteDepartman,
                Permissions.ViewPozisyon, Permissions.CreatePozisyon, Permissions.EditPozisyon, Permissions.DeletePozisyon,
                Permissions.ViewIzinler, Permissions.CreateIzinler, Permissions.ApproveIzinler,
                Permissions.ViewRaporlar
            };
            var claims = new Dictionary<string, List<string>>
            {
                { RoleClaims.Admin, allPermissions },
                { RoleClaims.SirketSahibi, allPermissions },

                { RoleClaims.Yonetici, new List<string>
                    {
                        Permissions.ViewPersonel, Permissions.CreatePersonel, Permissions.EditPersonel,
                        Permissions.ViewSirket, Permissions.ViewSube, Permissions.ViewDepartman, Permissions.ViewPozisyon,
                        Permissions.ViewIzinler, Permissions.CreateIzinler, Permissions.ApproveIzinler,
                        Permissions.ViewRaporlar
                    }
                },
                
                { RoleClaims.Calisan, new List<string>
                    {
                        Permissions.ViewPersonel, Permissions.CreateIzinler, Permissions.ViewIzinler
                    }
                }
            };
            foreach (var roleClaim in claims)
            {
                var role = await roleManager.FindByNameAsync(roleClaim.Key);
                foreach (var claim in roleClaim.Value)
                {
                    var claimExist = await roleManager.GetClaimsAsync(role);
                    if (!claimExist.Any(c => c.Value == claim))
                    {
                        await roleManager.AddClaimAsync(role, new Claim("permission", claim));
                    }
                }
            }

            AppUser adminUser = await userManager.FindByNameAsync("admin");
            AppRole adminRole = await roleManager.FindByNameAsync("admin");
            if (!userRoleRepository.Any(p => p.UserId == adminUser.Id && p.RoleId == adminRole.Id))
            {
                AppUserRole appUserRole = new() { UserId = adminUser.Id, SirketId = adminUser.Id, RoleId = adminRole.Id };
                userRoleRepository.Add(appUserRole);
                await unitOfWork.SaveChangesAsync();
            }

        }
    }
}
