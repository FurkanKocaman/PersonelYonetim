﻿using MediatR;
using PersonelYonetim.Server.Application.Users;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class UserModule
{
    public static void RegisterUserRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/users").WithTags("Users").RequireAuthorization("manager");
        group.MapPost("create",
            async (ISender sender, UserCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>().WithName("UserCreate");
        group.MapPost("addrole",
            async (ISender sender, UserAddRolesCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>().WithName("UserAddRole");
    }
}
