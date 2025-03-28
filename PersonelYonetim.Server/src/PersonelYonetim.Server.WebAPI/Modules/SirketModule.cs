﻿using MediatR;
using PersonelYonetim.Server.Application.Sirketler;
using PersonelYonetim.Server.Domain.RoleClaim;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class SirketModule
{
    public static void RegisterSirketRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/sirketler").WithTags("Sirketler").RequireAuthorization();

        group.MapPost("/create", async(ISender sender, SirketCreateCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).RequireAuthorization(Permissions.CreateSirket).Produces<Result<string>>().WithName("SirketCreate");
    }
}
