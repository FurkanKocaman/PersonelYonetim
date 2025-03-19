using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.Tokenler;

namespace PersonelYonetim.Server.Infrastructure.Configurations;

internal sealed class TokenConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.Property(p => p.TokenType).HasConversion(tip => tip.Value, value => TokenTypeEnum.FromValue(value));
    }
}
