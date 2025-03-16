using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.IzinTalepler;
using PersonelYonetim.Server.Domain.PersonelDepartmanlar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Domain.Subeler;
using PersonelYonetim.Server.Domain.Tokenler;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Infrastructure.Context;

internal sealed class ApplicationDbContext: IdentityDbContext<AppUser, AppRole, Guid>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Personel> Personeller { get; set; }    
    public DbSet<Departman> Departmanlar { get; set; }
    public DbSet<Pozisyon> Pozisyonlar { get; set; }
    public DbSet<IzinTalep> IzinTalepleri { get; set; }
    public DbSet<IdentityUserClaim<Guid>> IdentityUserClaims { get; set; }
    public DbSet<Token> Tokenler { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
        //modelBuilder.Ignore<IdentityUserClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();

        modelBuilder.Entity<IdentityUserRole<Guid>>()
            .HasKey(i => new { i.UserId, i.RoleId });

        modelBuilder.Entity<PersonelDepartman>()
            .HasOne(p => p.Personel)
            .WithMany(p => p.PersonelDepartmanlar)
            .HasForeignKey(p => p.PersonelId);

        modelBuilder.Entity<PersonelDepartman>()
            .HasOne(p => p.Departman)
            .WithMany(p => p.PersonelDepartmanlar)
            .HasForeignKey(p => p.DepartmanId)
            .OnDelete(DeleteBehavior.NoAction);
       
        modelBuilder.Entity<PersonelDepartman>()
            .HasOne(p => p.Pozisyon)
            .WithMany(p => p.PersonelDepartmanlar)
            .HasForeignKey(p => p.PozisyonId);

        modelBuilder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("UserClaims");

        modelBuilder.Entity<Sube>()
            .HasOne(p => p.Sirket)
            .WithMany(s => s.Subeler)
            .HasForeignKey(s => s.SirketId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Departman>()
            .HasOne(p => p.Sube)
            .WithMany(s => s.Departmanlar)
            .HasForeignKey(s => s.SubeId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Pozisyon>()
            .HasOne(p => p.Departman)
            .WithMany(s => s.Pozisyonlar)
            .HasForeignKey(s => s.DepartmanId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();
        var userEntries = ChangeTracker.Entries<AppUser>();

        HttpContextAccessor httpContextAccessor = new();
        //string? userIdString = httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;
        
        string userIdString = "3023f17b-df7f-4720-83b1-5334ec87cd13";
        if (userIdString != null)
        {
            Guid userId = Guid.Parse(userIdString);
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(p => p.CreatedAt).CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.CreateUserId).CurrentValue = userId;
                }
                if (entry.State == EntityState.Modified)
                {
                    if (entry.Property(p => p.IsDeleted).CurrentValue == true)
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
                if (entry.State == EntityState.Deleted)
                {
                    throw new ArgumentException("Cannot delete directly from Db");
                }
            }
            foreach (var entry in userEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(p => p.CreatedAt).CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.CreateUserId).CurrentValue = userId;
                }
                if (entry.State == EntityState.Modified)
                {
                    if (entry.Property(p => p.IsDeleted).CurrentValue == true)
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
                if (entry.State == EntityState.Deleted)
                {
                    throw new ArgumentException("Cannot delete directly from Db");
                }
            }
        }
       

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}

