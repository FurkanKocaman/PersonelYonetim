using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Domain.Tokenler;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.Application.Users;

public sealed record UserConfirmEmailCommand(
    string UserId,
    string Token) : IRequest<Result<string>>;

internal sealed class UserConfirmEmailCommandHandler(
    UserManager<AppUser> userManager
    ): IRequestHandler<UserConfirmEmailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user == null || user.IsDeleted)
            return Result<string>.Failure("Kullanıcı bulunamadı");

        var result = await userManager.ConfirmEmailAsync(user, request.Token);

        if (!result.Succeeded)
            return Result<string>.Failure("Doğrulama kodu yanlış");

        return Result<string>.Succeed($"{user.Email} confirmed");
    }
}

