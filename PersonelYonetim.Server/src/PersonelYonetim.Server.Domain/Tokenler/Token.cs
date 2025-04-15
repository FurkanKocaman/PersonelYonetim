using Ardalis.SmartEnum;

namespace PersonelYonetim.Server.Domain.Tokenler;

public sealed class Token
{
    public Token()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }
    public Guid UserId { get; set; } = default!;
    public string TokenString { get; set; } = default!;
    public DateTimeOffset Expires { get; set; } = default!;
    public TokenTypeEnum TokenType { get; set; } = default!;
    public Guid TenantId { get; set; }
}

public sealed class TokenTypeEnum : SmartEnum<TokenTypeEnum>
{
    public static readonly TokenTypeEnum EmailVerification = new("EmailVerification", 0);
    public static readonly TokenTypeEnum PhoneVerification = new("2FA",1);
    public static readonly TokenTypeEnum FaxVerification = new("RefreshToken", 2);

    public TokenTypeEnum(string name, int value) : base(name, value)
    {
    }
}

