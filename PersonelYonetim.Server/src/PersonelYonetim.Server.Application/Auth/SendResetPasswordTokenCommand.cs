using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Result;

namespace PersonelYonetim.Server.Application.Auth;
public sealed record SendResetPasswordTokenCommand(
    string Email) : IRequest<Result<string>>;


internal sealed class SendResetPasswordTokenCommandHandler(
    UserManager<AppUser> userManager,
    IEmailService emailService) : IRequestHandler<SendResetPasswordTokenCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SendResetPasswordTokenCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Result<string>.Failure("User bulunamadı");

        string token = await userManager.GeneratePasswordResetTokenAsync(user);

        string confirmationLink = $"http://localhost:5173/reset-password?userId={user.Id}&token={Uri.EscapeDataString(token)}";
        emailService.SendAsync(user.Email!, $"Reset Password Link", $"<p>Şifrenizi sıfırlamak için aşağıdaki bağlantıya tıklayın:</p><a href={confirmationLink}>Reset Password</a>", cancellationToken).Wait();


        return Result<string>.Succeed($"Şifre sıfırlama linki {user.Email} adresine gönderildi");
    }
}

