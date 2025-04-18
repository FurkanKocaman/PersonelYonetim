﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.Personeller;

namespace PersonelYonetim.Server.Infrastructure.Configurations;

internal sealed class PersonelConfiguration : IEntityTypeConfiguration<Personel>
{
    public void Configure(EntityTypeBuilder<Personel> builder)
    {
        builder.Property(p => p.Ad).HasColumnType("varchar(50)").IsRequired();
        builder.Property(p => p.Soyad).HasColumnType("varchar(50)").IsRequired();

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
           builder.HasIndex(i => i.Eposta).IsUnique();
        });
    }
}
