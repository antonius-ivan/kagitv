namespace IdentityServerHost.QuickPICOR.UI;

[SecurityHeaders]
[Authorize]
public class GrantsController(
    UserManager<ApplicationUser> userManager,
    IOpenIddictAuthorizationManager authorizationManager,
    IOpenIddictApplicationManager applicationManager,
    ILogger<GrantsController> logger) : Controller
{
    [HttpGet("~/Grants")]
    public async Task<IActionResult> Index()
    {
        return View("Index", await BuildViewModelAsync());
    }

    [HttpPost("~/Grants/Revoke")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Revoke(string clientId)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge();
        }

        await foreach (var authorization in authorizationManager.ListAsync())
        {
            var subject = await authorizationManager.GetSubjectAsync(authorization);
            if (!string.Equals(subject, user.Id, StringComparison.Ordinal))
            {
                continue;
            }

            var appId = await authorizationManager.GetApplicationIdAsync(authorization);
            if (string.IsNullOrWhiteSpace(appId))
            {
                continue;
            }

            var app = await applicationManager.FindByIdAsync(appId);
            if (app is null)
            {
                continue;
            }

            var currentClientId = await applicationManager.GetClientIdAsync(app);
            if (!string.Equals(currentClientId, clientId, StringComparison.Ordinal))
            {
                continue;
            }

            await authorizationManager.TryRevokeAsync(authorization);
        }

        logger.LogInformation("Grants revoked for client {ClientId}", clientId);
        return RedirectToAction(nameof(Index));
    }

    private async Task<GrantsViewModel> BuildViewModelAsync()
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return new GrantsViewModel { Grants = Array.Empty<GrantViewModel>() };
        }

        var grants = new List<GrantViewModel>();

        await foreach (var authorization in authorizationManager.ListAsync())
        {
            var subject = await authorizationManager.GetSubjectAsync(authorization);
            if (!string.Equals(subject, user.Id, StringComparison.Ordinal))
            {
                continue;
            }

            var appId = await authorizationManager.GetApplicationIdAsync(authorization);
            if (string.IsNullOrWhiteSpace(appId))
            {
                continue;
            }

            var app = await applicationManager.FindByIdAsync(appId);
            if (app is null)
            {
                continue;
            }

            var clientId = await applicationManager.GetClientIdAsync(app);
            var clientName = await applicationManager.GetDisplayNameAsync(app) ?? clientId ?? "unknown-client";
            var scopes = (await authorizationManager.GetScopesAsync(authorization)).ToArray();

            var identityScopes = scopes.Where(scope => scope is Scopes.OpenId or Scopes.Profile or Scopes.OfflineAccess).ToArray();
            var apiScopes = scopes.Where(scope => scope is not Scopes.OpenId and not Scopes.Profile and not Scopes.OfflineAccess).ToArray();

            grants.Add(new GrantViewModel
            {
                ClientId = clientId ?? string.Empty,
                ClientName = clientName,
                Description = await authorizationManager.GetPropertiesAsync(authorization) is { Count: > 0 } ? "Granted authorization" : null,
                Created = (await authorizationManager.GetCreationDateAsync(authorization))?.UtcDateTime ?? DateTime.UtcNow,
                Expires = null,
                IdentityGrantNames = identityScopes,
                ApiGrantNames = apiScopes
            });
        }

        return new GrantsViewModel { Grants = grants };
    }
}
