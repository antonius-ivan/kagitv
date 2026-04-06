namespace ICLAco.Identity.API.Services;

public class ProfileService(UserManager<ApplicationUser> userManager)
{
    public async Task<IEnumerable<Claim>> GetClaimsAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return [];
        }

        return await GetClaimsFromUserAsync(user);
    }

    public async Task<IEnumerable<Claim>> GetClaimsFromUserAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(Claims.Subject, user.Id),
            new(Claims.PreferredUsername, user.UserName ?? string.Empty),
            new(Claims.Name, user.Name ?? user.UserName ?? string.Empty)
        };

        if (!string.IsNullOrWhiteSpace(user.LastName))
            claims.Add(new Claim("last_name", user.LastName));

        if (!string.IsNullOrWhiteSpace(user.CardNumber))
            claims.Add(new Claim("card_number", user.CardNumber));

        if (!string.IsNullOrWhiteSpace(user.CardHolderName))
            claims.Add(new Claim("card_holder", user.CardHolderName));

        if (!string.IsNullOrWhiteSpace(user.SecurityNumber))
            claims.Add(new Claim("card_security_number", user.SecurityNumber));

        if (!string.IsNullOrWhiteSpace(user.Expiration))
            claims.Add(new Claim("card_expiration", user.Expiration));

        if (!string.IsNullOrWhiteSpace(user.City))
            claims.Add(new Claim("address_city", user.City));

        if (!string.IsNullOrWhiteSpace(user.Country))
            claims.Add(new Claim("address_country", user.Country));

        if (!string.IsNullOrWhiteSpace(user.State))
            claims.Add(new Claim("address_state", user.State));

        if (!string.IsNullOrWhiteSpace(user.Street))
            claims.Add(new Claim("address_street", user.Street));

        if (!string.IsNullOrWhiteSpace(user.ZipCode))
            claims.Add(new Claim("address_zip_code", user.ZipCode));

        if (userManager.SupportsUserEmail && !string.IsNullOrWhiteSpace(user.Email))
        {
            claims.AddRange([
                new Claim(Claims.Email, user.Email),
                new Claim(Claims.EmailVerified, user.EmailConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
            ]);
        }

        if (userManager.SupportsUserPhoneNumber && !string.IsNullOrWhiteSpace(user.PhoneNumber))
        {
            claims.AddRange([
                new Claim(Claims.PhoneNumber, user.PhoneNumber),
                new Claim(Claims.PhoneNumberVerified, user.PhoneNumberConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
            ]);
        }

        var roleNames = await userManager.GetRolesAsync(user);
        claims.AddRange(roleNames.Select(roleName => new Claim(Claims.Role, roleName)));

        return claims;
    }
}
