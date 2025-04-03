using MediatR;
using PersonelYonetim.Server.Application.Files;
using PersonelYonetim.Server.Application.IzinKurallar;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class FileModule
{
    public static void RegisterFileRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/files").WithTags("Files").RequireAuthorization();

        group.MapPost("/upload",
            async (ISender sender, UploadProfileImageCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.BadRequest(response);
            }).WithName("UploadProfileImage");

      
    }
}
