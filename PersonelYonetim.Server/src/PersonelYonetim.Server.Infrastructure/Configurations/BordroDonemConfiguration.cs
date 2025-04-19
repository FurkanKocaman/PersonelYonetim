using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.Bordro;

namespace PersonelYonetim.Server.Infrastructure.Configurations;
internal class BordroDonemConfiguration : IEntityTypeConfiguration<BordroDonem>
{
    public void Configure(EntityTypeBuilder<BordroDonem> builder)
    {
        builder.Property(p => p.BordroDonemDurum).HasConversion(durum => durum.Value, value => BordroDonemDurumEnum.FromValue(value));
    }
}
