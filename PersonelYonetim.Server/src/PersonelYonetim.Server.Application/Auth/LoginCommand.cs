﻿using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;
using PersonelYonetim.Server.Application.Tokenler;
using PersonelYonetim.Server.Domain.Tokenler;

namespace PersonelYonetim.Server.Application.Auth;

public sealed record LoginCommand(
    string UsernameOrEmail,
    string Password) : IRequest<Result<LoginCommandResponse>>;

public sealed record LoginCommandResponse
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}

internal sealed class LoginCommandHandler(
    UserManager<AppUser> userManager, 
    SignInManager<AppUser> signInManager,
    //IEmailService emailService,
    IJwtProvider jwtProvider,
    ISender sender) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {AppUser? user = await userManager.Users.FirstOrDefaultAsync(p => p.Email == request.UsernameOrEmail || p.Email == request.UsernameOrEmail || p.UserName == request.UsernameOrEmail, cancellationToken);

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
            TokenCreateCommand tokenCreate = new(user.Id, TokenTypeEnum.FromValue(0));
            var result = sender.Send(tokenCreate,cancellationToken);
            return Result<LoginCommandResponse>.Failure("Confirmation mail sent");
        }
        if (!signInResult.Succeeded)
        {
            return (500, "Password is wrong");
        }

        //Token üret

        var token = await jwtProvider.CreateTokenAsync(user);
        //await emailService.SendAsync(user.Email!, "Login", $"Login successful. AccessToken: {token}",cancellationToken);

        var response = new LoginCommandResponse()
        {
            AccessToken = token,
            RefreshToken = Guid.CreateVersion7().ToString()
        };

        return response;
    }
}
