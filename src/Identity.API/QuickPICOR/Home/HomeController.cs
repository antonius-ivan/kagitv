namespace IdentityServerHost.QuickPICOR.UI;

[SecurityHeaders]
[AllowAnonymous]
public class HomeController(IWebHostEnvironment environment, ILogger<HomeController> logger) : Controller
{
    public IActionResult Index()
    {
        if (environment.IsDevelopment())
        {
            return View();
        }

        logger.LogInformation("Homepage is disabled in production. Returning 404.");
        return NotFound();
    }

    public IActionResult Error(string? error, string? error_description, string? request_id)
    {
        var vm = new ErrorViewModel
        {
            Error = new ErrorMessage
            {
                Error = error ?? "server_error",
                ErrorDescription = environment.IsDevelopment() ? error_description : null,
                RequestId = request_id ?? HttpContext.TraceIdentifier
            }
        };

        return View("Error", vm);
    }
}
