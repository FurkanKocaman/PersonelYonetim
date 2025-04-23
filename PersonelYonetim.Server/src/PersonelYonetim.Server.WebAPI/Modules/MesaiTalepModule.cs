using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonelYonetim.Server.Application.MesaiTalepler;
using PersonelYonetim.Server.Domain.RoleClaim;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class MesaiTalepModule
{
    public static void RegisterMesaiRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/mesai-talep").WithTags("MesaiTalep").RequireAuthorization();
        group.MapPost("/create",
            async (ISender sender, MesaiTalepCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization(Permissions.CreateIzinler).Produces<Result<string>>().WithName("MesaiTalepCreate");

        //group.MapPut("/update",
        //    async (ISender sender, [FromBody] MesaiTalepUpdateCommand request, CancellationToken cancellationToken) =>
        //    {
        //        var response = await sender.Send(request, cancellationToken);
        //        return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        //    })
        //     .RequireAuthorization(Permissions.CreateIzinler).Produces<Result<string>>().WithName("MesaiTalepUpdate");
        //group.MapDelete("/delete",
        //   async (ISender sender, [FromBody] MesaiTalepDeleteCommand request, CancellationToken cancellationToken) =>
        //   {
        //       var response = await sender.Send(request, cancellationToken);
        //       return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        //   })
        //   .RequireAuthorization(Permissions.CreateIzinler).Produces<Result<string>>().WithName("MesaiTalepDelete");
        group.MapPost("/degerlendir",
             async (ISender sender, [FromBody] MesaiTalepOnayCommand request, CancellationToken cancellationToken) =>
             {
                 var response = await sender.Send(request, cancellationToken);
                 return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
             })
            .RequireAuthorization().Produces<Result<string>>().WithName("MesaiTalepDegerlendir");
    }
}
