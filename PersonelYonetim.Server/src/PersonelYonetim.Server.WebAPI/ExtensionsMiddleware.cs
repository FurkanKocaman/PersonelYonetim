using PersonelYonetim.Server.Domain.Users;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Rols;
using System.Threading.Tasks;

namespace PersonelYonetim.Server.WebAPI;

public static class ExtensionsMiddleware
{
    public static async Task CreateFirstUser(WebApplication app)
    {
        using(var scoped = app.Services.CreateScope())
        {
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            if (!userManager.Users.Any(p => p.UserName == "admin"))
            {
                AppUser user = new()
                {
                    Id = Guid.Parse("3023f17b-df7f-4720-83b1-5334ec87cd13"),
                    UserName = "admin",
                    Email = "admin@admin.com",
                    FirstName = "Furkan",
                    LastName = "Kocaman",
                    EmailConfirmed = true,
                    CreatedAt = DateTimeOffset.Now,
                };
                user.CreateUserId = user.Id;
                userManager.CreateAsync(user, "1").Wait();
            }
            if (!roleManager.Roles.Any(p => p.Name == "admin"))
            {
                AppRole role = new()
                {
                    Name = "admin",
                    CreatedAt = DateTimeOffset.Now,
                    CreateUserId = Guid.Parse("3023f17b-df7f-4720-83b1-5334ec87cd13"),
                };
                roleManager.CreateAsync(role).Wait();
            }
            if (!roleManager.Roles.Any(p => p.Name == "yönetici"))
            {
                AppRole role = new()
                {
                    Name = "yönetici",
                    CreatedAt = DateTimeOffset.Now,
                    CreateUserId = Guid.Parse("3023f17b-df7f-4720-83b1-5334ec87cd13"),
                };
                roleManager.CreateAsync(role).Wait();
            }
            if (!roleManager.Roles.Any(p => p.Name == "çalışan"))
            {
                AppRole role = new()
                {
                    Name = "çalışan",
                    CreatedAt = DateTimeOffset.Now,
                    CreateUserId = Guid.Parse("3023f17b-df7f-4720-83b1-5334ec87cd13"),
                };
                roleManager.CreateAsync(role).Wait();
            }
            if (!roleManager.Roles.Any(p => p.Name == "şirketSahibi"))
            {
                AppRole role = new()
                {
                    Name = "şirketSahibi",
                    CreatedAt = DateTimeOffset.Now,
                    CreateUserId = Guid.Parse("3023f17b-df7f-4720-83b1-5334ec87cd13"),
                };
                roleManager.CreateAsync(role).Wait();
            }

        }
    }
}
