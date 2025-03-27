using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.PersonelAtamalar;
using PersonelYonetim.Server.Domain.Rols;

namespace PersonelYonetim.Server.Infrastructure.Configurations;

internal sealed class PersonelAtamalarConfiguration : IEntityTypeConfiguration<PersonelAtama>
{
    public void Configure(EntityTypeBuilder<PersonelAtama> builder)
    {
        builder.Property(p => p.RolTipi).HasConversion(tip => tip!.Value, value => RolTipiEnum.FromValue(value));
        builder.Property(p => p.SozlesmeTuru).HasConversion(tip => tip.Value, value => SozlesmeTuruEnum.FromValue(value));
    }
}
