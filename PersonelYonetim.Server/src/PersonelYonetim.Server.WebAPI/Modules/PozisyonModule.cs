using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonelYonetim.Server.Application.Pozisyonlar;
using PersonelYonetim.Server.Domain.RoleClaim;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class PozisyonModule
{
    public static void RegisterPozisyonRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/pozisyonlar").WithTags("Pozisyonlar").RequireAuthorization();
        group.MapPost("/create",
            async (ISender sender, PozisyonCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization(Permissions.CreatePozisyon).Produces<Result<string>>().WithName("PozisyonCreate");
        group.MapPut("/update",
            async (ISender sender, PozisyonUpdateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization(Permissions.EditPozisyon).Produces<Result<string>>().WithName("PozisyonUpdate");
        group.MapDelete("/delete/{id}",
            async (ISender sender,[FromRoute] Guid id, CancellationToken cancellationToken) =>
            {
                PozisyonDeleteCommand request = new(id);
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization(Permissions.DeletePozisyon).Produces<Result<string>>().WithName("PozisyonDelete");
    }
}
