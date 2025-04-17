using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.Izinler;
using PersonelYonetim.Server.Domain.OnaySurecleri;

namespace PersonelYonetim.Server.Infrastructure.Configuration;
internal sealed class TalepDegerlendirmeConfiguration : IEntityTypeConfiguration<TalepDegerlendirme>
{
    public void Configure(EntityTypeBuilder<TalepDegerlendirme> builder)
    {
        builder.Property(p => p.TalepTipi)
            .HasConversion(tip => tip!.Value, value => OnaySurecTuruEnum.FromValue(value));
        builder.Property(p => p.DegerlendirmeDurumu).HasConversion(durum => durum.Value, value => DegerlendirmeDurumEnum.FromValue(value));

        builder.HasOne(p => p.OnaySureciAdimi)
            .WithMany()
            .HasForeignKey(p => p.OnaySureciAdimiId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
