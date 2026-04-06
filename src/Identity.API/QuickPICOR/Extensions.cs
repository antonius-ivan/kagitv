namespace IdentityServerHost.QuickPICOR.UI;

public static class Extensions
{
    public static IActionResult LoadingPage(this Controller controller, string viewName, string redirectUri)
    {
        controller.HttpContext.Response.StatusCode = 200;
        controller.HttpContext.Response.Headers["Location"] = "";

        return controller.View(viewName, new RedirectViewModel { RedirectUrl = redirectUri });
    }

    public static IEnumerable<string> GetDestinations(Claim claim)
    {
        return claim.Type switch
        {
            Claims.Name or Claims.PreferredUsername => [Destinations.AccessToken, Destinations.IdentityToken],
            Claims.Email => [Destinations.AccessToken, Destinations.IdentityToken],
            Claims.Role => [Destinations.AccessToken, Destinations.IdentityToken],
            "security_stamp" => [],
            _ => [Destinations.AccessToken]
        };
    }
}
