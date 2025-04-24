using MediatR;
using PersonelYonetim.Server.Application.Roller;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class RoleModule
{
    public static void RegisterRoleRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/role").WithTags("Auth");

        group.MapPost("create", async (ISender sender, RolCreateCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>().WithName("RoleCreate");


        group.MapPost("add-claim", async (ISender sender, RoleAddClaimsCommand request, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>().WithName("RoleAddClaims");

        group.MapDelete("delete/{Id}", async (ISender sender, Guid Id, CancellationToken cancellationToken) =>
        {
            RoleDeleteCommand request = new(Id);
            var response = await sender.Send(request, cancellationToken);
            return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
        }).Produces<Result<string>>().WithName("RoleDelete");
    }
}
