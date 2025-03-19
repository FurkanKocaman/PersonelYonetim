using GenericRepository;
using PersonelYonetim.Server.Domain.Tokenler;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;

internal sealed class TokenRepository : Repository<Token, ApplicationDbContext>, ITokenRepository
{
    public TokenRepository(ApplicationDbContext context) : base(context)
    {
    }
}
