using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonelYonetim.Server.Application.Subeler;
using PersonelYonetim.Server.Application.TakvimEtkinlikler;
using PersonelYonetim.Server.Domain.RoleClaim;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class TakvimEtkinlikModule
{
    public static void RegisterTakvimEtkinlikRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/takvim-etkinlik").WithTags("TakvimEtkinlik").RequireAuthorization();

        group.MapPost("/create", async (ISender sender, [FromBody] TakvimEtkinlikCreateCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).RequireAuthorization().Produces<Result<string>>().WithName("TakvimEtkinlikCreate");

        group.MapPut("/update", async (ISender sender, [FromBody] TakvimEtkinlikUpdateCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).RequireAuthorization().Produces<Result<string>>().WithName("TakvimEtkinlikUpdate");

        group.MapDelete("/delete/{id}", async (ISender sender, [FromRoute]Guid id, CancellationToken cancellationToken) =>
        {
            var request = new TakvimEtkinlikDeleteCommand(id);
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).RequireAuthorization().Produces<Result<string>>().WithName("TakvimEtkinlikDelete");
    }
}
