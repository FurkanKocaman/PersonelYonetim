using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.Bildirimler;

namespace PersonelYonetim.Server.Infrastructure.Configurations;
internal sealed class BildirimConfigurations : IEntityTypeConfiguration<Bildirim>
{
    public void Configure(EntityTypeBuilder<Bildirim> builder)
    {
        builder.Property(p => p.BildirimTipi).HasConversion(x => x.Value, value => BildirimTipiEnum.FromValue(value));
        builder.Property(p => p.AliciTipi).HasConversion(x => x.Value, value => AliciTipiEnum.FromValue(value));
    }
}
