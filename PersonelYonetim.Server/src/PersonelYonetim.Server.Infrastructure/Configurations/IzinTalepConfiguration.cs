using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.IzinTalepler;

namespace PersonelYonetim.Server.Infrastructure.Configurations;

internal sealed class IzinTalepConfiguration : IEntityTypeConfiguration<IzinTalep>
{
    public void Configure(EntityTypeBuilder<IzinTalep> builder)
    {
        builder.Property(p => p.IzinTipi).HasConversion(tip => tip.Value, value => IzinTipiEnum.FromValue(value));
        builder.Property(p => p.DegerlendirmeDurumu).HasConversion(durum => durum.Value, value => DegerlendirmeDurumEnum.FromValue(value));
    }
}
