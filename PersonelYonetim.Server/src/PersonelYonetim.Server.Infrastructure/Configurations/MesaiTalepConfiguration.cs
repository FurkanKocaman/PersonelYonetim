using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.Mesailer;

namespace PersonelYonetim.Server.Infrastructure.Configurations;
public class MesaiTalepConfiguration : IEntityTypeConfiguration<MesaiTalep>
{
    public void Configure(EntityTypeBuilder<MesaiTalep> builder)
    {
        builder.Property(p => p.MesaiDegerlendirmeDurum).HasConversion(tip => tip!.Value, value => MesaiDegerlendirmeDurumEnum.FromValue(value));
    }
}
