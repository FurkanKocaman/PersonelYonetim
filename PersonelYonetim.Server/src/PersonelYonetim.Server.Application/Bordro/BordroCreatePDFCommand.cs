using MediatR;
using TS.Result;

namespace PersonelYonetim.Server.Application.Bordro;
public sealed record BordroCreatePDFCommand(
    Guid Id
    ) : IRequest<Result<string>>;


internal sealed class BordroCreatePDFCommandHandler(
    ) : IRequestHandler<BordroCreateCommand, Result<string>>
{
    public Task<Result<string>> Handle(BordroCreateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
