using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.PersonelAtamalar;

namespace PersonelYonetim.Server.Infrastructure.Configurations;
internal class PersonelIzinKuralConfiguration : IEntityTypeConfiguration<PersonelIzinKural>
{
    public void Configure(EntityTypeBuilder<PersonelIzinKural> builder)
    {
        builder
            .HasOne(p => p.Personel)
            .WithMany()
            .HasForeignKey(p => p.PersonelId)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasOne(p => p.IzinKural)
            .WithMany(p => p.PersonelIzinKurallar)
            .HasForeignKey(p => p.IzinKuralId)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasOne(p => p.OnaySurec)
            .WithMany()
            .HasForeignKey(p => p.OnaySurecId)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasOne(p => p.Sirket)
            .WithMany()
            .HasForeignKey(p => p.SirketId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
