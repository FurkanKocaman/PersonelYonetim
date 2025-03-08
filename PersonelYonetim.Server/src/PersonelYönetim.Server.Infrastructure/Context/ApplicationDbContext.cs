using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonelYönetim.Server.Domain.Abstractions;
using PersonelYönetim.Server.Domain.Departman;
using PersonelYönetim.Server.Domain.IzinTalepleri;
using PersonelYönetim.Server.Domain.Personeller;
using PersonelYönetim.Server.Domain.Pozisyon;
using PersonelYönetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYönetim.Server.Infrastructure.Context;

internal sealed class ApplicationDbContext: IdentityDbContext<Appuser, IdentityRole<Guid>,Guid>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Personel> Personeller { get; set; }
    public DbSet<Departman> Departmanlar { get; set; }
    public DbSet<Pozisyon> Pozisyonlar { get; set; }
    public DbSet<IzinTalep> IzinTalepleri { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();
        modelBuilder.Ignore<IdentityUserRole<Guid>>();

    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();

        HttpContextAccessor httpContextAccessor = new();
        string userIdString = httpContextAccessor.HttpContext!.User.Claims.First(p => p.Type == ClaimTypes.NameIdentifier).Value;
        //First start string userIdString = "3023f17b-df7f-4720-83b1-5334ec87cd13";
        Guid userId = Guid.Parse(userIdString);

        foreach (var entry in entries)
        {
            if(entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreatedAt).CurrentValue = DateTimeOffset.Now;
                entry.Property(p => p.CreateUserId).CurrentValue = userId;
            }
            if (entry.State == EntityState.Modified)
            {
                if(entry.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entry.Property(p => p.DeleteAt).CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.DeleteUserId).CurrentValue = userId;
                }
                else
                {
                    entry.Property(p => p.UpdateAt).CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.UpdateUserId).CurrentValue = userId;
                }
               
            }
            if(entry.State == EntityState.Deleted)
            {
                throw new ArgumentException("Cannot delete directly from Db");
            }
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}

