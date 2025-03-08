using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYönetim.Server.Domain.Personeller;

namespace PersonelYönetim.Server.Infrastructure.Configurations;

internal sealed class PersonelConfiguration : IEntityTypeConfiguration<Personel>
{
    public void Configure(EntityTypeBuilder<Personel> builder)
    {
        builder.Property(p => p.Ad).HasColumnType("varchar(50)").IsRequired();
        builder.Property(p => p.Soyad).HasColumnType("varchar(50)").IsRequired();

        builder.OwnsOne(p => p.Iletisim, builder =>
        {
           builder.Property(i => i.Eposta).HasColumnName("Eposta");
           builder.Property(i => i.Telefon).HasColumnName("Telefon");
        });
    }
}
