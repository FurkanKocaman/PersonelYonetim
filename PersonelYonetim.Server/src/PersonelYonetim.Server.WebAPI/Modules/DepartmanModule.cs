using MediatR;
using PersonelYonetim.Server.Application.Departmanlar;
using PersonelYonetim.Server.Domain.RoleClaim;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;
public static class DepartmanModule
{
    public static void RegisterDepartmanRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/departmanlar").WithTags("Departmanlar").RequireAuthorization();

        group.MapPost("/create",
            async (ISender sender, DepartmanCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization(Permissions.CreateDepartman).Produces<Result<string>>().WithName("DepartmanCreate");
        group.MapPut("/update",
            async (ISender sender, DepartmanUpdateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization(Permissions.EditDepartman).Produces<Result<string>>().WithName("DepartmanUpdate");
        group.MapDelete("/delete",
            async (ISender sender, Guid id, CancellationToken cancellationToken) =>
            {
                DepartmanDeleteCommand request = new(id);
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization(Permissions.DeleteDepartman).Produces<Result<string>>().WithName("DepartmanDelete");
    }
}
