using ICLAco.SalesItem.API.Model.Wisata;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ICLAco.SalesItem.API.Infrastructure.Wisata;

public class WisataAuthService(
    WisataDbContext dbContext,
    IPasswordHasher<WisataUser> passwordHasher,
    IOptions<WisataJwtOptions> jwtOptions)
{
    private readonly WisataJwtOptions options = jwtOptions.Value;

    public async Task<WisataTokenResponse?> LoginAsync(string username, string password, CancellationToken cancellationToken)
    {
        var normalizedUsername = username.Trim();
        if (string.IsNullOrWhiteSpace(normalizedUsername) || string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        var user = await dbContext.Users
            .Include(item => item.RefreshTokens)
            .SingleOrDefaultAsync(item => item.Username == normalizedUsername, cancellationToken);

        if (user is null)
        {
            return null;
        }

        var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return null;
        }

        dbContext.UserRefreshTokens.RemoveRange(user.RefreshTokens);

        var accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(options.AccessTokenMinutes);
        var refreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(options.RefreshTokenMinutes);
        var refreshToken = CreateRefreshToken();

        await dbContext.UserRefreshTokens.AddAsync(new WisataUserRefreshToken
        {
            UserId = user.UserId,
            RefreshToken = refreshToken,
            ExpiredAt = refreshTokenExpiresAt
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return CreateTokenResponse(user, accessTokenExpiresAt, refreshToken, refreshTokenExpiresAt);
    }

    public async Task<WisataTokenResponse?> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return null;
        }

        var storedToken = await dbContext.UserRefreshTokens
            .Include(item => item.User)
            .SingleOrDefaultAsync(item => item.RefreshToken == refreshToken, cancellationToken);

        if (storedToken is null || storedToken.ExpiredAt <= DateTime.UtcNow)
        {
            if (storedToken is not null)
            {
                dbContext.UserRefreshTokens.Remove(storedToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return null;
        }

        dbContext.UserRefreshTokens.Remove(storedToken);

        var accessTokenExpiresAt = DateTime.UtcNow.AddMinutes(options.AccessTokenMinutes);
        var refreshTokenExpiresAt = DateTime.UtcNow.AddMinutes(options.RefreshTokenMinutes);
        var rotatedRefreshToken = CreateRefreshToken();

        await dbContext.UserRefreshTokens.AddAsync(new WisataUserRefreshToken
        {
            UserId = storedToken.UserId,
            RefreshToken = rotatedRefreshToken,
            ExpiredAt = refreshTokenExpiresAt
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return CreateTokenResponse(storedToken.User, accessTokenExpiresAt, rotatedRefreshToken, refreshTokenExpiresAt);
    }

    public async Task LogoutAsync(string refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return;
        }

        var storedToken = await dbContext.UserRefreshTokens
            .SingleOrDefaultAsync(item => item.RefreshToken == refreshToken, cancellationToken);

        if (storedToken is null)
        {
            return;
        }

        dbContext.UserRefreshTokens.Remove(storedToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private WisataTokenResponse CreateTokenResponse(
        WisataUser user,
        DateTime accessTokenExpiresAt,
        string refreshToken,
        DateTime refreshTokenExpiresAt)
    {
        return new WisataTokenResponse(
            AccessToken: CreateAccessToken(user, accessTokenExpiresAt),
            AccessTokenExpiresAt: accessTokenExpiresAt,
            RefreshToken: refreshToken,
            RefreshTokenExpiresAt: refreshTokenExpiresAt,
            Username: user.Username);
    }

    private string CreateAccessToken(WisataUser user, DateTime expiresAt)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string CreateRefreshToken()
    {
        return Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(48));
    }
}

public sealed record WisataTokenResponse(
    string AccessToken,
    DateTime AccessTokenExpiresAt,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt,
    string Username);