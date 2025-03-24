using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Tokenler;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.Application.Tokenler;

public sealed record TokenCreateCommand(
    Guid UserId,
    TokenTypeEnum TokenTypeEnum) : IRequest<Result<string>>;

internal sealed class TokenCreateCommandHandler(
    UserManager<AppUser> userManager,
    IEmailService emailService
    ) : IRequestHandler<TokenCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(TokenCreateCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
            return Result<string>.Failure("Kullanıcı bulunamadı.");

        Token token = new();
        token.UserId = user.Id;
        token.TokenString = await userManager.GenerateEmailConfirmationTokenAsync(user);
        token.TokenType = request.TokenTypeEnum;
        token.Expires = DateTimeOffset.UtcNow.AddHours(1);
        token.UserId = user.Id;

        string confirmationLink = $"https://localhost:7063/users/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token.TokenString)}";

        emailService.SendAsync(user.Email!, $"{request.TokenTypeEnum.Name} Link", $"<p>E-posta adresinizi doğrulamak için aşağıdaki bağlantıya tıklayın:</p><a href={confirmationLink}>E-posta Doğrula</a>", cancellationToken).Wait();

        return Result<string>.Succeed($"Token {user.Email} adresine gönderildi");
    }
}
