using ICLAco.Catalog.API.Model;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ICLACo.SalesItem.API;

public static class SalesItemApi
{
    public static IEndpointRouteBuilder MapCatalogApi(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/menu", async (ClaimsPrincipal user, IMenuTreeService menuTreeService, CancellationToken cancellationToken) =>
            Results.Ok(await menuTreeService.GetMenuAsync(user, cancellationToken)))
            .RequireAuthorization()
            .WithName("GetRoleMenu")
            .WithSummary("Get role-filtered menu tree")
            .WithDescription("Returns the recursive ERP menu tree filtered by the current user's roles.")
            .WithTags("Menu");

        // RouteGroupBuilder for sales item endpoints
        var vApi = app.NewVersionedApi("SalesItem");
        var api = vApi.MapGroup("api/salesitem").HasApiVersion(1, 0).HasApiVersion(2, 0);
        var v1 = vApi.MapGroup("api/salesitem").HasApiVersion(1, 0);
        var v2 = vApi.MapGroup("api/salesitem").HasApiVersion(2, 0);

        // Routes for querying salesitem items.
        v1.MapGet("/items", GetAllItemsV1)
            .WithName("ListItems")
            .WithSummary("List salesitem items")
            .WithDescription("Get a paginated list of items in the salesitem.")
            .WithTags("Items");
        v2.MapGet("/items", GetAllItems)
            .WithName("ListItems-V2")
            .WithSummary("List salesitem items")
            .WithDescription("Get a paginated list of items in the salesitem.")
            .WithTags("Items");
        api.MapGet("/items/by", GetItemsByIds)
            .WithName("BatchGetItems")
            .WithSummary("Batch get salesitem items")
            .WithDescription("Get multiple items from the salesitem")
            .WithTags("Items");
        api.MapGet("/items/{id:int}", GetItemById)
            .WithName("GetItem")
            .WithSummary("Get salesitem item")
            .WithDescription("Get an item from the salesitem")
            .WithTags("Items");
        v1.MapGet("/items/by/{name:minlength(1)}", GetItemsByName)
            .WithName("GetItemsByName")
            .WithSummary("Get salesitem items by name")
            .WithDescription("Get a paginated list of salesitem items with the specified name.")
            .WithTags("Items");
        api.MapGet("/items/{id:int}/pic", GetItemPictureById)
            .WithName("GetItemPicture")
            .WithSummary("Get salesitem item picture")
            .WithDescription("Get the picture for a salesitem item")
            .WithTags("Items");

        // Routes for resolving salesitem items using AI.
        //v1.MapGet("/items/withsemanticrelevance/{text:minlength(1)}", GetItemsBySemanticRelevanceV1)
        //    .WithName("GetRelevantItems")
        //    .WithSummary("Search salesitem for relevant items")
        //    .WithDescription("Search the salesitem for items related to the specified text")
        //    .WithTags("Search");

        //        // Routes for resolving salesitem items using AI.
        //v2.MapGet("/items/withsemanticrelevance", GetItemsBySemanticRelevance)
        //    .WithName("GetRelevantItems-V2")
        //    .WithSummary("Search salesitem for relevant items")
        //    .WithDescription("Search the salesitem for items related to the specified text")
        //    .WithTags("Search");

        // Routes for resolving salesitem items by type and brand.
        v1.MapGet("/items/type/{typeId}/brand/{brandId?}", GetItemsByBrandAndTypeId)
            .WithName("GetItemsByTypeAndBrand")
            .WithSummary("Get salesitem items by type and brand")
            .WithDescription("Get salesitem items of the specified type and brand")
            .WithTags("Types");
        v1.MapGet("/items/type/all/brand/{brandId:int?}", GetItemsByBrandId)
            .WithName("GetItemsByBrand")
            .WithSummary("List salesitem items by brand")
            .WithDescription("Get a list of salesitem items for the specified brand")
            .WithTags("Brands");
        api.MapGet("/salesitemtypes",
            [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
            async (SalesItemContext context) => await context.SalesItemTypes.OrderBy(x => x.Type).ToListAsync())
            .WithName("ListItemTypes")
            .WithSummary("List salesitem item types")
            .WithDescription("Get a list of the types of salesitem items")
            .WithTags("Types");
        api.MapGet("/salesitembrands",
            [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
            async (SalesItemContext context) => await context.SalesItemBrands.OrderBy(x => x.Brand).ToListAsync())
            .WithName("ListItemBrands")
            .WithSummary("List salesitem item brands")
            .WithDescription("Get a list of the brands of salesitem items")
            .WithTags("Brands");

        // Routes for modifying salesitem items.
        //v1.MapPut("/items", UpdateItemV1)
        //    .WithName("UpdateItem")
        //    .WithSummary("Create or replace a salesitem item")
        //    .WithDescription("Create or replace a salesitem item")
        //    .WithTags("Items");
        //v2.MapPut("/items/{id:int}", UpdateItem)
        //    .WithName("UpdateItem-V2")
        //    .WithSummary("Create or replace a salesitem item")
        //    .WithDescription("Create or replace a salesitem item")
        //    .WithTags("Items");
        //api.MapPost("/items", CreateItem)
        //    .WithName("CreateItem")
        //    .WithSummary("Create a salesitem item")
        //    .WithDescription("Create a new item in the salesitem");
        //api.MapDelete("/items/{id:int}", DeleteItemById)
        //    .WithName("DeleteItem")
        //    .WithSummary("Delete salesitem item")
        //    .WithDescription("Delete the specified salesitem item");

        // Currency endpoints
        api.MapGet("/currencies", GetAllCurrencies)
            .WithName("ListCurrencies")
            .WithSummary("List currencies")
            .WithDescription("Get all supported currencies")
            .WithTags("Currencies");

        // Per-currency price overrides
        api.MapGet("/items/{id:int}/prices", GetItemPrices)
            .WithName("GetItemPrices")
            .WithSummary("Get item currency prices")
            .WithDescription("Get all per-currency prices for a salesitem item")
            .WithTags("Items");

        // Physical dimensions
        api.MapGet("/items/{id:int}/dimensions", GetItemDimensions)
            .WithName("GetItemDimensions")
            .WithSummary("Get item dimensions")
            .WithDescription("Get the physical dimensions and weight for a salesitem item")
            .WithTags("Items");

        // Unit measurements
        api.MapGet("/unitmeasurements", GetAllUnitMeasurements)
            .WithName("ListUnitMeasurements")
            .WithSummary("List unit measurements")
            .WithDescription("Get all unit measurement types")
            .WithTags("Units");

        return app;
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Ok<PaginatedItems<CatalogItemDto>>> GetAllItemsV1(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] SalesItemServices services)
    {
        return await GetAllItems(paginationRequest, services, null, null, null);
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Ok<PaginatedItems<CatalogItemDto>>> GetAllItems(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] SalesItemServices services,
        [Description("The name of the item to return")] string? name,
        [Description("The type of items to return")] int? type,
        [Description("The brand of items to return")] int? brand)
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var root = services.Context.SalesItemItems
            .Include(c => c.SalesItemBrand)
            .Include(c => c.SalesItemType)
            .AsQueryable();

        if (name is not null)
        {
            root = root.Where(c => c.Name.StartsWith(name));
        }
        if (type is not null)
        {
            root = root.Where(c => c.SalesItemTypeId == type);
        }
        if (brand is not null)
        {
            root = root.Where(c => c.SalesItemBrandId == brand);
        }

        var totalItems = await root
            .LongCountAsync();

        var itemsOnPage = await root
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        await ApplyPreferredPricingAsync(services.Context, itemsOnPage);

        var dtoItems = itemsOnPage.Select(ToCatalogItemDto).ToList();

        return TypedResults.Ok(new PaginatedItems<CatalogItemDto>(pageIndex, pageSize, totalItems, dtoItems));
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Ok<List<CatalogItemDto>>> GetItemsByIds(
        [AsParameters] SalesItemServices services,
        [Description("List of ids for salesitem items to return")] int[] ids)
    {
        var items = await services.Context.SalesItemItems
            .Include(c => c.SalesItemBrand)
            .Include(c => c.SalesItemType)
            .Where(item => ids.Contains(item.Id))
            .ToListAsync();

        await ApplyPreferredPricingAsync(services.Context, items);
        return TypedResults.Ok(items.Select(ToCatalogItemDto).ToList());
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Results<Ok<CatalogItemDto>, NotFound, BadRequest<ProblemDetails>>> GetItemById(
        HttpContext httpContext,
        [AsParameters] SalesItemServices services,
        [Description("The salesitem item id")] int id)
    {
        if (id <= 0)
        {
            return TypedResults.BadRequest<ProblemDetails>(new (){
                Detail = "Id is not valid"
            });
        }

        var item = await services.Context.SalesItemItems
            .Include(ci => ci.SalesItemBrand)
            .Include(ci => ci.SalesItemType)
            .SingleOrDefaultAsync(ci => ci.Id == id);

        if (item == null)
        {
            return TypedResults.NotFound();
        }

        await ApplyPreferredPricingAsync(services.Context, [item]);

        return TypedResults.Ok(ToCatalogItemDto(item));
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Ok<PaginatedItems<CatalogItemDto>>> GetItemsByName(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] SalesItemServices services,
        [Description("The name of the item to return")] string name)
    {
        return await GetAllItems(paginationRequest, services, name, null, null);
    }

    [ProducesResponseType<byte[]>(StatusCodes.Status200OK, "application/octet-stream",
        [ "image/png", "image/gif", "image/jpeg", "image/bmp", "image/tiff",
          "image/wmf", "image/jp2", "image/svg+xml", "image/webp" ])]
    public static async Task<Results<PhysicalFileHttpResult,NotFound>> GetItemPictureById(
        SalesItemContext context,
        IWebHostEnvironment environment,
        [Description("The salesitem item id")] int id)
    {
        var item = await context.SalesItemItems.FindAsync(id);

        if (item is null || item.PictureFileName is null)
        {
            return TypedResults.NotFound();
        }

        var path = GetFullPath(environment.ContentRootPath, item.PictureFileName);

        string imageFileExtension = Path.GetExtension(item.PictureFileName) ?? string.Empty;
        string mimetype = GetImageMimeTypeFromImageFileExtension(imageFileExtension);
        DateTime lastModified = File.GetLastWriteTimeUtc(path);

        return TypedResults.PhysicalFile(path, mimetype, lastModified: lastModified);
    }

    //COMMENTED ON 2025-12-31 NO AI FIRST
    //[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    //public static async Task<Results<Ok<PaginatedItems<SalesItemItem>>, RedirectToRouteHttpResult>> GetItemsBySemanticRelevanceV1(
    //    [AsParameters] PaginationRequest paginationRequest,
    //    [AsParameters] SalesItemServices services,
    //    [Description("The text string to use when search for related items in the salesitem")] string text)

    //{
    //    return await GetItemsBySemanticRelevance(paginationRequest, services, text);
    //}
    //COMMENTED ON 2025-12-31 NO AI FIRST
    //[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    //public static async Task<Results<Ok<PaginatedItems<SalesItemItem>>, RedirectToRouteHttpResult>> GetItemsBySemanticRelevance(
    //    [AsParameters] PaginationRequest paginationRequest,
    //    [AsParameters] SalesItemServices services,
    //    [Description("The text string to use when search for related items in the salesitem"), Required, MinLength(1)] string text)
    //{
    //    var pageSize = paginationRequest.PageSize;
    //    var pageIndex = paginationRequest.PageIndex;

    //    if (!services.SalesItemAI.IsEnabled)
    //    {
    //        return await GetItemsByName(paginationRequest, services, text);
    //    }

    //    // Create an embedding for the input search
    //    var vector = await services.SalesItemAI.GetEmbeddingAsync(text);

    //    if (vector is null)
    //    {
    //        return await GetItemsByName(paginationRequest, services, text);
    //    }

    //    // Get the total number of items
    //    var totalItems = await services.Context.SalesItemItems
    //        .LongCountAsync();

    //    // Get the next page of items, ordered by most similar (smallest distance) to the input search
    //    List<SalesItemItem> itemsOnPage;
    //    if (services.Logger.IsEnabled(LogLevel.Debug))
    //    {
    //        var itemsWithDistance = await services.Context.SalesItemItems
    //            .Where(c => c.Embedding != null)
    //            .Select(c => new { Item = c, Distance = c.Embedding!.CosineDistance(vector) })
    //            .OrderBy(c => c.Distance)
    //            .Skip(pageSize * pageIndex)
    //            .Take(pageSize)
    //            .ToListAsync();

    //        services.Logger.LogDebug("Results from {text}: {results}", text, string.Join(", ", itemsWithDistance.Select(i => $"{i.Item.Name} => {i.Distance}")));

    //        itemsOnPage = itemsWithDistance.Select(i => i.Item).ToList();
    //    }
    //    else
    //    {
    //        itemsOnPage = await services.Context.SalesItemItems
    //            .Where(c => c.Embedding != null)
    //            .OrderBy(c => c.Embedding!.CosineDistance(vector))
    //            .Skip(pageSize * pageIndex)
    //            .Take(pageSize)
    //            .ToListAsync();
    //    }

    //    return TypedResults.Ok(new PaginatedItems<SalesItemItem>(pageIndex, pageSize, totalItems, itemsOnPage));
    //}

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Ok<PaginatedItems<CatalogItemDto>>> GetItemsByBrandAndTypeId(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] SalesItemServices services,
        [Description("The type of items to return")] int typeId,
        [Description("The brand of items to return")] int? brandId)
    {
        return await GetAllItems(paginationRequest, services, null, typeId, brandId);
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Ok<PaginatedItems<CatalogItemDto>>> GetItemsByBrandId(
        [AsParameters] PaginationRequest paginationRequest,
        [AsParameters] SalesItemServices services,
        [Description("The brand of items to return")] int? brandId)
    {
        return await GetAllItems(paginationRequest, services, null, null, brandId);
    }

    //COMMENTED ON 2025-12-31 NO AI FIRST
    //public static async Task<Results<Created, BadRequest<ProblemDetails>, NotFound<ProblemDetails>>> UpdateItemV1(
    //    HttpContext httpContext,
    //    [AsParameters] SalesItemServices services,
    //    SalesItemItem productToUpdate)
    //{
    //    if (productToUpdate?.Id == null)
    //    {
    //        return TypedResults.BadRequest<ProblemDetails>(new (){
    //            Detail = "Item id must be provided in the request body."
    //        });
    //    }
    //    return await UpdateItem(httpContext, productToUpdate.Id, services, productToUpdate);
    //}

    //COMMENTED ON 2025-12-31 NO AI FIRST
    //public static async Task<Results<Created, BadRequest<ProblemDetails>, NotFound<ProblemDetails>>> UpdateItem(
    //    HttpContext httpContext,
    //    [Description("The id of the salesitem item to delete")] int id,
    //    [AsParameters] SalesItemServices services,
    //    SalesItemItem productToUpdate)
    //{
    //    var salesitemItem = await services.Context.SalesItemItems.SingleOrDefaultAsync(i => i.Id == id);

    //    if (salesitemItem == null)
    //    {
    //        return TypedResults.NotFound<ProblemDetails>(new (){
    //            Detail = $"Item with id {id} not found."
    //        });
    //    }

    //    // Update current product
    //    var salesitemEntry = services.Context.Entry(salesitemItem);
    //    salesitemEntry.CurrentValues.SetValues(productToUpdate);

    //    salesitemItem.Embedding = await services.SalesItemAI.GetEmbeddingAsync(salesitemItem);

    //    var priceEntry = salesitemEntry.Property(i => i.Price);

    //    if (priceEntry.IsModified) // Save product's data and publish integration event through the Event Bus if price has changed
    //    {
    //        //Create Integration Event to be published through the Event Bus
    //        var priceChangedEvent = new ProductPriceChangedIntegrationEvent(salesitemItem.Id, productToUpdate.Price, priceEntry.OriginalValue);

    //        // Achieving atomicity between original SalesItem database operation and the IntegrationEventLog thanks to a local transaction
    //        await services.EventService.SaveEventAndSalesItemContextChangesAsync(priceChangedEvent);

    //        // Publish through the Event Bus and mark the saved event as published
    //        await services.EventService.PublishThroughEventBusAsync(priceChangedEvent);
    //    }
    //    else // Just save the updated product because the Product's Price hasn't changed.
    //    {
    //        await services.Context.SaveChangesAsync();
    //    }
    //    return TypedResults.Created($"/api/salesitem/items/{id}");
    //}

    //COMMENTED ON 2025-12-31 NO AI FIRST
    //[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    //public static async Task<Created> CreateItem(
    //    [AsParameters] SalesItemServices services,
    //    SalesItemItem product)
    //{
    //    var item = new SalesItemItem(product.Name)
    //    {
    //        Id = product.Id,
    //        SalesItemBrandId = product.SalesItemBrandId,
    //        SalesItemTypeId = product.SalesItemTypeId,
    //        Description = product.Description,
    //        PictureFileName = product.PictureFileName,
    //        Price = product.Price,
    //        AvailableStock = product.AvailableStock,
    //        RestockThreshold = product.RestockThreshold,
    //        MaxStockThreshold = product.MaxStockThreshold
    //    };
    //    item.Embedding = await services.SalesItemAI.GetEmbeddingAsync(item);

    //    services.Context.SalesItemItems.Add(item);
    //    await services.Context.SaveChangesAsync();

    //    return TypedResults.Created($"/api/salesitem/items/{item.Id}");
    //}

    public static async Task<Results<NoContent, NotFound>> DeleteItemById(
        [AsParameters] SalesItemServices services,
        [Description("The id of the salesitem item to delete")] int id)
    {
        var item = services.Context.SalesItemItems.SingleOrDefault(x => x.Id == id);

        if (item is null)
        {
            return TypedResults.NotFound();
        }

        services.Context.SalesItemItems.Remove(item);
        await services.Context.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    private static string GetImageMimeTypeFromImageFileExtension(string extension) => extension switch
    {
        ".png" => "image/png",
        ".gif" => "image/gif",
        ".jpg" or ".jpeg" => "image/jpeg",
        ".bmp" => "image/bmp",
        ".tiff" => "image/tiff",
        ".wmf" => "image/wmf",
        ".jp2" => "image/jp2",
        ".svg" => "image/svg+xml",
        ".webp" => "image/webp",
        _ => "application/octet-stream",
    };

    public static string GetFullPath(string contentRootPath, string pictureFileName) =>
        Path.Combine(contentRootPath, "Pics", pictureFileName);

    public static async Task<Ok<List<Currency>>> GetAllCurrencies(SalesItemContext context)
    {
        var currencies = await context.Currencies.OrderBy(c => c.Code).ToListAsync();
        return TypedResults.Ok(currencies);
    }

    public static async Task<Results<Ok<List<SalesitemPrice>>, NotFound>> GetItemPrices(
        int id,
        SalesItemContext context)
    {
        var item = await context.SalesItemItems.FindAsync(id);
        if (item is null) return TypedResults.NotFound();

        var prices = await context.SalesItemPrices
            .Where(p => p.SalesItemId == id)
            .Include(p => p.Currency)
            .OrderBy(p => p.CurrencyCode)
            .ToListAsync();

        return TypedResults.Ok(prices);
    }

    public static async Task<Results<Ok<SalesitemDimension>, NotFound>> GetItemDimensions(
        int id,
        SalesItemContext context)
    {
        var dimension = await context.SalesItemDimensions.FindAsync(id);
        if (dimension is null) return TypedResults.NotFound();
        return TypedResults.Ok(dimension);
    }

    public static async Task<Ok<List<UnitMeasurement>>> GetAllUnitMeasurements(SalesItemContext context)
    {
        var units = await context.UnitMeasurements.OrderBy(u => u.MeasurementType).ThenBy(u => u.Name).ToListAsync();
        return TypedResults.Ok(units);
    }

    private static async Task ApplyPreferredPricingAsync(SalesItemContext context, List<SalesItemItem> items)
    {
        if (items.Count == 0)
        {
            return;
        }

        var preferredCurrencyCode = await ResolvePreferredCurrencyCodeAsync(context);
        var itemIds = items.Select(i => i.Id).ToArray();

        var idrOverridesByItemId = await context.SalesItemPrices
            .Where(p => itemIds.Contains(p.SalesItemId) && p.CurrencyCode == "IDR")
            .ToDictionaryAsync(p => p.SalesItemId, p => p.Price);

        foreach (var item in items)
        {
            if (!idrOverridesByItemId.TryGetValue(item.Id, out var idrPrice))
            {
                continue;
            }

            if (preferredCurrencyCode == "IDR")
            {
                item.BaseCurrencyCode = "IDR";
                item.BasePrice = idrPrice;
                continue;
            }

            if (preferredCurrencyCode == "USD")
            {
                // Test mode rule: USD is derived from IDR by dividing by 2.
                item.BaseCurrencyCode = "USD";
                item.BasePrice = decimal.Round(idrPrice / 2m, 2, MidpointRounding.AwayFromZero);
            }
        }
    }

    private static async Task<string> ResolvePreferredCurrencyCodeAsync(SalesItemContext context)
    {
        var configuredCulture = await context.SystemConfigs
            .Where(c => c.ConfigKey == "culture")
            .Select(c => c.ConfigValue)
            .FirstOrDefaultAsync();

        if (string.IsNullOrWhiteSpace(configuredCulture))
        {
            return "USD";
        }

        try
        {
            var culture = CultureInfo.GetCultureInfo(configuredCulture);
            if (culture.Name.Equals("id-ID", StringComparison.OrdinalIgnoreCase) ||
                culture.TwoLetterISOLanguageName.Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                return "IDR";
            }
        }
        catch (CultureNotFoundException)
        {
            // Fall back to USD if the configured culture is invalid.
        }

        return "USD";
    }

    private static CatalogItemDto ToCatalogItemDto(SalesItemItem item)
        => new(
            Id: item.Id,
            Code: item.Code,
            Barcode: item.Barcode,
            Name: item.Name,
            Description: item.Description,
            BasePrice: item.BasePrice,
            BaseCurrencyCode: item.BaseCurrencyCode,
            PictureUrl: string.Empty,
            CatalogBrandId: item.SalesItemBrandId,
            CatalogBrand: new CatalogBrandDto(item.SalesItemBrandId, item.SalesItemBrand?.Brand ?? string.Empty),
            CatalogTypeId: item.SalesItemTypeId,
            CatalogType: new CatalogItemTypeDto(item.SalesItemTypeId, item.SalesItemType?.Type ?? string.Empty));

    public sealed record CatalogItemDto(
        int Id,
        string Code,
        string Barcode,
        string Name,
        string? Description,
        decimal BasePrice,
        string BaseCurrencyCode,
        string PictureUrl,
        int CatalogBrandId,
        CatalogBrandDto CatalogBrand,
        int CatalogTypeId,
        CatalogItemTypeDto CatalogType);

    public sealed record CatalogBrandDto(int Id, string Brand);
    public sealed record CatalogItemTypeDto(int Id, string Type);
}
