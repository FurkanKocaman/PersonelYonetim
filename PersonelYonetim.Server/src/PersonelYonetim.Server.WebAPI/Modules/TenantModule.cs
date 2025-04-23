using MediatR;
using PersonelYonetim.Server.Application.Tenants;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class TenantModule
{
    public static void RegisterTenantRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/tenants").WithTags("Tenants");
       
        group.MapPut("update",
            async (ISender sender, TenantUpdateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>().WithName("TenantUpdate");
    }
}
