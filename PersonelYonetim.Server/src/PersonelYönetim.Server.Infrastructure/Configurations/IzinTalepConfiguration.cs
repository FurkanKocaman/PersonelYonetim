﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYönetim.Server.Domain.IzinTalepleri;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonelYönetim.Server.Infrastructure.Configurations;

internal sealed class IzinTalepConfiguration : IEntityTypeConfiguration<IzinTalep>
{
    public void Configure(EntityTypeBuilder<IzinTalep> builder)
    {
        builder.Property(p => p.IzinTipi).HasConversion(tip => tip.Value, value => IzinTipiEnum.FromValue(value));
        builder.Property(p => p.OnayDurumu).HasConversion(durum => durum.Value, value => OnayDurumEnum.FromValue(value));
    }
}
