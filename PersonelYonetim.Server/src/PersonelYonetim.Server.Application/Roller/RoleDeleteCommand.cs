using MediatR;
using Microsoft.AspNetCore.Identity;
using PersonelYonetim.Server.Application.Services;
using PersonelYonetim.Server.Domain.Rols;
using TS.Result;

namespace PersonelYonetim.Server.Application.Roller;
public sealed record RoleDeleteCommand(
    Guid Id
    ):IRequest<Result<string>>;

internal sealed class RoleDeleteCommandHandler(
    RoleManager<AppRole> roleManager,
    ICurrentUserService currentUserService
    ) : IRequestHandler<RoleDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        Guid? userId = currentUserService.UserId;
        if (!tenantId.HasValue || !userId.HasValue)
            return Result<string>.Failure("tenant bulunamadı");

        var role = await roleManager.FindByIdAsync(request.Id.ToString());
        if (role is null || role.TenantId != tenantId)
            return Result<string>.Failure("ROl bulunamamdı");

        role.IsDeleted = true;
        role.DeleteUserId = userId.Value;
        role.DeleteAt = DateTimeOffset.Now;

        await roleManager.UpdateAsync(role);

        return Result<string>.Succeed("Role başarıyla silindi");
    }
}
