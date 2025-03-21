﻿using MediatR;
using TS.Result;

namespace PersonelYonetim.Server.Application.Roller;

public sealed record RolCreateCommand() : IRequest<Result<string>>;

internal sealed class RoleCreateCommandHandler(
    ) : IRequestHandler<RolCreateCommand, Result<string>>
{
    public Task<Result<string>> Handle(RolCreateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
