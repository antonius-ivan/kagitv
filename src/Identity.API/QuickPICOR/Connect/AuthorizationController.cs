namespace IdentityServerHost.QuickPICOR.UI;

public class AuthorizationController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IOpenIddictApplicationManager applicationManager,
    IOpenIddictAuthorizationManager authorizationManager,
    IOpenIddictScopeManager scopeManager,
    ProfileService profileService,
    ILogger<AuthorizationController> logger) : Controller
{
    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Authorize()
    {
        var clientId = await GetParameterAsync(Parameters.ClientId) ?? string.Empty;
        var redirectUri = await GetParameterAsync(Parameters.RedirectUri) ?? string.Empty;
        var responseType = await GetParameterAsync(Parameters.ResponseType) ?? string.Empty;
        var responseMode = await GetParameterAsync(Parameters.ResponseMode) ?? string.Empty;
        var promptValues = await GetSpaceSeparatedValuesAsync(Parameters.Prompt);
        var requestedScopes = await GetSpaceSeparatedValuesAsync(Parameters.Scope);

        logger.LogInformation(
            "Authorize request. ClientId: {ClientId}, RedirectUri: {RedirectUri}, ResponseType: {ResponseType}, ResponseMode: {ResponseMode}, Prompt: {Prompt}, Scope: {Scope}",
            clientId,
            redirectUri,
            responseType,
            responseMode,
            string.Join(' ', promptValues),
            string.Join(' ', requestedScopes));

        var authenticateResult = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        if (!authenticateResult.Succeeded || promptValues.Contains(PromptValues.Login))
        {
            if (promptValues.Contains(PromptValues.None))
            {
                return Forbid(
                    new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.LoginRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is not logged in."
                    }),
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            var prompt = string.Join(" ", promptValues.Where(value => !string.Equals(value, PromptValues.Login, StringComparison.Ordinal)));
            var parameters = Request.HasFormContentType ?
                Request.Form.Where(parameter => parameter.Key is not Parameters.Prompt).ToList() :
                Request.Query.Where(parameter => parameter.Key is not Parameters.Prompt).ToList();

            parameters.Add(KeyValuePair.Create(Parameters.Prompt, new Microsoft.Extensions.Primitives.StringValues(prompt)));

            var returnUrl = Request.PathBase + Request.Path + QueryString.Create(parameters);
            logger.LogInformation("User is not authenticated. Redirecting to login with returnUrl: {ReturnUrl}", returnUrl);
            return Redirect(Url.Action("Login", "Account", new { returnUrl }) ?? "/Account/Login");
        }

        var user = await userManager.GetUserAsync(authenticateResult.Principal);
        if (user is null)
        {
            var returnUrl = Request.PathBase + Request.Path + Request.QueryString;
            return Redirect(Url.Action("Login", "Account", new { returnUrl }) ?? "/Account/Login");
        }

            var application = await applicationManager.FindByClientIdAsync(clientId);
        if (application is null)
        {
            logger.LogWarning("Authorize failed: unknown client id {ClientId}", clientId);
            return Forbid(
                new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidClient,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The specified client application was not found."
                    }),
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var decision = Request.Query["consent"].ToString();
        var consentedScopes = ParseScopesFromQuery(Request.Query["scopes"].ToString(), requestedScopes.ToHashSet(StringComparer.Ordinal));
        if (decision.Equals("no", StringComparison.OrdinalIgnoreCase))
        {
            logger.LogInformation("Consent denied by user for client {ClientId}", clientId);
            return Forbid(
                new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.AccessDenied,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The authorization was denied by the user."
                }),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        if (string.IsNullOrWhiteSpace(decision))
        {
            if (string.Equals(clientId, "blazeWeb", StringComparison.OrdinalIgnoreCase))
            {
                decision = "yes";
                consentedScopes = requestedScopes;
                logger.LogInformation("Auto-approving consent for first-party client {ClientId}. Scopes: {Scopes}", clientId, string.Join(' ', consentedScopes));
            }
            else
            {
                var returnUrl = Request.PathBase + Request.Path + Request.QueryString;
                logger.LogInformation("Interactive consent required for client {ClientId}. Redirecting to consent UI.", clientId);
                return RedirectToAction("Index", "Consent", new { returnUrl });
            }
        }

        var principal = await signInManager.CreateUserPrincipalAsync(user);
        var profileClaims = await profileService.GetClaimsAsync(user.Id);
        principal = new ClaimsPrincipal(new ClaimsIdentity(profileClaims, principal.Identity?.AuthenticationType, Claims.Name, Claims.Role));

        principal.SetClaim(Claims.Subject, user.Id);
        principal.SetClaim(Claims.Email, user.Email ?? string.Empty);
        principal.SetClaim(Claims.Name, user.UserName ?? user.Id);
        principal.SetScopes(consentedScopes);

        var resources = new List<string>();
        await foreach (var resource in scopeManager.ListResourcesAsync(System.Collections.Immutable.ImmutableArray.CreateRange(consentedScopes)))
        {
            resources.Add(resource);
        }
        principal.SetResources(resources);

        foreach (var claim in principal.Claims)
        {
            claim.SetDestinations(Extensions.GetDestinations(claim));
        }

        object? authorization = null;
        await foreach (var item in authorizationManager.FindAsync(
                           subject: user.Id,
                           client: await applicationManager.GetIdAsync(application),
                           status: Statuses.Valid,
                           type: AuthorizationTypes.Permanent,
                           scopes: System.Collections.Immutable.ImmutableArray.CreateRange(consentedScopes)))
        {
            authorization = item;
            break;
        }

        authorization ??= await authorizationManager.CreateAsync(
            principal: principal,
            subject: user.Id,
            client: await applicationManager.GetIdAsync(application),
            type: AuthorizationTypes.Permanent,
            scopes: System.Collections.Immutable.ImmutableArray.CreateRange(consentedScopes));

        principal.SetAuthorizationId(await authorizationManager.GetIdAsync(authorization));

        logger.LogInformation("Authorize success for client {ClientId}. Returning authorization response.", clientId);

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [HttpPost("~/connect/token")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Exchange()
    {
        try
        {
            var grantType = await GetParameterAsync(Parameters.GrantType);
            var clientId = await GetParameterAsync(Parameters.ClientId);
            var requestedScopes = await GetSpaceSeparatedValuesAsync(Parameters.Scope);

            logger.LogInformation(
                "Token request received. GrantType: {GrantType}, ClientId: {ClientId}, Scope: {Scope}, ContentType: {ContentType}",
                grantType,
                clientId,
                string.Join(' ', requestedScopes),
                Request.ContentType);

            if (string.Equals(grantType, GrantTypes.ClientCredentials, StringComparison.Ordinal))
            {
                var application = await applicationManager.FindByClientIdAsync(clientId ?? string.Empty);
                if (application is null)
                {
                    logger.LogWarning("Client credentials token request failed: invalid client {ClientId}", clientId);
                    return Forbid(
                        new AuthenticationProperties(new Dictionary<string, string?>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidClient,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The specified client credentials are invalid."
                        }),
                        OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }

                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, Claims.Name, Claims.Role);
                identity.SetClaim(Claims.Subject, clientId ?? string.Empty);
                identity.SetClaim(Claims.Name, await applicationManager.GetDisplayNameAsync(application) ?? clientId ?? string.Empty);

                var principal = new ClaimsPrincipal(identity);
                principal.SetScopes(System.Collections.Immutable.ImmutableArray.CreateRange(requestedScopes));

                var resources = new List<string>();
                await foreach (var resource in scopeManager.ListResourcesAsync(principal.GetScopes()))
                {
                    resources.Add(resource);
                }
                principal.SetResources(resources);

                foreach (var claim in principal.Claims)
                {
                    claim.SetDestinations(Extensions.GetDestinations(claim));
                }

                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            if (!result.Succeeded || result.Principal is null)
            {
                logger.LogWarning("Authorization code/refresh token request failed authentication for client {ClientId}", clientId);
                return Forbid(
                    new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The token is no longer valid."
                    }),
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            var user = await userManager.FindByIdAsync(result.Principal.GetClaim(Claims.Subject) ?? string.Empty);
            if (user is null)
            {
                logger.LogWarning("Token request failed: user from principal not found. Subject: {Subject}", result.Principal.GetClaim(Claims.Subject));
                return Forbid(
                    new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The token is no longer valid."
                    }),
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            var profileClaims = await profileService.GetClaimsAsync(user.Id);
            var tokenIdentity = new ClaimsIdentity(result.Principal.Claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, Claims.Name, Claims.Role);
            tokenIdentity.AddClaims(profileClaims.Where(claim => tokenIdentity.HasClaim(claim.Type, claim.Value) is false));

            var principalForToken = new ClaimsPrincipal(tokenIdentity);
            principalForToken.SetScopes(result.Principal.GetScopes());

            var tokenResources = new List<string>();
            await foreach (var resource in scopeManager.ListResourcesAsync(principalForToken.GetScopes()))
            {
                tokenResources.Add(resource);
            }
            principalForToken.SetResources(tokenResources);

            foreach (var claim in principalForToken.Claims)
            {
                claim.SetDestinations(Extensions.GetDestinations(claim));
            }

            logger.LogInformation("Token request succeeded for client {ClientId} and subject {Subject}", clientId, result.Principal.GetClaim(Claims.Subject));

            return SignIn(principalForToken, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception in /connect/token exchange.");
            return Forbid(
                new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ServerError,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "An unexpected error occurred while processing the token request."
                }),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
    }

    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
    [HttpGet("~/connect/userinfo")]
    [HttpPost("~/connect/userinfo")]
    public async Task<IActionResult> Userinfo()
    {
        var subject = User.GetClaim(Claims.Subject);
        var user = string.IsNullOrWhiteSpace(subject)
            ? null
            : await userManager.FindByIdAsync(subject);
        if (user is null)
        {
            logger.LogWarning("Userinfo failed: no user resolved from access token subject. Subject: {Subject}", subject);
            return Forbid(
                new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidToken,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The access token is invalid."
                }),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var profileClaims = (await profileService.GetClaimsAsync(user.Id)).ToArray();
        var claimsByType = profileClaims
            .GroupBy(claim => claim.Type)
            .ToDictionary(group => group.Key, group => group.Select(claim => claim.Value).ToArray(), StringComparer.Ordinal);

        var response = new Dictionary<string, object>
        {
            [Claims.Subject] = user.Id,
            [Claims.Email] = user.Email ?? string.Empty,
            [Claims.Name] = user.UserName ?? user.Id,
            [Claims.PreferredUsername] = user.UserName ?? user.Id
        };

        AppendSingleClaim(response, claimsByType, "address_street");
        AppendSingleClaim(response, claimsByType, "address_city");
        AppendSingleClaim(response, claimsByType, "address_state");
        AppendSingleClaim(response, claimsByType, "address_country");
        AppendSingleClaim(response, claimsByType, "address_zip_code");

        if (claimsByType.TryGetValue(Claims.Role, out var roleNames) && roleNames.Length > 0)
        {
            response[Claims.Role] = roleNames;
        }

        logger.LogInformation("Userinfo success for subject {Subject}", user.Id);
        return Ok(response);
    }

    [HttpGet("~/connect/endsession")]
    [HttpPost("~/connect/endsession")]
    public IActionResult LogoutEndpoint()
    {
        return RedirectToAction("Logout", "Account", new { logoutId = Request.Query["logoutId"].ToString() });
    }

    private static IEnumerable<string> ParseScopesFromQuery(string rawScopes, HashSet<string> requestedScopes)
    {
        if (string.IsNullOrWhiteSpace(rawScopes))
        {
            return requestedScopes;
        }

        var parsed = rawScopes
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(scope => requestedScopes.Contains(scope))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        return parsed.Length == 0 ? requestedScopes : parsed;
    }

    private async Task<string?> GetParameterAsync(string name)
    {
        if (Request.HasFormContentType)
        {
            var form = await Request.ReadFormAsync();
            var formValue = form[name].ToString();
            if (!string.IsNullOrWhiteSpace(formValue))
            {
                return formValue;
            }
        }

        var queryValue = Request.Query[name].ToString();
        return string.IsNullOrWhiteSpace(queryValue) ? null : queryValue;
    }

    private static void AppendSingleClaim(Dictionary<string, object> response, IReadOnlyDictionary<string, string[]> claimsByType, string claimType)
    {
        if (claimsByType.TryGetValue(claimType, out var values) && values.Length > 0 && string.IsNullOrWhiteSpace(values[0]) is false)
        {
            response[claimType] = values[0];
        }
    }

    private async Task<HashSet<string>> GetSpaceSeparatedValuesAsync(string name)
    {
        var value = await GetParameterAsync(name);
        if (string.IsNullOrWhiteSpace(value))
        {
            return new HashSet<string>(StringComparer.Ordinal);
        }

        return value
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToHashSet(StringComparer.Ordinal);
    }
}
