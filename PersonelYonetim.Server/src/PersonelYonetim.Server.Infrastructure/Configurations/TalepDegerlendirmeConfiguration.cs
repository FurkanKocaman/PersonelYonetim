using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.OnaySurecleri;

namespace PersonelYonetim.Server.Infrastructure.Configurations;
internal sealed class TalepDegerlendirmeConfiguration : IEntityTypeConfiguration<TalepDegerlendirme>
{
    public void Configure(EntityTypeBuilder<TalepDegerlendirme> builder)
    {
        builder.Property(p => p.TalepTuru)
            .HasConversion(tip => tip!.Value, value => OnaySurecTuruEnum.FromValue(value));
        builder.Property(p => p.DegerlendirmeDurumu).HasConversion(durum => durum.Value, value => DegerlendirmeDurumEnum.FromValue(value));
    }
}
