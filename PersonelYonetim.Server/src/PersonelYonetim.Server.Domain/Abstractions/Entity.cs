using PersonelYonetim.Server.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace PersonelYonetim.Server.Domain.Abstractions;
public abstract class Entity
{
    public Entity()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }

    #region Audit Log
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; }
    public Guid CreateUserId { get; set; } = default!;
    public string CreateuserName => GetCreateUserName();
    public DateTimeOffset? UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public string? UpdateuserName => GetUpdateUserName();
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeleteAt { get; set; }
    public Guid? DeleteUserId { get; set; }
    private string GetCreateUserName()
    {
        HttpContextAccessor httpContextAccessor = new();
        var userManager = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();

        AppUser user  = userManager.Users.First(p => p.Id == CreateUserId);
        
        return user.FirstName + " " + user.LastName + " (" + user.Email + ")";
    }
    private string? GetUpdateUserName()
    {
        if (UpdateUserId is null) return null;

        HttpContextAccessor httpContextAccessor = new();
        var userManager = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();

        AppUser user = userManager.Users.First(p => p.Id == UpdateUserId);

        return user.FirstName + " " + user.LastName + " (" + user.Email + ")";
    }
    #endregion
}

