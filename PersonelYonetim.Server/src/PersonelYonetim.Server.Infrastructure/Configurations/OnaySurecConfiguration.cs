using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.OnaySurecleri;
using PersonelYonetim.Server.Domain.Rols;

namespace PersonelYonetim.Server.Infrastructure.Configurations;
internal class OnaySurecConfiguration : IEntityTypeConfiguration<OnaySurec>
{
    public void Configure(EntityTypeBuilder<OnaySurec> builder)
    {
        builder.Property(p => p.OnaySurecTuruEnum).HasConversion(tip => tip.Value, value => OnaySurecTuruEnum.FromValue(value));
    }
}
internal class OnaySurecAdimConfiguration : IEntityTypeConfiguration<OnaySureciAdimi>
{
    public void Configure(EntityTypeBuilder<OnaySureciAdimi> builder)
    {
        builder.Property(p => p.Rol).HasConversion(tip => tip!.Value, value => RolTipiEnum.FromValue(value));
    }
}
