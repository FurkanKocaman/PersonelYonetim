using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.Abstractions;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.Bordro;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using PersonelYonetim.Server.Domain.Duyurular;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.KurumsalBirimler;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.PersonelDetaylar;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;
using PersonelYonetim.Server.Domain.Personeller;
using PersonelYonetim.Server.Domain.Pozisyonlar;
using PersonelYonetim.Server.Domain.Roller;
using PersonelYonetim.Server.Domain.Rols;
using PersonelYonetim.Server.Domain.TakvimEtkinlikler;
using PersonelYonetim.Server.Domain.Tenants;
using PersonelYonetim.Server.Domain.Tokenler;
using PersonelYonetim.Server.Domain.Users;
using PersonelYonetim.Server.Domain.ZamanYonetimler;
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
    public DbSet<PersonelDetay> PersonelDetaylar { get; set; }

    public DbSet<IzinTalep> IzinTalepleri { get; set; }
    public DbSet<IzinTur> IzinTurleri { get; set; }
    public DbSet<IzinKural> IzinKuralları { get; set; }

    public DbSet<IdentityRoleClaim<Guid>> IdentityRoleClaims { get; set; }
    public DbSet<CalismaTakvimi> CalismaTakvimleri { get; set; }
    public DbSet<CalismaGun> CalismaGunleri { get; set; }
    public DbSet<TakvimEtkinlik> TakvimEtkinlikler { get; set; }
    public DbSet<Bildirim> Bildirimler { get; set; }
    public DbSet<PersonelBildirim> PersonelBildirimler { get; set; }
    public DbSet<Duyuru> Duyurular { get; set; }
    public DbSet<OnaySurec> OnaySurecleri { get; set; }
    public DbSet<OnaySureciAdimi> OnaySureciAdimlari { get; set; }
    public DbSet<CalismaCizelge> CalismaCizelgeleri { get; set; }
    public DbSet<GunlukCalisma> GunlukCalismalar { get; set; }
    public DbSet<CalismaPeriyodu> CalismaPeriyotları { get; set; }
    public DbSet<MolaPeriyodu> MolaPeriyotları { get; set; }
    public DbSet<IzinPeriyodu> IzinPeriyotları { get; set; }
    public DbSet<FazlaMesaiPeriyodu> FazlaMesaiPeriyotları { get; set; }
    public DbSet<Token> Tokenler { get; set; }

    public DbSet<BordroDonem> BordroDonemleri { get; set; }
    public DbSet<MaasPusula> MaasPusulalar { get; set; }

    public DbSet<KurumsalBirimTipi> KurumsalBirimTipleri { get; set; }
    public DbSet<KurumsalBirim> KurumsalBirimler { get; set; }

    public DbSet<PersonelGorevlendirme> PersonelGorevlendirmeler { get; set; }
    public DbSet<GorevlendirmeIzinKurali> GorevlendirmeIzinKurallari { get; set; }
    public DbSet<GorevlendirmeRolu> GorevlendirmeRolleri { get; set; }
    public DbSet<Pozisyon> Pozisyonlar { get; set; }

    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Ignore<IdentityUserClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();

        modelBuilder.Entity<PersonelDetay>()
            .HasOne(p => p.Personel)
            .WithOne(p => p.PersonelDetay)
            .HasForeignKey<PersonelDetay>(p => p.PersonelId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PersonelGorevlendirme>()
            .HasOne(p => p.Personel)
            .WithMany(p => p.PersonelGorevlendirmeler)
            .HasForeignKey(p => p.PersonelId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<PersonelGorevlendirme>()
            .HasOne(p => p.KurumsalBirim)
            .WithMany(p => p.Gorevlendirmeler)
            .HasForeignKey(p => p.KurumsalBirimId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<PersonelGorevlendirme>()
            .HasOne(p => p.Pozisyon)
            .WithMany()
            .HasForeignKey(p => p.PozisyonId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<PersonelGorevlendirme>()
            .HasOne(p => p.RaporlananGorevlendirme)
            .WithMany()
            .HasForeignKey(p => p.RaporlananGorevlendirmeId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<GorevlendirmeRolu>()
            .HasOne(p => p.PersonelGorevlendirme)
            .WithMany(p => p.GorevlendirmeRolleri)
            .HasForeignKey(p => p.PersonelGorevlendirmeId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<GorevlendirmeRolu>()
            .HasOne(p => p.Rol)
            .WithMany()
            .HasForeignKey(p => p.RolId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<GorevlendirmeIzinKurali>()
            .HasOne(p => p.PersonelGorevlendirme)
            .WithOne(p => p.GorevlendirmeIzinKurali)
            .HasForeignKey<GorevlendirmeIzinKurali>(p => p.PersonelGorevlendirmeId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<GorevlendirmeIzinKurali>()
            .HasOne(p => p.IzinKural)
            .WithMany(p => p.GorevlendirmeIzinKurallar)
            .HasForeignKey(p => p.IzinKuralId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<GorevlendirmeIzinKurali>()
            .HasOne(p => p.OzelOnaySurec)
            .WithMany()
            .HasForeignKey(p => p.OzelOnaySurecId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CalismaCizelge>()
            .HasOne(p => p.Personel)
            .WithMany()
            .HasForeignKey(p => p.PersonelId)
            .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<GunlukCalisma>()
           .HasOne(p => p.Personel)
           .WithMany()
           .HasForeignKey(p => p.PersonelId)
           .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<GunlukCalisma>()
           .HasOne(p => p.CalismaCizelgesi)
           .WithMany(p => p.GunlukCalismalar)
           .HasForeignKey(p => p.CalismaCizelgesiId)
           .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CalismaPeriyodu>()
           .HasOne(p => p.GunlukCalisma)
           .WithMany(p => p.CalismaPeriyotlari)
           .HasForeignKey(p => p.GunlukCalismaId)
           .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<MolaPeriyodu>()
          .HasOne(p => p.GunlukCalisma)
          .WithMany(p => p.MolaPeriyotlari)
          .HasForeignKey(p => p.GunlukCalismaId)
          .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<FazlaMesaiPeriyodu>()
          .HasOne(p => p.GunlukCalisma)
          .WithMany(p => p.FazlaMesaiPeriyotlari)
          .HasForeignKey(p => p.GunlukCalismaId)
          .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<IzinPeriyodu>()
          .HasOne(p => p.GunlukCalisma)
          .WithMany(p => p.IzinPeriyotlari)
          .HasForeignKey(p => p.GunlukCalismaId)
          .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<OnaySureciAdimi>()
            .HasOne(p => p.OnaySurec)
            .WithMany(p => p.OnayAdimlari)
            .HasForeignKey(p => p.OnaySurecId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<OnaySureciAdimi>()
            .HasOne(p => p.Personel)
            .WithMany()
            .HasForeignKey(p => p.PersonelId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<PersonelBildirim>()
            .HasOne(p => p.Bildirim)
            .WithMany(p => p.PersonelBildirimler)
            .HasForeignKey(p => p.BildirimId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<PersonelBildirim>()
            .HasOne(p => p.Personel)
            .WithMany(p => p.PersonelBildirimler)
            .HasForeignKey(p => p.PersonelId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<AppUserRole>()
            .HasKey(p => new {p.UserId, p.RoleId});

        modelBuilder.Entity<TalepDegerlendirme>()
            .HasOne(p => p.Degerlendiren)
            .WithMany()
            .HasForeignKey(p => p.DegerlendirenId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<IzinTalep>()
            .HasOne(p => p.Personel)
            .WithMany()
            .HasForeignKey(p => p.PersonelId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<IzinTalep>()
            .HasOne(p => p.IzinTur)
            .WithMany()
            .HasForeignKey(p => p.IzinTurId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<IzinTalep>()
            .Navigation(e => e.DegerlendirmeAdimlari)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        modelBuilder.Entity<Personel>()
            .HasOne(p => p.User)
            .WithOne()
            .HasForeignKey<Personel>(p => p.UserId);

        modelBuilder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("RoleClaims");

        modelBuilder.Entity<KazancBilesen>()
            .HasOne(p => p.MaasPusula)
            .WithMany(p => p.KazancBilesenleri)
            .HasForeignKey(p => p.MaasPusulaId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<KesintiBilesen>()
            .HasOne(p => p.MaasPusula)
            .WithMany(p => p.KesintiBilesenleri)
            .HasForeignKey(p => p.MaasPusulaId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<IzinKural>()
            .HasOne(p => p.VarsayilanOnaySurec)
            .WithMany()
            .HasForeignKey(p => p.VarsayilanOnaySurecId)
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
                        if (entry.Property(p => p.IsActive).CurrentValue == false)
                        {
                            entry.Property(p => p.InActiveDate).CurrentValue = DateTimeOffset.Now;
                        }
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

