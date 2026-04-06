namespace IdentityServerHost.QuickPICOR.UI;

[SecurityHeaders]
[AllowAnonymous]
public class ExternalController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ILogger<ExternalController> logger) : Controller
{
    [HttpGet("~/External/Challenge")]
    public IActionResult Challenge(string scheme, string? returnUrl)
    {
        returnUrl ??= "~/";

        if (!Url.IsLocalUrl(returnUrl) && !returnUrl.StartsWith("/connect", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Invalid return URL.");
        }

        var redirectUrl = Url.Action(nameof(Callback), new { returnUrl });
        var properties = signInManager.ConfigureExternalAuthenticationProperties(scheme, redirectUrl!);
        return Challenge(properties, scheme);
    }

    [HttpGet("~/External/Callback")]
    public async Task<IActionResult> Callback(string? returnUrl)
    {
        returnUrl ??= "~/";

        var info = await signInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            logger.LogWarning("External login info not available.");
            return RedirectToAction("Login", "Account", new { returnUrl });
        }

        var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (!signInResult.Succeeded)
        {
            var email = info.Principal.FindFirstValue(ClaimTypes.Email) ?? info.Principal.FindFirstValue(Claims.Email);
            var user = new ApplicationUser
            {
                UserName = email ?? Guid.NewGuid().ToString("N"),
                Email = email,
                EmailConfirmed = !string.IsNullOrWhiteSpace(email)
            };

            var createResult = await userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return RedirectToAction("Login", "Account", new { returnUrl });
            }

            var loginResult = await userManager.AddLoginAsync(user, info);
            if (!loginResult.Succeeded)
            {
                foreach (var error in loginResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return RedirectToAction("Login", "Account", new { returnUrl });
            }

            await signInManager.SignInAsync(user, isPersistent: false);
        }

        if (Url.IsLocalUrl(returnUrl) || returnUrl.StartsWith("/connect", StringComparison.OrdinalIgnoreCase))
        {
            return Redirect(returnUrl);
        }

        return Redirect("~/");
    }
}
