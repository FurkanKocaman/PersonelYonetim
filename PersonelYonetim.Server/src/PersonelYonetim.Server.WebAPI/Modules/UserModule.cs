using MediatR;
using PersonelYonetim.Server.Application.Users;
using PersonelYonetim.Server.Domain.RoleClaim;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class UserModule
{
    public static void RegisterUserRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/users").WithTags("Users");
        group.MapPost("create",
            async (ISender sender, UserCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>().WithName("UserCreate");
        group.MapPut("update",
            async (ISender sender, UserUpdateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>().WithName("Userupdate");
        group.MapPost("addroles",
            async (ISender sender, UserAddRolesCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>().WithName("UserAddRoles");

        group.MapGet("confirm-email",
            async (ISender sender, [AsParameters]UserConfirmEmailCommand request, CancellationToken cancellationToken = default) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            }).Produces<Result<string>>().WithName("ConfirmEmail");
    }
}
