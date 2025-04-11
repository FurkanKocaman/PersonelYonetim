using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.Bordro;

namespace PersonelYonetim.Server.Infrastructure.Configurations;
internal class MaasPusulaConfiguration : IEntityTypeConfiguration<MaasPusula>
{
    public void Configure(EntityTypeBuilder<MaasPusula> builder)
    {
        builder
            .HasOne(p => p.BordroDonem)
            .WithMany(p => p.MaasPusulalar)
            .HasForeignKey(p => p.BordroDonemId)
            .OnDelete(DeleteBehavior.NoAction);
        builder
             .HasOne(p => p.Personel)
            .WithMany()
            .HasForeignKey(p => p.PersonelId)
            .OnDelete(DeleteBehavior.NoAction);
        builder
            .HasOne(p => p.Sirket)
            .WithMany()
            .HasForeignKey(p => p.SirketId)
            .OnDelete(DeleteBehavior.NoAction);
    
    }
}
