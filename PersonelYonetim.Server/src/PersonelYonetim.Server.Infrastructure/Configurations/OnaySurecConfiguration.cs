using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.OnaySurecleri;


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
        builder.Property(p => p.OnaylayiciTanimTipi).HasConversion(tip => tip.Value, value => OnaylayiciTanimTipiEnum.FromValue(value));

        builder.HasOne(p => p.Role)
            .WithMany()
            .HasForeignKey(p => p.RolId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(p => p.HedefKurumsalBirim)
            .WithMany()
            .HasForeignKey(p => p.HedefKurumsalBirimId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
