using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Roller;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Domain.UnitOfWork;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.Application.Users;

public sealed record UserAddRolesCommand(
    Guid Id,
    Guid SirketId,
    IEnumerable<int> Roles) : IRequest<Result<string>>;

public sealed class UserAddRolesCommandValidator : AbstractValidator<UserAddRolesCommand>
{
    public UserAddRolesCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id boş olamaz");
    }
}

internal sealed class UserAddRolesCommandHandler(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    IUserRoleRepository userRoleRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UserAddRolesCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserAddRolesCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
            return Result<string>.Failure("Kullanıcı bulunamadı");
        foreach (var role in request.Roles)
        {
            //if (!await roleManager.RoleExistsAsync(role.ToString()))
            //{
            //    return Result<string>.Failure($"Rol:{role} bulunamadı");
            //}
            var roleInDb = await roleManager.FindByNameAsync(role.ToString());
            if(await userRoleRepository.AnyAsync(p => p.UserId == request.Id && p.RoleId == roleInDb!.Id && p.SirketId == request.SirketId))
            {
                return Result<string>.Failure($"Kullanıcı belirtilen şirkette {role} rolüne sahip");
            }
            AppUserRole appUserRole = new()
            {
                UserId = request.Id,
                SirketId = request.SirketId,
                RoleId = roleInDb!.Id,
            };
            userRoleRepository.Add(appUserRole);
        }
        await unitOfWork.SaveChangesAsync();
    
        return Result<string>.Succeed("Kullanıcıya rol başarıyla eklendi");

    }
}