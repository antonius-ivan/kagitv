using ICLAco.AppHost;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddForwardedHeaders();

var launchProfileName = ShouldUseHttpForEndpoints() ? "http" : "https";

var consolidationApi = builder.AddProject<Projects.Consolidation_API>("consolidation-api");

var identityApi = builder.AddProject<Projects.Identity_API>("identity-api", launchProfileName)
    .WaitFor(consolidationApi)
    .WithExternalHttpEndpoints();

var identityEndpoint = identityApi.GetEndpoint(launchProfileName);

var salesItemApi = builder.AddProject<Projects.SalesItem_API>("salesitem-api")
    .WaitFor(consolidationApi) // Consolidation.API temporarily owns schema migration and seeding.
    .WithEnvironment("Identity__Url", identityEndpoint);

var blazenexj = builder.AddNodeApp("blazenexj", "../BlazeNexJ", "./app/server.ts")
    .WithNpm()
    .WithRunScript("dev")
    .WithHttpEndpoint(port: 26100, env: "PORT")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(salesItemApi)
    .WithEnvironment("IdentityUrl", identityEndpoint);
blazenexj.WithEnvironment("CallBackUrl", blazenexj.GetEndpoint("http"));
consolidationApi.WithEnvironment("BlazeNexJClient", blazenexj.GetEndpoint("http"));

identityApi.WithEnvironment("BlazeNexJClient", blazenexj.GetEndpoint("http"));

builder.Build().Run();

// For test use only.
// Looks for an environment variable that forces the use of HTTP for all the endpoints. We
// are doing this for ease of running the Playwright tests in CI.
static bool ShouldUseHttpForEndpoints()
{
    const string EnvVarName = "ESHOP_USE_HTTP_ENDPOINTS";
    var envValue = Environment.GetEnvironmentVariable(EnvVarName);

    // Attempt to parse the environment variable value; return true if it's exactly "1".
    return int.TryParse(envValue, out int result) && result == 1;
}
