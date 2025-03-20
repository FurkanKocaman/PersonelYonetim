using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.PersonelAtamalar;

namespace PersonelYonetim.Server.Infrastructure.Configurations;

internal sealed class PersonelAtamalarConfiguration : IEntityTypeConfiguration<PersonelAtama>
{
    public void Configure(EntityTypeBuilder<PersonelAtama> builder)
    {
        builder.Property(p => p.YoneticiTipi).HasConversion(tip => tip!.Value, value => YoneticiTipiEnum.FromValue(value));
        builder.Property(p => p.SozlesmeTuru).HasConversion(tip => tip.Value, value => SozlesmeTuruEnum.FromValue(value));
        builder.Property(p => p.CalismaSekli).HasConversion(tip => tip.Value, value => CalismaSekliEnum.FromValue(value));
    }
}
