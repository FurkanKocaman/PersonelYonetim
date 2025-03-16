using MediatR;
using PersonelYonetim.Server.Application.Sirketler;
using PersonelYonetim.Server.Application.Subeler;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class SubeModule
{
    public static void RegisterSubeRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/subeler").WithTags("Subeler").RequireAuthorization();

        group.MapPost("/create", async (ISender sender, SubeCreateCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>().WithName("SubeCreate");
    }
}
