using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PersonelYonetim.Server.Infrastructure.Configurations;
internal class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.Property(p => p.SGKPrimIsciKesintiOrani).HasColumnType("decimal(18,5)");
        builder.Property(p => p.SGKIssizlikPrimIsciKesintiOrani).HasColumnType("decimal(18,5)");
        builder.Property(p => p.SGKPrimIsverenKesintiOrani).HasColumnType("decimal(18,5)");
        builder.Property(p => p.SGKIssizlikPrimIsverenKesintiOrani).HasColumnType("decimal(18,5)");
        builder.Property(p => p.DamgaVergisiOrani).HasColumnType("decimal(18,5)");
    }
}
