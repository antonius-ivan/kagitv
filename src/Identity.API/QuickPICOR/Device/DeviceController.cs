namespace IdentityServerHost.QuickPICOR.UI;

[Authorize]
[SecurityHeaders]
public class DeviceController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IOpenIddictApplicationManager applicationManager,
    IOpenIddictScopeManager scopeManager,
    ILogger<DeviceController> logger) : Controller
{
    [HttpGet("~/connect/verify")]
    public async Task<IActionResult> Index()
    {
        var userCode = Request.Query[Parameters.UserCode].ToString();

        if (string.IsNullOrWhiteSpace(userCode))
        {
            return View("UserCodeCapture");
        }

        if (User?.Identity?.IsAuthenticated != true)
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/connect/verify?" + QueryString.Create(Parameters.UserCode, userCode).ToUriComponent().TrimStart('?')
            });
        }

        var vm = await BuildViewModelAsync(userCode);
        if (vm is null)
        {
            return View("Error");
        }

        vm.ConfirmUserCode = true;
        return View("UserCodeConfirmation", vm);
    }

    [HttpPost("~/connect/verify")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Callback(DeviceAuthorizationInputModel model)
    {
        if (model is null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (string.Equals(model.Button, "no", StringComparison.OrdinalIgnoreCase))
        {
            return Forbid(
                new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.AccessDenied,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The authorization was denied by the user."
                }),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        if (model.ScopesConsented is null || !model.ScopesConsented.Any())
        {
            ModelState.AddModelError(string.Empty, ConsentOptions.MustChooseOneErrorMessage);
            return View("Error");
        }

        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/connect/verify" });
        }

        var principal = await signInManager.CreateUserPrincipalAsync(user);
        principal.SetClaim(Claims.Subject, user.Id);
        principal.SetClaim(Claims.Name, user.UserName ?? user.Id);
        principal.SetScopes(System.Collections.Immutable.ImmutableArray.CreateRange(model.ScopesConsented));

        var resources = new List<string>();
        await foreach (var resource in scopeManager.ListResourcesAsync(System.Collections.Immutable.ImmutableArray.CreateRange(model.ScopesConsented)))
        {
            resources.Add(resource);
        }
        principal.SetResources(resources);

        foreach (var claim in principal.Claims)
        {
            claim.SetDestinations(Extensions.GetDestinations(claim));
        }

        logger.LogInformation("Device verification approved for user {UserId}", user.Id);
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private Task<DeviceAuthorizationViewModel?> BuildViewModelAsync(string userCode)
    {
        return Task.FromResult<DeviceAuthorizationViewModel?>(new DeviceAuthorizationViewModel
        {
            UserCode = userCode,
            RememberConsent = true,
            AllowRememberConsent = true,
            IdentityScopes = Array.Empty<ScopeViewModel>(),
            ApiScopes = Array.Empty<ScopeViewModel>(),
            ScopesConsented = Array.Empty<string>(),
            ClientName = "Device Client"
        });
    }

    private static ScopeViewModel CreateScopeViewModel(string scope)
    {
        return new ScopeViewModel
        {
            Value = scope,
            DisplayName = scope,
            Description = "API access",
            Checked = true,
            Emphasize = string.Equals(scope, Scopes.OfflineAccess, StringComparison.OrdinalIgnoreCase)
        };
    }
}
