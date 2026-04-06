namespace IdentityServerHost.QuickPICOR.UI;

[SecurityHeaders]
[AllowAnonymous]
public class AccountController(
    SignInManager<ApplicationUser> signInManager,
    IAuthenticationSchemeProvider schemeProvider,
    IAuthenticationHandlerProvider handlerProvider,
    ILogger<AccountController> logger) : Controller
{
    [HttpGet("~/Account/Login", Name = "Login")]
    public async Task<IActionResult> Login(string? returnUrl)
    {
        var vm = await BuildLoginViewModelAsync(returnUrl);
        ViewData["ReturnUrl"] = returnUrl;

        if (vm.IsExternalLoginOnly)
        {
            return RedirectToAction("Challenge", "External", new { scheme = vm.ExternalLoginScheme, returnUrl });
        }

        return View(vm);
    }

    [HttpPost("~/Account/Login", Name = "Login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginInputModel model, string button)
    {
        if (!string.Equals(button, "login", StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToSafeUrl(model.ReturnUrl, "~/");
        }

        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                logger.LogInformation("User {Username} signed in.", model.Username);
                return RedirectToSafeUrl(model.ReturnUrl, "~/");
            }

            ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
        }

        var vm = await BuildLoginViewModelAsync(model);
        ViewData["ReturnUrl"] = model.ReturnUrl;
        return View(vm);
    }

    [HttpGet("~/Account/Logout")]
    public async Task<IActionResult> Logout(string? logoutId)
    {
        var vm = await BuildLogoutViewModelAsync(logoutId);

        if (!vm.ShowLogoutPrompt)
        {
            return await Logout(new LogoutInputModel { LogoutId = logoutId });
        }

        return View(vm);
    }

    [HttpPost("~/Account/Logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(LogoutInputModel model)
    {
        var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

        if (User?.Identity?.IsAuthenticated == true)
        {
            await signInManager.SignOutAsync();
            logger.LogInformation("User logged out.");
        }

        if (vm.TriggerExternalSignout)
        {
            var url = Url.Action("Logout", new { logoutId = vm.LogoutId });
            return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme!);
        }

        return View("LoggedOut", vm);
    }

    [HttpGet("~/Account/AccessDenied")]
    public IActionResult AccessDenied()
    {
        return View();
    }

    private async Task<LoginViewModel> BuildLoginViewModelAsync(string? returnUrl)
    {
        var schemes = await schemeProvider.GetAllSchemesAsync();

        var providers = schemes
            .Where(x => !string.IsNullOrWhiteSpace(x.DisplayName))
            .Select(x => new ExternalProvider
            {
                DisplayName = x.DisplayName ?? x.Name,
                AuthenticationScheme = x.Name
            })
            .ToArray();

        return new LoginViewModel
        {
            AllowRememberLogin = AccountOptions.AllowRememberLogin,
            EnableLocalLogin = AccountOptions.AllowLocalLogin,
            ReturnUrl = returnUrl,
            ExternalProviders = providers
        };
    }

    private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
    {
        var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
        vm.Username = model.Username;
        vm.RememberLogin = model.RememberLogin;
        return vm;
    }

    private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string? logoutId)
    {
        var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

        if (User?.Identity?.IsAuthenticated != true)
        {
            vm.ShowLogoutPrompt = false;
        }

        await Task.CompletedTask;
        return vm;
    }

    private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string? logoutId)
    {
        var vm = new LoggedOutViewModel
        {
            AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
            PostLogoutRedirectUri = ExtractPostLogoutRedirect(logoutId),
            ClientName = "Client application",
            LogoutId = logoutId
        };

        if (User?.Identity?.IsAuthenticated == true)
        {
            var idp = User.FindFirst("idp")?.Value;
            if (!string.IsNullOrWhiteSpace(idp) && !string.Equals(idp, IdentityConstants.ApplicationScheme, StringComparison.OrdinalIgnoreCase))
            {
                var handler = await handlerProvider.GetHandlerAsync(HttpContext, idp);
                if (handler is IAuthenticationSignOutHandler)
                {
                    vm.ExternalAuthenticationScheme = idp;
                }
            }
        }

        return vm;
    }

    private static string? ExtractPostLogoutRedirect(string? logoutId)
    {
        if (string.IsNullOrWhiteSpace(logoutId))
        {
            return null;
        }

        if (Uri.TryCreate(logoutId, UriKind.RelativeOrAbsolute, out var redirectUri))
        {
            return redirectUri.ToString();
        }

        return null;
    }

    private IActionResult RedirectToSafeUrl(string? returnUrl, string fallback)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        if (!string.IsNullOrWhiteSpace(returnUrl) && returnUrl.StartsWith("/connect", StringComparison.OrdinalIgnoreCase))
        {
            return Redirect(returnUrl);
        }

        return Redirect(fallback);
    }
}
