using PersonelYönetim.Server.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace PersonelYönetim.Server.WebAPI;

public static class ExtensionsMiddleware
{
    public static void CreateFirstUser(WebApplication app)
    {
        using(var scoped = app.Services.CreateScope())
        {
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<Appuser>>();
            if(!userManager.Users.Any(p => p.UserName == "admin"))
            {
                Appuser user = new()
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
        }
    }
}
