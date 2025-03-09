using PersonelYonetim.Server.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PersonelYonetim.Server.Infrastructure.Configurations;

internal sealed class AppUserConfiguration : IEntityTypeConfiguration<Appuser>
{
    public void Configure(EntityTypeBuilder<Appuser> builder)
    {
        builder.HasIndex(i => i.UserName).IsUnique();
        builder.Property(p => p.FirstName).HasColumnType("varchar(50)");
        builder.Property(P => P.LastName).HasColumnType("varchar(50)");
        builder.Property(P => P.UserName).HasColumnType("varchar(20)");
        builder.Property(P => P.Email).HasColumnType("varchar(MAX)");
    }
}
