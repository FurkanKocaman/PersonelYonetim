using MediatR;
using PersonelYonetim.Server.Application.Pozisyonlar;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class PozisyonModule
{
    public static void RegisterPozisyonRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/pozisyonlar").WithTags("Pozisyonlar").RequireAuthorization();
        group.MapPost("create",
            async (ISender sender, PozisyonCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>().WithName("PozisyonCreate");
    }
}
