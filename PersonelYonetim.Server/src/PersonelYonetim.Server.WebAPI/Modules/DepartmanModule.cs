using MediatR;
using PersonelYonetim.Server.Application.Departmanlar;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;
public static class DepartmanModule
{
    public static void RegisterDepartmanRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/departmanlar").WithTags("Departmanlar").RequireAuthorization("manager");

        group.MapPost(string.Empty,
            async (ISender sender, DepartmanCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>().WithName("DepartmanCreate");
    }
}
