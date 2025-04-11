using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PersonelYonetim.Server.Domain.UnitOfWork;
using PersonelYonetim.Server.Infrastructure.Context;

namespace PersonelYonetim.Server.Infrastructure.Repositories;

class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }
    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public IDbContextTransaction BeginTransaction()
    {
        return _context.Database.BeginTransaction();
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        await transaction.CommitAsync();
    }


    public async Task RollbackTransactionAsync(IDbContextTransaction transaction)
    {
        await transaction.RollbackAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public DbContext GetDbContext()
    {
        return _context;
    }
    //public async Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> action)
    //{
    //    await using var transaction = await BeginTransaction();
    //    try
    //    {
    //        var result = await action();
    //        await CommitTransactionAsync();
    //        return result;
    //    }
    //    catch
    //    {
    //        await RollbackTransactionAsync();
    //        throw;
    //    }
    //}
}
