using MediatR;
using PersonelYonetim.Server.Application.Sirketler;
using PersonelYonetim.Server.Application.Subeler;
using PersonelYonetim.Server.Domain.RoleClaim;
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
        }).RequireAuthorization(Permissions.CreateSube).Produces<Result<string>>().WithName("SubeCreate");
        group.MapPut("/update", async (ISender sender, SubeUpdateCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).RequireAuthorization(Permissions.EditSube).Produces<Result<string>>().WithName("SubeUpdate");
        group.MapDelete("/delete/{id}", async (ISender sender, Guid id, CancellationToken cancellationToken) =>
        {
            SubeDeleteCommand request = new(id);
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).RequireAuthorization(Permissions.DeleteSube).Produces<Result<string>>().WithName("SubeDelete");
    }
}
