using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Rols;
using System.Reflection.Emit;

namespace PersonelYonetim.Server.Infrastructure.Configurations;

internal sealed class PersonelAtamalarConfiguration : IEntityTypeConfiguration<PersonelAtama>
{
    public void Configure(EntityTypeBuilder<PersonelAtama> builder)
    {
        builder.Property(p => p.RolTipi).HasConversion(tip => tip!.Value, value => RolTipiEnum.FromValue(value));
        builder.Property(p => p.SozlesmeTuru).HasConversion(tip => tip.Value, value => SozlesmeTuruEnum.FromValue(value));
        builder.Property(p => p.CalismaSekli).HasConversion(tip => tip.Value, value => CalismaSekliEnum.FromValue(value));

        builder
         .HasOne(p => p.Personel)
         .WithMany(p => p.PersonelAtamalar)
         .HasForeignKey(p => p.PersonelId)
         .OnDelete(DeleteBehavior.NoAction);

        builder
           .HasOne(pa => pa.Departman)
           .WithMany()
           .HasForeignKey(pa => pa.DepartmanId)
           .OnDelete(DeleteBehavior.NoAction);
        builder
             .HasOne(pa => pa.Pozisyon)
            .WithMany()
            .HasForeignKey(pa => pa.PozisyonId)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasOne(pa => pa.Sube)
            .WithMany()
            .HasForeignKey(pa => pa.SubeId)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasOne(pa => pa.Sirket)
            .WithMany()
            .HasForeignKey(pa => pa.SirketId)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasOne(p => p.Yonetici)
            .WithMany()
            .HasForeignKey(p => p.YoneticiId)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasOne(p => p.CalismaTakvimi)
            .WithMany(p => p.Personeller)
            .HasForeignKey(p => p.CalismaTakvimId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
