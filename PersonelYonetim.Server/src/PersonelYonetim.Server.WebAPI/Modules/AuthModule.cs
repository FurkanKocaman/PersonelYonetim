using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Auth;
using PersonelYonetim.Server.Application.Roller;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class AuthModule
{
    public static void RegisterAuthRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("login",
            async (ISender sender, LoginCommand request, CancellationToken cancellationToken, UserManager<AppUser> userManager) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<LoginCommandResponse>>();

        group.MapPost("register",
            async (ISender sender, RegisterCommand request, CancellationToken cancellationToken, UserManager<AppUser> userManager) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<LoginCommandResponse>>();

        group.MapPost("send-reset-password-token", async (ISender sender, SendResetPasswordTokenCommand request, CancellationToken cancellationToken, UserManager<AppUser> userManager) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        })
            .Produces<Result<string>>();
        group.MapPost("reset-password", async (ISender sender, ResetPasswordCommand request, CancellationToken cancellationToken, UserManager<AppUser> userManager) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        })
            .Produces<Result<string>>();
        group.MapPost("add-claim", async (ISender sender, RoleAddClaimsCommand request, CancellationToken cancellationToken, UserManager<AppUser> userManager) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        })
           .Produces<Result<string>>();
    }
}
