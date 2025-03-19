using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;
using TS.Result;

namespace PersonelYonetim.Server.Application.Users;


public sealed record UserAddClaimsCommand(
    Guid Id,
    IEnumerable<string> Claims) : IRequest<Result<string>>;

public sealed class UserAddClaimsCommandValidator : AbstractValidator<UserAddClaimsCommand>
{
    public UserAddClaimsCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id boş olamaz");
    }
}

internal sealed class UserAddClaimsCommandHandler(
    UserManager<AppUser> userManager) : IRequestHandler<UserAddClaimsCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserAddClaimsCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
            return Result<string>.Failure("Kullanıcı bulunamadı");

        //foreach (var claim in request.Claims)
        //{
        //    if (!await roleManager.RoleExistsAsync(role))
        //    {
        //        return Result<string>.Failure($"Rol:{role} bulunamadı");
        //    }
        //    if (await userManager.IsInRoleAsync(user, role))
        //    {
        //        return Result<string>.Failure($"Kullanıcı zaten {role} rolüne sahip");
        //    }
        //}
        IEnumerable<Claim> claims = [new Claim("Permission", "AddPersonel"), new Claim("Permission","DeletePersonel")];
       ;
        var result = await userManager.AddClaimsAsync(user, claims);
        if (!result.Succeeded)
        {
            return Result<string>.Failure("Kullanıcıya claimler eklenirken hata oluştu.");

        }
        return Result<string>.Succeed("Kullanıcıya claimler başarıyla eklendi");

    }
}
