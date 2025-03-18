using MediatR;
using PersonelYonetim.Server.Application.Pozisyonlar;
using PersonelYonetim.Server.Domain.RoleClaim;
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
            .RequireAuthorization(Permissions.CreatePozisyon).Produces<Result<string>>().WithName("PozisyonCreate");
    }
}
