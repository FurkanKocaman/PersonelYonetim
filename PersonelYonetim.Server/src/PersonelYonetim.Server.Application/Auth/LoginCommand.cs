using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace PersonelYonetim.Server.Application.Auth;

public sealed record LoginCommand(
    string Email,
    string Password) : IRequest<Result<LoginCommandResponse>>;

public sealed record LoginCommandResponse
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}

internal sealed class LoginCommandHandler(
    UserManager<Appuser> userManager, 
    SignInManager<Appuser> signInManager,
    IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Appuser? user = await userManager.Users.FirstOrDefaultAsync(p => p.Email == request.Email, cancellationToken);

        if (user is null)
        {
            return Result<LoginCommandResponse>.Failure("User not found");
        }

        SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);
        if(signInResult.IsLockedOut)
        {
            TimeSpan? timeSpan = user.LockoutEnd - DateTime.UtcNow;
            if (timeSpan is not null)
                return (500, $"Your user has been locked for {Math.Ceiling(timeSpan.Value.TotalMinutes)} minutes because you entered your password incorrectly 5 times.");
            else
                return (500, $"Your user has been locked for 5 minutes because you entered your password incorrectly 5 times.");
        }

        if (signInResult.IsNotAllowed)
        {
            return (500, "Your mail is not confirmed");
        }
        if (!signInResult.Succeeded)
        {
            return (500, "Password is wrong");
        }

        //Token üret

        var token = await jwtProvider.CreateTokenAsync(user);

        var response = new LoginCommandResponse()
        {
            AccessToken = token,
            RefreshToken = Guid.CreateVersion7().ToString()
        };

        return response;
    }
}
