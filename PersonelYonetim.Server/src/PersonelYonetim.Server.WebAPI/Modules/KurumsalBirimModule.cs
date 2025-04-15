using MediatR;
using PersonelYonetim.Server.Application.KurumsalBirimler;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class KurumsalBirimModule
{
    public static void RegisterKurumsalBirimRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/kurumsal-birim").WithTags("KurumsalBirim").RequireAuthorization();
        group.MapPost("/create",
            async (ISender sender, KurumsalBirimCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<Guid>>().WithName("KurumsalBirimCreate");
    }
}
