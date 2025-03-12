using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonelYonetim.Server.Application.IzinTalepler;
using PersonelYonetim.Server.Application.Personeller;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class IzinTalepModule
{
    public static void RegisterIzinTalepRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/izin-talep").WithTags("IzinTalep").RequireAuthorization("user");

        group.MapPost("create",
            async (ISender sender, [FromBody] IzinTalepCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>().WithName("IzinTalepCreate");

        group.MapPut("update",
            async (ISender sender, [FromBody] IzinTalepUpdateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>().WithName("IzinTalepUpdate");

        group.MapDelete("delete",
           async (ISender sender, [FromBody] IzinTalepDeleteCommand request, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           })
           .Produces<Result<string>>().WithName("IzinTalepDelete");

        RouteGroupBuilder _group = app.MapGroup("/izin-talep").WithTags("IzinTalep").RequireAuthorization("manager");
        _group.MapPost("onay",
             async (ISender sender, [FromBody]IzinTalepOnayCommand request, CancellationToken cancellationToken) =>
             {
                 var response = await sender.Send(request, cancellationToken);
                 return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
             })
            .Produces<Result<string>>().WithName("IzinTalepOnay");
    }
}
