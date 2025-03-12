using MediatR;
using TS.Result;
using PersonelYonetim.Server.Application.Personeller;

namespace PersonelYonetim.Server.WebAPI.Modules;
public static class PersonelModule
{
    public static void RegisterPersonelRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/personeller").WithTags("Personeller").RequireAuthorization();

        group.MapPost(string.Empty,
            async (ISender sender, PersonelCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>().WithName("PersonelCreate");
    }
}
