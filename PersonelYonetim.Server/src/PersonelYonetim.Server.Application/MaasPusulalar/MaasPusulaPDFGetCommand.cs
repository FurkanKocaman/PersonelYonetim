using MediatR;
using TS.Result;

namespace PersonelYonetim.Server.Application.MaasPusulalar;
public sealed record MaasPusulaPDFGetCommand(
    Guid personelId,
    int Yil,
    int Ay
    ) : IRequest<Result<string>>;

internal sealed class MaasPusulaPDFGetCommandHandler(
    ) : IRequestHandler<MaasPusulaPDFGetCommand, Result<string>>
{
    public Task<Result<string>> Handle(MaasPusulaPDFGetCommand request, CancellationToken cancellationToken)
    {
        var fileName = $"{request.personelId}_Bordro_{request.Yil}-{request.Ay}.pdf";
        var filePath = Path.Combine("wwwroot", "pdf");

        filePath = Path.Combine(filePath, fileName);

        if (File.Exists(filePath))
        {
            var downloadUrl = $"/pdf/{fileName}";
            return Task.FromResult(Result<string>.Succeed(downloadUrl));
        }
        else
        {
            return Task.FromResult(Result<string>.Failure("Pusulaya ait pdf bulunamamdı"));
        }
    }
}
