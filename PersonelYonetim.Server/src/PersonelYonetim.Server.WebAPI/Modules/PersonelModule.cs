using MediatR;
using TS.Result;
using PersonelYonetim.Server.Application.Personeller;
using Microsoft.AspNetCore.Mvc;

namespace PersonelYonetim.Server.WebAPI.Modules;
public static class PersonelModule
{
    public static void RegisterPersonelRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/personeller").WithTags("Personeller").RequireAuthorization();

        group.MapPost("create",
            async (ISender sender, PersonelCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>().WithName("PersonelCreate");

        group.MapPut("update",
            async (ISender sender, PersonelUpdateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>().WithName("PersonelUpdate");

        group.MapDelete("delete",
             async (ISender sender, [FromBody]PersonelDeleteCommand request, CancellationToken cancellationToken) =>
             {
                 var response = await sender.Send(request, cancellationToken);
                 return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
             })
            .Produces<Result<string>>().WithName("PersonelDelete");
    }
}
