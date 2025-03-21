using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.IzinTalepler;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Roller;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Domain.Sirketler;
using PersonelYonetim.Server.Domain.Subeler;
using PersonelYonetim.Server.Domain.Tokenler;
using PersonelYonetim.Server.Domain.Users;
using System.Security.Claims;

namespace PersonelYonetim.Server.Infrastructure.Context;

internal sealed class ApplicationDbContext: IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>, IUnitOfWork
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public DbSet<Personel> Personeller { get; set; }    
    public DbSet<Sirket> Sirketler { get; set; }
    public DbSet<Sube> Subeler { get; set; }
    public DbSet<Departman> Departmanlar { get; set; }
    public DbSet<Pozisyon> Pozisyonlar { get; set; }
    public DbSet<IzinTalep> IzinTalepleri { get; set; }
    public DbSet<IdentityRoleClaim<Guid>> IdentityRoleClaims { get; set; }
    public DbSet<Token> Tokenler { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        //modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();

        modelBuilder.Entity<AppUserRole>()
            .HasKey(p => new {p.UserId, p.RoleId, p.SirketId});

        modelBuilder.Entity<IzinTalep>()
            .HasOne(p => p.Degerlendiren)
            .WithMany()
            .HasForeignKey(p => p.DegerlendirenId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<IzinTalep>()
            .HasOne(p => p.Personel)
            .WithMany()
            .HasForeignKey(p => p.PersonelId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<PersonelAtama>()
            .HasOne(p => p.Personel)
            .WithMany(p => p.PersonelAtamalar)
            .HasForeignKey(p => p.PersonelId);

        modelBuilder.Entity<Personel>()
            .HasMany(p => p.PersonelAtamalar)
            .WithOne(p => p.Personel)
            .HasForeignKey(p => p.PersonelId);

        modelBuilder.Entity<PersonelAtama>()
           .HasOne(pa => pa.Departman)
           .WithMany()
           .HasForeignKey(pa => pa.DepartmanId)
           .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<PersonelAtama>()
            .HasOne(pa => pa.Pozisyon)
            .WithMany()
            .HasForeignKey(pa => pa.PozisyonId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<PersonelAtama>()
            .HasOne(pa => pa.Sube)
            .WithMany()
            .HasForeignKey(pa => pa.SubeId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<PersonelAtama>()
            .HasOne(pa => pa.Sirket)
            .WithMany()
            .HasForeignKey(pa => pa.SirketId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Personel>()
            .HasOne(p => p.Yonetici)
            .WithMany()
            .HasForeignKey(p => p.YoneticiId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Personel>()
            .HasOne(p => p.User)
            .WithOne()
            .HasForeignKey<Personel>(p => p.UserId);

        modelBuilder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("RoleClaims");

        modelBuilder.Entity<Sube>()
            .HasOne(p => p.Sirket)
            .WithMany(s => s.Subeler)
            .HasForeignKey(s => s.SirketId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Departman>()
            .HasOne(p => p.Sube)
            .WithMany(s => s.Departmanlar)
            .HasForeignKey(s => s.SubeId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Pozisyon>()
            .HasOne(p => p.Sirket)
            .WithMany()
            .HasForeignKey(p =>p.SirketId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Departman>()
            .HasOne(p => p.Sirket)
            .WithMany()
            .HasForeignKey(p => p.SirketId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();
        var userEntries = ChangeTracker.Entries<AppUser>();

        var userId = GetCurrentUserId();


            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(p => p.CreatedAt).CurrentValue = DateTimeOffset.Now;
                    if(userId.HasValue)
                        entry.Property(p => p.CreateUserId).CurrentValue = userId.Value;
                }
                if (entry.State == EntityState.Modified)
                {
                    if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                    {
                        entry.Property(p => p.DeleteAt).CurrentValue = DateTimeOffset.Now;
                        if (userId.HasValue)
                            entry.Property(p => p.DeleteUserId).CurrentValue = userId.Value;
                    }
                    else
                    {
                        entry.Property(p => p.UpdateAt).CurrentValue = DateTimeOffset.Now;
                        if (userId.HasValue)
                            entry.Property(p => p.UpdateUserId).CurrentValue = userId.Value;
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
                    if (userId.HasValue)
                        entry.Property(p => p.CreateUserId).CurrentValue = userId.Value;
                }
                if (entry.State == EntityState.Modified)
                {
                    if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                    {
                        entry.Property(p => p.DeleteAt).CurrentValue = DateTimeOffset.Now;
                        if (userId.HasValue)
                            entry.Property(p => p.DeleteUserId).CurrentValue = userId.Value;
                    }
                    else
                    {
                        entry.Property(p => p.UpdateAt).CurrentValue = DateTimeOffset.Now;
                        if (userId.HasValue)
                            entry.Property(p => p.UpdateUserId).CurrentValue = userId.Value;
                    }
                }
                if (entry.State == EntityState.Deleted)
                {
                    throw new ArgumentException("Cannot delete directly from Db");
                }
            }
        
       

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private Guid? GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return userId;
            }
        }
        return null;
    }
}

