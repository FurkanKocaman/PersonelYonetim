using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TS.Result;

namespace PersonelYonetim.Server.Application.Files;
public sealed record UploadProfileImageCommand(
     [FromForm]IFormFile File) : IRequest<Result<string>>;

internal sealed class UploadProfileImageCommandHandler(
    IWebHostEnvironment  env) : IRequestHandler<UploadProfileImageCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
    {
        if (request.File == null || request.File.Length == 0)
            return Result<string>.Failure("Dosya yüklemesi başarısız oldu");

        var uploadFolder = Path.Combine(env.WebRootPath, "profile_images");
        if(!Directory.Exists(uploadFolder))
            Directory.CreateDirectory(uploadFolder);

        var fileName = $"{Guid.CreateVersion7()}{Path.GetExtension(request.File.FileName)}";
        var filePath = Path.Combine(uploadFolder, fileName);

        using(var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream);
        }

        var fileUrl = $"/profile_images/{fileName}";

        return Result<string>.Succeed(fileUrl);

    }
}
