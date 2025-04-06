using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonelYonetim.Server.Application.Files;

namespace PersonelYonetim.Server.WebAPI.Modules;

public static class FileModule
{
    public static void RegisterFileRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/files").WithTags("Files").RequireAuthorization();
        group.MapPost("/upload-profile-image",
            async (ISender sender, [FromForm] UploadProfileImageCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.BadRequest(response);
            }).DisableAntiforgery().WithName("UploadProfileImage");

      
    }
}
