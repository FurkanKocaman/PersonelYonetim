using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.Application.Users;

public sealed record UserDeleteCommand(
    string Email) :IRequest<Result<string>> ;

internal sealed class UserDeleteCommandHandler(
    UserManager<AppUser> userManager) : IRequestHandler<UserDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null || user.IsDeleted)
            return Result<string>.Failure("Kullanıcı bulunamadı");

        user.IsDeleted = true;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return Result<string>.Failure("Kullanıcı silinirken hata oluştu");
        return Result<string>.Succeed("Kullanıcı başarıyla silindi");
    }
}