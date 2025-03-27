using MediatR;
using PersonelYonetim.Server.Application.IzinKurallar;
using PersonelYonetim.Server.Application.IzinTalepler;
using PersonelYonetim.Server.Application.IzinTurler;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class IzinModule
{
    public static void RegisterIzinRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/izinler").WithTags("Izinler").RequireAuthorization();

        group.MapPost("/izin-turler",
            async (ISender sender, IzinTurCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.BadRequest(response);
            }).WithName("IzinTurCreate");

        group.MapPost("/izin-kurallar",
           async (ISender sender, IzinKuralCreateCommand request, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.BadRequest(response);
           }).WithName("IzinKuralCreate");
    }
}
