using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.Bordro;

namespace PersonelYonetim.Server.Infrastructure.Configurations;
internal class MaasPusulaConfiguration : IEntityTypeConfiguration<MaasPusula>
{
    public void Configure(EntityTypeBuilder<MaasPusula> builder)
    {
        builder.Property(p => p.Durum).HasConversion(tip => tip.Value, value => MaasPusulaDurumEnum.FromValue(value));

        builder.Property(p => p.GelirVergisiOrani).HasColumnType("decimal(18,5)");
        builder.Property(p => p.SGKPrimiIsciOrani).HasColumnType("decimal(18,5)");
        builder.Property(p => p.IssizlikPrimiIsciOrani).HasColumnType("decimal(18,5)");
        builder.Property(p => p.SGKPrimiIsverenOrani).HasColumnType("decimal(18,5)");
        builder.Property(p => p.IssizlikPrimiIsverenOrani).HasColumnType("decimal(18,5)");
        builder.Property(p => p.DamgaVergisiOrani).HasColumnType("decimal(18,5)");

        builder
            .HasOne(p => p.BordroDonem)
            .WithMany(p => p.MaasPusulalar)
            .HasForeignKey(p => p.BordroDonemId)
            .OnDelete(DeleteBehavior.NoAction);
        builder
             .HasOne(p => p.Personel)
            .WithMany()
            .HasForeignKey(p => p.PersonelId)
            .OnDelete(DeleteBehavior.NoAction);
    
    
    }
}
