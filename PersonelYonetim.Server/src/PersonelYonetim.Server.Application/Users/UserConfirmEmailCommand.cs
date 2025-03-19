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
    //ITokenRepository tokenRepository,
    //IUnitOfWork unitOfWork
    ): IRequestHandler<UserConfirmEmailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return Result<string>.Failure("Kullanıcı bulunamadı");

        var result = await userManager.ConfirmEmailAsync(user, request.Token);

        if (!result.Succeeded)
            return Result<string>.Failure("Doğrulama kodu yanlış");

        //var tokenInDb = await tokenRepository.FirstOrDefaultAsync(p => p.TokenString == request.Token);
        //if (tokenInDb is null)
        //    return Result<string>.Failure("Token bulunamadı");

        //tokenRepository.Delete(tokenInDb);
        //await unitOfWork.SaveChangesAsync();

        return Result<string>.Succeed($"{user.Email} confirmed");
    }
}

