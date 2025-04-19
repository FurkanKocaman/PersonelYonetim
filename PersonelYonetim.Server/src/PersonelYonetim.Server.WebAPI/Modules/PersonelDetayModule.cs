using MediatR;
using PersonelYonetim.Server.Application.PersonelDetaylar;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class PersonelDetayModule
{
    public static void RegisterpersonelDetayRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/personel-detay").WithTags("PersonelDetay").RequireAuthorization();
        group.MapPost("/create",
            async (ISender sender, PersonelDetayCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>().WithName("PersonelDetayCreate");

        group.MapPut("/update",
            async (ISender sender, PersonelDetayUpdateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>().WithName("PersonelDetayUpdate");
    }
}
