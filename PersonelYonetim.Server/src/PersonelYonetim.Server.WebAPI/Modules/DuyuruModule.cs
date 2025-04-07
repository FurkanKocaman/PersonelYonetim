using MediatR;
using PersonelYonetim.Server.Application.Bildirimler;
using PersonelYonetim.Server.Application.Duyurular;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class DuyuruModule
{
    public static void RegisterDuyuruRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/duyurular").WithTags("Duyurular").RequireAuthorization();

        group.MapPost("/create",
            async (ISender sender, DuyuruCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>().WithName("DuyuruCreate");

    }
}
