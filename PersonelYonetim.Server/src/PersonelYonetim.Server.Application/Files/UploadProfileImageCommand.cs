using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TS.Result;

namespace PersonelYonetim.Server.Application.Files;
public sealed record UploadProfileImageCommand( 
    IFormFile file) : IRequest<Result<string>>;

internal sealed class UploadProfileImageCommandHandler(
    IWebHostEnvironment  env) : IRequestHandler<UploadProfileImageCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
    {
        if (request.file == null || request.file.Length == 0)
            return Result<string>.Failure("Dosya yüklemesi başarısız oldu");

        var uploadFolder = Path.Combine(env.WebRootPath, "profile_images");
        if(!Directory.Exists(uploadFolder))
            Directory.CreateDirectory(uploadFolder);

        var fileName = $"{Guid.CreateVersion7()}{Path.GetExtension(request.file.FileName)}.jpg";
        var filePath = Path.Combine(uploadFolder, fileName);

        using(var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.file.CopyToAsync(stream);
        }

        var fileUrl = $"{filePath}";

        return Result<string>.Succeed(fileUrl);

    }
}
