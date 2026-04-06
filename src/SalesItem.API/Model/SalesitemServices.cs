
using ICLAco.Catalog.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

public class SalesItemServices(
    SalesItemContext context,
    //[FromServices] ICatalogAI catalogAI,
    IOptions<SalesItemOptions> options,
    ILogger<SalesItemServices> logger
    //,
    //[FromServices] ICatalogIntegrationEventService eventService
    )
{

    public SalesItemContext Context { get; } = context;
    //public ICatalogAI CatalogAI { get; } = catalogAI;
    public IOptions<SalesItemOptions> Options { get; } = options;
    public ILogger<SalesItemServices> Logger { get; } = logger;
    //public ICatalogIntegrationEventService EventService { get; } = eventService;
};
