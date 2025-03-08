using PersonelYönetim.Server.Domain.Users;

namespace PersonelYönetim.Server.Application.Services;

public interface IJwtProvider
{
    public Task<string> CreateTokenAsync(Appuser user, CancellationToken cancellationToken = default);
}
