namespace IdentityServerHost.QuickPICOR.UI;

[SecurityHeaders]
[Authorize]
public class ConsentController(
    IOpenIddictApplicationManager applicationManager,
    ILogger<ConsentController> logger) : Controller
{
    [HttpGet("~/Consent/Index")]
    public async Task<IActionResult> Index(string returnUrl)
    {
        var vm = await BuildViewModelAsync(returnUrl);
        if (vm is not null)
        {
            return View("Index", vm);
        }

        return View("Error");
    }

    [HttpPost("~/Consent/Index")]
    [ValidateAntiForgeryToken]
    public IActionResult Index(ConsentInputModel model)
    {
        if (string.IsNullOrWhiteSpace(model.ReturnUrl))
        {
            return View("Error");
        }

        if (!string.Equals(model.Button, "yes", StringComparison.OrdinalIgnoreCase))
        {
            var deniedUrl = AppendQuery(model.ReturnUrl, "consent", "no");
            return Redirect(deniedUrl);
        }

        if (model.ScopesConsented is null || !model.ScopesConsented.Any())
        {
            ModelState.AddModelError(string.Empty, ConsentOptions.MustChooseOneErrorMessage);
            return RedirectToAction(nameof(Index), new { returnUrl = model.ReturnUrl });
        }

        var approvedUrl = AppendQuery(model.ReturnUrl, "consent", "yes");
        approvedUrl = AppendQuery(approvedUrl, "scopes", string.Join(',', model.ScopesConsented));
        approvedUrl = AppendQuery(approvedUrl, "remember", model.RememberConsent ? "true" : "false");
        approvedUrl = AppendQuery(approvedUrl, "description", model.Description ?? string.Empty);

        return Redirect(approvedUrl);
    }

    private async Task<ConsentViewModel?> BuildViewModelAsync(string returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            return null;
        }

        var queryStart = returnUrl.IndexOf('?');
        var rawQuery = queryStart >= 0 ? returnUrl[queryStart..] : string.Empty;
        var query = QueryHelpers.ParseQuery(rawQuery);

        var clientId = query.TryGetValue(Parameters.ClientId, out var clientIdValue)
            ? clientIdValue.ToString()
            : string.Empty;

        var scopeValues = query.TryGetValue(Parameters.Scope, out var scopeValue)
            ? scopeValue.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries)
            : Array.Empty<string>();

        var app = string.IsNullOrWhiteSpace(clientId) ? null : await applicationManager.FindByClientIdAsync(clientId);
        if (app is null)
        {
            logger.LogWarning("Consent requested with unknown client id: {ClientId}", clientId);
            return null;
        }

        var identityScopes = new List<ScopeViewModel>();
        var apiScopes = new List<ScopeViewModel>();

        foreach (var scope in scopeValues)
        {
            var vm = CreateScopeViewModel(scope);
            if (scope is Scopes.OpenId or Scopes.Profile or Scopes.OfflineAccess)
            {
                identityScopes.Add(vm);
            }
            else
            {
                apiScopes.Add(vm);
            }
        }

        return new ConsentViewModel
        {
            ReturnUrl = returnUrl,
            RememberConsent = true,
            AllowRememberConsent = true,
            ClientName = await applicationManager.GetDisplayNameAsync(app) ?? clientId,
            ClientUrl = null,
            ClientLogoUrl = null,
            IdentityScopes = identityScopes,
            ApiScopes = apiScopes,
            ScopesConsented = scopeValues
        };
    }

    private static ScopeViewModel CreateScopeViewModel(string scope)
    {
        return scope switch
        {
            Scopes.OpenId => new ScopeViewModel
            {
                Value = scope,
                DisplayName = "OpenID",
                Description = "Authenticate using your account",
                Required = true,
                Checked = true
            },
            Scopes.Profile => new ScopeViewModel
            {
                Value = scope,
                DisplayName = "Profile",
                Description = "Access your basic profile information",
                Checked = true
            },
            Scopes.OfflineAccess => new ScopeViewModel
            {
                Value = scope,
                DisplayName = ConsentOptions.OfflineAccessDisplayName,
                Description = ConsentOptions.OfflineAccessDescription,
                Emphasize = true,
                Checked = true
            },
            _ => new ScopeViewModel
            {
                Value = scope,
                DisplayName = scope,
                Description = "API access",
                Checked = true
            }
        };
    }

    private static string AppendQuery(string url, string key, string value)
    {
        return QueryHelpers.AddQueryString(url, key, value);
    }
}
