using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PersonelYonetim.Server.Domain.Personeller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonelYonetim.Server.Domain.Sirketler;

namespace PersonelYonetim.Server.Infrastructure.Configurations;
internal sealed class SirketConfiguration : IEntityTypeConfiguration<Sirket>
{
    public void Configure(EntityTypeBuilder<Sirket> builder)
    {
        builder.Property(p => p.Ad).HasColumnType("varchar(50)").IsRequired();

        builder.OwnsOne(p => p.Adres, builder =>
        {
            builder.Property(a => a.Ulke).HasColumnName("Ülke");
            builder.Property(a => a.Sehir).HasColumnName("Şehir");
            builder.Property(a => a.Ilce).HasColumnName("İlçe");
            builder.Property(a => a.TamAdres).HasColumnName("TamAdres");
        });

        builder.OwnsOne(p => p.Iletisim, builder =>
        {
            builder.Property(i => i.Eposta).HasColumnName("Eposta");
            builder.Property(i => i.Telefon).HasColumnName("Telefon");
        });
    }
}
