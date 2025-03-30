using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace PersonelYonetim.Server.Domain.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    DbContext GetDbContext();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(IDbContextTransaction transaction);
    Task RollbackTransactionAsync(IDbContextTransaction transaction);
    IDbContextTransaction BeginTransaction();
}
