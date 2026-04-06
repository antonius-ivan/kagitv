/* COMMENTED 2026-03-10, Persistence centralization in progress: helper/runtime service duplication is out of scope for compile stabilization.

using Microsoft.Extensions.Options;

public class SalesItemServices(
    SalesItemContext context,
    IOptions<SalesItemOptions> options,
    ILogger<SalesItemServices> logger)
{
    public SalesItemContext Context { get; } = context;
    public IOptions<SalesItemOptions> Options { get; } = options;
    public ILogger<SalesItemServices> Logger { get; } = logger;
};
*/
