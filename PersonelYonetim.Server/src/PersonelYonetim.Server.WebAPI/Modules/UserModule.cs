using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonelYonetim.Server.Application.Users;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class UserModule
{
    public static void RegisterUserRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/users").WithTags("Users");
            //.RequireAuthorization("manager");
        group.MapPost("create",
            async (ISender sender, UserCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>().WithName("UserCreate");
        group.MapPost("addroles",
            async (ISender sender, UserAddRolesCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>().WithName("UserAddRoles");

        group.MapPost("addclaims",
           async (ISender sender, UserAddClaimsCommand request, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           })
           .Produces<Result<string>>().WithName("UserAddClaims");
        group.MapGet("confirm-email",
            async (ISender sender, [AsParameters]UserConfirmEmailCommand request, CancellationToken cancellationToken = default) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            }).Produces<Result<string>>().WithName("ConfirmEmail");
    }
}
