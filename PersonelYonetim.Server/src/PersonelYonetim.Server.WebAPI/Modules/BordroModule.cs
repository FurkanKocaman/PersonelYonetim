using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Auth;
using PersonelYonetim.Server.Application.Bordro;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class BordroModule
{
    public static void RegisterBordroRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/bordro").WithTags("Bordro");

        group.MapPost("/create",
            async (ISender sender, BordroCreateCommand request, CancellationToken cancellationToken, UserManager<AppUser> userManager) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<LoginCommandResponse>>();

    }
}
