using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.PersonelGorevlendirmeler;

namespace PersonelYonetim.Server.Infrastructure.Configurations;
internal sealed class PersonelGorevlendirmeConfiguration : IEntityTypeConfiguration<PersonelGorevlendirme>
{
    public void Configure(EntityTypeBuilder<PersonelGorevlendirme> builder)
    {
        builder.Property(p => p.IseGirisTarihi).HasColumnType("datetimeoffset").IsRequired();
        builder.Property(p => p.PozisyonBaslangicTarihi).HasColumnType("datetimeoffset").IsRequired();
        builder.Property(p => p.PozisyonBitisTarihi).HasColumnType("datetimeoffset");
        builder.Property(p => p.IstenCikisTarihi).HasColumnType("datetimeoffset");
        builder.Property(p => p.GorevlendirmeTipi).HasConversion<string>();
        builder.Property(p => p.CalismaSekli).HasConversion<string>();
        builder.Property(p => p.BrutUcret).HasColumnType("decimal(18,2)");

        builder.Property(p => p.GorevlendirmeTipi).HasConversion(tip => tip!.Value, value => GorevlendirmeTipiEnum.FromValue(value));
        builder.Property(p => p.CalismaSekli).HasConversion(tip => tip!.Value, value => CalismaSekliEnum.FromValue(value));
    }
}
