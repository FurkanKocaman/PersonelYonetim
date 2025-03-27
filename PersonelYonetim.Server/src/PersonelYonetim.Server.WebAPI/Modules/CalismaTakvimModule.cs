using MediatR;
using PersonelYonetim.Server.Application.CalismaTakvimleri;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class CalismaTakvimModule
{
    public static void RegisterCalismaTakvimRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/calisma-takvim").WithTags("CalismaTakvimler").RequireAuthorization();

        group.MapPost(string.Empty,
            async (ISender sender, CalismaTakvimiCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>().WithName("CalismaTakvimCreate");
    }
}
