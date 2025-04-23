using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Rols;

namespace PersonelYonetim.Server.Infrastructure.Configurations;
public class CustomRoleValidator : IRoleValidator<AppRole>
{
    public Task<IdentityResult> ValidateAsync(RoleManager<AppRole> manager, AppRole role)
    {
        var errors = new List<IdentityError>();

        if (string.IsNullOrEmpty(role.Name))
        {
            errors.Add(new IdentityError
            {
                Code = "RoleNameIsEmpty",
                Description = "Rol adı boş olamaz"
            });
        }

        return errors.Count == 0 ? Task.FromResult(IdentityResult.Success) : Task.FromResult(IdentityResult.Failed(errors.ToArray()));

    }
}
