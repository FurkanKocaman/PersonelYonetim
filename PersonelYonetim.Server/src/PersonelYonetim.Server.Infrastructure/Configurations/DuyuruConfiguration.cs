﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.Bildirimler;
using PersonelYonetim.Server.Domain.Duyurular;

namespace PersonelYonetim.Server.Infrastructure.Configurations;
internal sealed class DuyuruConfiguration : IEntityTypeConfiguration<Duyuru>
{
    public void Configure(EntityTypeBuilder<Duyuru> builder)
    {
        builder.Property(p => p.AliciTipi).HasConversion(x => x.Value, value => AliciTipiEnum.FromValue(value));
    }
}
