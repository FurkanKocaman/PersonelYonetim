using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.CalismaTakvimleri;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PersonelYonetim.Server.Infrastructure.Configurations;
internal class CalismaTakvimConfiguration : IEntityTypeConfiguration<CalismaTakvimi>
{
    public void Configure(EntityTypeBuilder<CalismaTakvimi> builder)
    {
        builder
           .HasMany(p => p.CalismaGunler)
           .WithOne(p => p.CalismaTakvimi)
           .HasForeignKey(p => p.CalismaTakvimId)
           .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(p => p.Sirket)
            .WithMany()
            .HasForeignKey(p => p.SirketId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
