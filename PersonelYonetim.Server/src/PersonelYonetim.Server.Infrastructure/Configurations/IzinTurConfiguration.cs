using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.Izinler;

namespace PersonelYonetim.Server.Infrastructure.Configurations;

internal sealed class IzinTurConfiguration : IEntityTypeConfiguration<IzinTur>
{
    public void Configure(EntityTypeBuilder<IzinTur> builder)
    {
        builder.Property(p => p.LimitTipi).HasConversion(tip => tip!.Value, value => LimitTipiEnum.FromValue(value));
        builder.Property(p => p.EksiBakiyeHakkı).HasConversion(tip => tip!.Value, value => EksiBakiyeHakkıEnum.FromValue(value));
        builder.Property(p => p.HakEdis).HasConversion(tip => tip!.Value, value => HakEdisEnum.FromValue(value));
        builder.Property(p => p.DevretmeTipi).HasConversion(tip => tip!.Value, value => DevretmeTipiEnum.FromValue(value));
    }
}
