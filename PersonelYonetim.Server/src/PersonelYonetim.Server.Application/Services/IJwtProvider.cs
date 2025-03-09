using PersonelYonetim.Server.Domain.Users;

namespace PersonelYonetim.Server.Application.Services;

public interface IJwtProvider
{
    public Task<string> CreateTokenAsync(Appuser user, CancellationToken cancellationToken = default);
}
