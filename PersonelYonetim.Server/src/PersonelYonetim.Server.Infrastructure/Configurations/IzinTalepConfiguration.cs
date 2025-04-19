using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.Izinler;

namespace PersonelYonetim.Server.Infrastructure.Configurations;

internal sealed class IzinTalepConfiguration : IEntityTypeConfiguration<IzinTalep>
{
    public void Configure(EntityTypeBuilder<IzinTalep> builder)
    {
       
    }
}
