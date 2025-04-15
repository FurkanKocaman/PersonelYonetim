using MediatR;
using PersonelYonetim.Server.Application.CalismaCizelgeleri;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class CalismaCizelgeModule
{
    public static void RegisterCalismaCizelgeRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/calisma-cizelge").WithTags("CalismaCizelgeleri").RequireAuthorization();
        group.MapPost("/calisma-periyot-create",
            async (ISender sender, GunlukCalismaPeriyotCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>().WithName("CalismaPeriyotCreate");
    }
}
