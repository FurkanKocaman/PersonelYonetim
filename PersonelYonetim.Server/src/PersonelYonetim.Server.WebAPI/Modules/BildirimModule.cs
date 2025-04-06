using MediatR;
using PersonelYonetim.Server.Application.Bildirimler;
using PersonelYonetim.Server.Application.Departmanlar;
using PersonelYonetim.Server.Domain.RoleClaim;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class BildirimModule
{
    public static void RegisterBildirimRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/bildirimler").WithTags("Bildirimler").RequireAuthorization();

        group.MapPut("/update",
            async (ISender sender, BildirimUpdateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>().WithName("BildirimUpdate");
      
    }
}
