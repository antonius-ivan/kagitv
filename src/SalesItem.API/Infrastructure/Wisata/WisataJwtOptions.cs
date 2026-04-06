namespace ICLAco.SalesItem.API.Infrastructure.Wisata;

public class WisataJwtOptions
{
    public const string SectionName = "WisataJwt";

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string SigningKey { get; set; } = string.Empty;

    public int AccessTokenMinutes { get; set; } = 1;

    public int RefreshTokenMinutes { get; set; } = 3;
}