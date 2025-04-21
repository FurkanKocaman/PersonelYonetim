using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.MaasPusulalar;
using PersonelYonetim.Server.Domain.Users;
using TS.Result;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class MaasPusulaModule
{
    public static void RegisterMaasPusulaRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/maas-pusula").WithTags("MaasPusula");

        group.MapPost("/create",
            async (ISender sender, MaasPusulaPDFCreateCommand request, CancellationToken cancellationToken, UserManager<AppUser> userManager) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
        group.MapPost("/degerlendir",
            async (ISender sender, MaasPusulaDegerlendirCommand request, CancellationToken cancellationToken, UserManager<AppUser> userManager) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();

        group.MapPut("/update",
            async (ISender sender, MaasPusulaUpdateCommand request, CancellationToken cancellationToken, UserManager<AppUser> userManager) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();

    }
}
