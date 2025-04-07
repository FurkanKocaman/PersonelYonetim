using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.Application.Auth;
public sealed record ResetPasswordCommand(
    string UserId,
    string Token,
    string NewPassword) : IRequest<Result<string>>;

internal sealed class ResetPasswordCommandHandler(
    UserManager<AppUser> userManager
    ) : IRequestHandler<ResetPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return Result<string>.Failure("User bulunamadı");

        var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

        if (!result.Succeeded)
            return Result<string>.Failure("Hata oluştu");

        return Result<string>.Succeed("Şifre başarıyla sıfırlandı");

    }
}
