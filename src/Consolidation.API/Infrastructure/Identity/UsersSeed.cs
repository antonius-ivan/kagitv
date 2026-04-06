using Consolidation.API.Model.Identity;
using ICLAco.ServiceDefaults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;

namespace Consolidation.API.Infrastructure.Identity;

public class UsersSeed(
    ILogger<UsersSeed> logger,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IConfiguration configuration,
    IOpenIddictApplicationManager applicationManager,
    IOpenIddictScopeManager scopeManager) : IDbSeeder<ApplicationDbContext>
{
    public async Task SeedAsync(ApplicationDbContext context)
    {
        var alice = await userManager.FindByNameAsync("alice");

        if (alice == null)
        {
            alice = new ApplicationUser
            {
                UserName = "alice",
                Email = "AliceSmith@email.com",
                EmailConfirmed = true,
                CardHolderName = "Alice Smith",
                CardNumber = "XXXXXXXXXXXX1881",
                CardType = 1,
                City = "Redmond",
                Country = "U.S.",
                Expiration = "12/24",
                Id = Guid.NewGuid().ToString(),
                LastName = "Smith",
                Name = "Alice",
                PhoneNumber = "1234567890",
                ZipCode = "98052",
                State = "WA",
                Street = "15703 NE 61st Ct",
                SecurityNumber = "123"
            };

            var result = await userManager.CreateAsync(alice, "Pass123$");

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("alice created");
            }
        }
        else if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("alice already exists");
        }

        var bob = await userManager.FindByNameAsync("bob");

        if (bob == null)
        {
            bob = new ApplicationUser
            {
                UserName = "bob",
                Email = "BobSmith@email.com",
                EmailConfirmed = true,
                CardHolderName = "Bob Smith",
                CardNumber = "XXXXXXXXXXXX1881",
                CardType = 1,
                City = "Redmond",
                Country = "U.S.",
                Expiration = "12/24",
                Id = Guid.NewGuid().ToString(),
                LastName = "Smith",
                Name = "Bob",
                PhoneNumber = "1234567890",
                ZipCode = "98052",
                State = "WA",
                Street = "15703 NE 61st Ct",
                SecurityNumber = "456"
            };

            var result = await userManager.CreateAsync(bob, "Pass123$");

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("bob created");
            }
        }
        else if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("bob already exists");
        }

        var ivan = await userManager.FindByNameAsync("ivan");

        if (ivan == null)
        {
            ivan = new ApplicationUser
            {
                UserName = "ivan",
                Email = "aivan@gmail.com",
                EmailConfirmed = true,
                CardHolderName = "aivan Smith",
                CardNumber = "XXXXXXXXXXXX1881",
                CardType = 1,
                City = "Redmond",
                Country = "U.S.",
                Expiration = "12/24",
                Id = Guid.NewGuid().ToString(),
                LastName = "Smith",
                Name = "Aivan",
                PhoneNumber = "1234567890",
                ZipCode = "98052",
                State = "WA",
                Street = "15703 NE 61st Ct",
                SecurityNumber = "123"
            };

            var result = await userManager.CreateAsync(ivan, "Pass123$");

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("alice created");
            }
        }
        else if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("ivan already exists");
        }

        var usradmin = await userManager.FindByNameAsync("usradmin");

        if (usradmin == null)
        {
            usradmin = new ApplicationUser
            {
                UserName = "usradmin",
                Email = "usradmin@gmail.com",
                EmailConfirmed = true,
                CardHolderName = "usradmin Smith",
                CardNumber = "XXXXXXXXXXXX1881",
                CardType = 1,
                City = "Redmond",
                Country = "U.S.",
                Expiration = "12/24",
                Id = Guid.NewGuid().ToString(),
                LastName = "Smith",
                Name = "User Admin",
                PhoneNumber = "1234567890",
                ZipCode = "98052",
                State = "WA",
                Street = "15703 NE 61st Ct",
                SecurityNumber = "123"
            };

            var result = await userManager.CreateAsync(usradmin, "Pass123$");

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("usradmin created");
            }
        }
        else if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("usradmin already exists");
        }

        var marcella = await userManager.FindByNameAsync("marcella");

        if (marcella == null)
        {
            marcella = new ApplicationUser
            {
                UserName = "marcella",
                Email = "marcella@gmail.com",
                EmailConfirmed = true,
                CardHolderName = "marcella marcl",
                CardNumber = "XXXXXXXXXXXX1881",
                CardType = 1,
                City = "Redmond",
                Country = "U.S.",
                Expiration = "12/24",
                Id = Guid.NewGuid().ToString(),
                LastName = "Smith",
                Name = "User Admin",
                PhoneNumber = "1234567890",
                ZipCode = "98052",
                State = "WA",
                Street = "15703 NE 61st Ct",
                SecurityNumber = "123"
            };

            var result = await userManager.CreateAsync(marcella, "Pass123$");

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("marcella created");
            }
        }
        else if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("marcella already exists");
        }

        await EnsureRoleAsync("Admin");
        await EnsureRoleAsync("Staff");

        await EnsureUserInRoleAsync(ivan, "Admin");
        await EnsureUserInRoleAsync(usradmin, "Admin");
        await EnsureUserInRoleAsync(alice, "Staff");
        await EnsureUserInRoleAsync(bob, "Staff");
        await EnsureUserInRoleAsync(marcella, "Staff");

        await SeedOpenIddictAsync();
    }

    private async Task EnsureRoleAsync(string roleName)
    {
        if (await roleManager.RoleExistsAsync(roleName))
        {
            return;
        }

        var result = await roleManager.CreateAsync(new IdentityRole(roleName));
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }
    }

    private async Task EnsureUserInRoleAsync(ApplicationUser user, string roleName)
    {
        if (await userManager.IsInRoleAsync(user, roleName))
        {
            return;
        }

        var result = await userManager.AddToRoleAsync(user, roleName);
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }
    }

    private async Task SeedOpenIddictAsync()
    {
        await EnsureScopeAsync("orders", "Orders Service");
        await EnsureScopeAsync("basket", "Basket Service");
        await EnsureScopeAsync("webhooks", "Webhooks registration Service");

        var mauiCallback = configuration["MauiCallback"] ?? "maui://authcallback";
        var blazeWebClient = configuration["BlazeWebClient"] ?? "https://localhost:7246";
        var blazeNexJClient = configuration["BlazeNexJClient"] ?? "http://localhost:26100";
        var webhooksWebClient = configuration["WebhooksWebClient"] ?? "https://localhost:7201";
        var basketApiClient = configuration["BasketApiClient"] ?? "https://localhost:5103";
        var orderingApiClient = configuration["OrderingApiClient"] ?? "https://localhost:5105";
        var webhooksApiClient = configuration["WebhooksApiClient"] ?? "https://localhost:5110";

        await EnsureConfidentialCodeClientAsync(
            clientId: "maui",
            displayName: "eShop MAUI OpenId Client",
            clientSecret: "secret",
            redirectUris: [$"{mauiCallback}"],
            postLogoutRedirectUris: [$"{mauiCallback}/Account/Redirecting"],
            scopes: [
                OpenIddictConstants.Scopes.OpenId,
                OpenIddictConstants.Scopes.Profile,
                OpenIddictConstants.Scopes.OfflineAccess,
                "orders",
                "basket",
                "mobileshoppingagg",
                "webhooks"
            ]);

        await EnsureConfidentialCodeClientAsync(
            clientId: "blazeWeb",
            displayName: "BlazeWeb Client",
            clientSecret: "secret",
            redirectUris: [$"{blazeWebClient}/signin-oidc"],
            postLogoutRedirectUris: [$"{blazeWebClient}/signout-callback-oidc"],
            scopes: [
                OpenIddictConstants.Scopes.OpenId,
                OpenIddictConstants.Scopes.Profile,
                OpenIddictConstants.Scopes.OfflineAccess,
                "orders",
                "basket",
                "webshoppingagg",
                "webhooks"
            ]);

        await EnsureConfidentialCodeClientAsync(
            clientId: "blazenexj",
            displayName: "BlazeNexJ Client",
            clientSecret: "secret",
            redirectUris: [$"{blazeNexJClient}/auth/callback"],
            postLogoutRedirectUris: [$"{blazeNexJClient}/salesitem"],
            scopes: [
                OpenIddictConstants.Scopes.OpenId,
                OpenIddictConstants.Scopes.Profile,
                OpenIddictConstants.Scopes.OfflineAccess,
                "orders",
                "basket",
                "webshoppingagg",
                "webhooks"
            ]);

        await EnsureConfidentialCodeClientAsync(
            clientId: "webhooksclient",
            displayName: "Webhooks Client",
            clientSecret: "secret",
            redirectUris: [$"{webhooksWebClient}/signin-oidc"],
            postLogoutRedirectUris: [$"{webhooksWebClient}/signout-callback-oidc"],
            scopes: [
                OpenIddictConstants.Scopes.OpenId,
                OpenIddictConstants.Scopes.Profile,
                OpenIddictConstants.Scopes.OfflineAccess,
                "webhooks"
            ]);

        await EnsureImplicitSwaggerClientAsync(
            clientId: "basketswaggerui",
            displayName: "Basket Swagger UI",
            redirectUri: $"{basketApiClient}/swagger/oauth2-redirect.html",
            postLogoutRedirectUri: $"{basketApiClient}/swagger/",
            scopes: ["basket"]);

        await EnsureImplicitSwaggerClientAsync(
            clientId: "orderingswaggerui",
            displayName: "Ordering Swagger UI",
            redirectUri: $"{orderingApiClient}/swagger/oauth2-redirect.html",
            postLogoutRedirectUri: $"{orderingApiClient}/swagger/",
            scopes: ["orders"]);

        await EnsureImplicitSwaggerClientAsync(
            clientId: "webhooksswaggerui",
            displayName: "WebHooks Service Swagger UI",
            redirectUri: $"{webhooksApiClient}/swagger/oauth2-redirect.html",
            postLogoutRedirectUri: $"{webhooksApiClient}/swagger/",
            scopes: ["webhooks"]);
    }

    private async Task EnsureScopeAsync(string name, string displayName)
    {
        if (await scopeManager.FindByNameAsync(name) is not null)
        {
            return;
        }

        await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
        {
            Name = name,
            DisplayName = displayName,
            Resources = { name }
        });
    }

    private async Task EnsureConfidentialCodeClientAsync(
        string clientId,
        string displayName,
        string clientSecret,
        IEnumerable<string> redirectUris,
        IEnumerable<string> postLogoutRedirectUris,
        IEnumerable<string> scopes)
    {
        if (await applicationManager.FindByClientIdAsync(clientId) is not null)
        {
            return;
        }

        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            DisplayName = displayName,
            ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
            ClientType = OpenIddictConstants.ClientTypes.Confidential
        };

        descriptor.Permissions.UnionWith([
            OpenIddictConstants.Permissions.Endpoints.Authorization,
            OpenIddictConstants.Permissions.Endpoints.Token,
            OpenIddictConstants.Permissions.Endpoints.EndSession,
            OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
            OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
            OpenIddictConstants.Permissions.ResponseTypes.Code
        ]);

        foreach (var scope in scopes)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + scope);
        }

        AddUris(descriptor.RedirectUris, redirectUris);
        AddUris(descriptor.PostLogoutRedirectUris, postLogoutRedirectUris);

        await applicationManager.CreateAsync(descriptor);
    }

    private async Task EnsureImplicitSwaggerClientAsync(
        string clientId,
        string displayName,
        string redirectUri,
        string postLogoutRedirectUri,
        IEnumerable<string> scopes)
    {
        if (await applicationManager.FindByClientIdAsync(clientId) is not null)
        {
            return;
        }

        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = clientId,
            DisplayName = displayName,
            ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
            ClientType = OpenIddictConstants.ClientTypes.Public
        };

        descriptor.Permissions.UnionWith([
            OpenIddictConstants.Permissions.Endpoints.Authorization,
            OpenIddictConstants.Permissions.GrantTypes.Implicit,
            OpenIddictConstants.Permissions.ResponseTypes.Token
        ]);

        foreach (var scope in scopes)
        {
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + scope);
        }

        AddUris(descriptor.RedirectUris, [redirectUri]);
        AddUris(descriptor.PostLogoutRedirectUris, [postLogoutRedirectUri]);

        await applicationManager.CreateAsync(descriptor);
    }

    private static void AddUris(ICollection<Uri> target, IEnumerable<string> values)
    {
        foreach (var value in values)
        {
            if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
            {
                target.Add(uri);
            }
        }
    }
}