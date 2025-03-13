using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.Application.Users;

public sealed record UserAddRolesCommand(
    Guid Id,
    IEnumerable<string> Roles) : IRequest<Result<string>>;

public sealed class UserAddRolesCommandValidator : AbstractValidator<UserAddRolesCommand>
{
    public UserAddRolesCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id boş olamaz");
    }
}

internal sealed class UserAddRolesCommandHandler(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager) : IRequestHandler<UserAddRolesCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserAddRolesCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
            return Result<string>.Failure("Kullanıcı bulunamadı");
        foreach (var role in request.Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                return Result<string>.Failure($"Rol:{role} bulunamadı");
            }
            if(await userManager.IsInRoleAsync(user, role))
            {
                return Result<string>.Failure($"Kullanıcı zaten {role} rolüne sahip");
            }
        }

        var result = await userManager.AddToRolesAsync(user, request.Roles);
        if (!result.Succeeded)
        {
            return Result<string>.Failure("Kullanıcıya rol eklenirken hata oluştu.");
            
        }
        return Result<string>.Succeed("Kullanıcıya rol başarıyla eklendi");

    }
}