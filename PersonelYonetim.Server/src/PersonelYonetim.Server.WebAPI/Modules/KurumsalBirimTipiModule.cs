using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonelYonetim.Server.Application.KurumsalBirimler;
using PersonelYonetim.Server.Application.KurumsalBirimTipleri;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class KurumsalBirimTipiModule
{
    public static void RegisterKurumsalBirimTipiRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/kurumsal-birim-tipi").WithTags("KurumsalBirimTipi").RequireAuthorization();
        group.MapPost("/create",
            async (ISender sender, KurumsalBirimTipiCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>().WithName("KurumsalBirimTipiCreate");

        group.MapDelete("/delete/{id}",
             async (ISender sender, [FromRoute] Guid id, CancellationToken cancellationToken) =>
             {
                 KurumsalBirimTipiDeleteCommand request = new(id);
                 var response = await sender.Send(request, cancellationToken);
                 return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
             })
             .RequireAuthorization().Produces<Result<string>>().WithName("KurumsalBirimTipiDelete");
    }
}
