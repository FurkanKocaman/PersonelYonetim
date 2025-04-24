using MediatR;
using PersonelYonetim.Server.Application.Bordro;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class BordroModule
{
    public static void RegisterBordroRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/bordro").WithTags("Bordro");

        group.MapPost("/create",
            async (ISender sender, BordroCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            }).Produces<Result<string>>();

        group.MapPut("/update-calisanlar",
           async (ISender sender, BordroCalisanlarUpdateCommand request, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           }).Produces<Result<string>>();

        group.MapPost("/kazanc-ekle",
           async (ISender sender, BordroKazancEkleCommand request, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           }).Produces<Result<string>>();

        group.MapPost("/kesinti-ekle",
        async (ISender sender, BordroKesintiEkleCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>();
    }
}
