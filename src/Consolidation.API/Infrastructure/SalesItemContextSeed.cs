using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Consolidation.API.Infrastructure
{
    public class SalesItemContextSeed(
        IWebHostEnvironment env,
        IOptions<SalesItemOptions> settings,
        ILogger<SalesItemContextSeed> logger) : IDbSeeder<SalesItemContext>
    {
        public async Task SeedAsync(SalesItemContext context)
        {
            var useCustomizationData = settings.Value.UseCustomizationData;
            var contentRootPath = env.ContentRootPath;
            var picturePath = env.WebRootPath;

            if (!context.Currencies.Any())
            {
                var currencies = new[]
                {
                    new Currency { Code = "USD", Name = "US Dollar",       Symbol = "$",  NumericCode = 840, MinorUnit = 2 },
                    new Currency { Code = "IDR", Name = "Indonesian Rupiah", Symbol = "Rp", NumericCode = 360, MinorUnit = 0 },
                    new Currency { Code = "EUR", Name = "Euro",             Symbol = "€",  NumericCode = 978, MinorUnit = 2 },
                    new Currency { Code = "SGD", Name = "Singapore Dollar", Symbol = "S$", NumericCode = 702, MinorUnit = 2 },
                    new Currency { Code = "MYR", Name = "Malaysian Ringgit", Symbol = "RM", NumericCode = 458, MinorUnit = 2 },
                };
                await context.Currencies.AddRangeAsync(currencies);
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded {Count} currencies", currencies.Length);
            }

            if (!context.SystemConfigs.Any(c => c.ConfigKey == "culture"))
            {
                await context.SystemConfigs.AddAsync(new SystemConfig
                {
                    ConfigKey = "culture",
                    ConfigValue = "id-ID", //en-US
                    UpdatedAtUtc = DateTime.UtcNow
                });
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded default system config: culture=en-US");
            }

            if (!context.UnitMeasurements.Any())
            {
                var units = new[]
                {
                    new UnitMeasurement { Name = "Kilogram",   Symbol = "kg",  MeasurementType = "weight" },
                    new UnitMeasurement { Name = "Gram",       Symbol = "g",   MeasurementType = "weight" },
                    new UnitMeasurement { Name = "Centimeter", Symbol = "cm",  MeasurementType = "length" },
                    new UnitMeasurement { Name = "Meter",      Symbol = "m",   MeasurementType = "length" },
                    new UnitMeasurement { Name = "Piece",      Symbol = "pcs", MeasurementType = "quantity" },
                    new UnitMeasurement { Name = "Box",        Symbol = "box", MeasurementType = "quantity" },
                };
                await context.UnitMeasurements.AddRangeAsync(units);
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded {Count} unit measurements", units.Length);
            }

            if (!context.SalesItemItems.Any())
            {
                var sourcePath = Path.Combine(contentRootPath, "Setup", "salesitem.json");
                var sourceJson = File.ReadAllText(sourcePath);
                var sourceItems = JsonSerializer.Deserialize<SalesItemSourceEntry[]>(sourceJson) ?? Array.Empty<SalesItemSourceEntry>();

                context.SalesItemBrands.RemoveRange(context.SalesItemBrands);
                await context.SalesItemBrands.AddRangeAsync(sourceItems.Select(x => x.Brand).Distinct()
                    .Where(brandName => brandName != null)
                    .Select(brandName => new SalesitemBrand(brandName!)));
                logger.LogInformation("Seeded salesitem with {NumBrands} brands", context.SalesItemBrands.Count());

                context.SalesItemTypes.RemoveRange(context.SalesItemTypes);
                await context.SalesItemTypes.AddRangeAsync(sourceItems.Select(x => x.Type).Distinct()
                    .Where(typeName => typeName != null)
                    .Select(typeName => new SalesItemType(typeName!)));
                logger.LogInformation("Seeded salesitem with {NumTypes} types", context.SalesItemTypes.Count());

                await context.SaveChangesAsync();

                var brandIdsByName = await context.SalesItemBrands.ToDictionaryAsync(x => x.Brand, x => x.Id);
                var typeIdsByName = await context.SalesItemTypes.ToDictionaryAsync(x => x.Type, x => x.Id);

                var now = DateTime.UtcNow;
                var seededSalesItems = sourceItems
                    .Where(source => source.Name != null && source.Brand != null && source.Type != null)
                    .Select(source =>
                    {
                        var readableCode = BuildReadableCode(source.Id, source.Brand!, source.Type!, source.Name!);
                        return new
                        {
                            Source = source,
                            Item = new SalesItemItem(source.Name!)
                            {
                                Description = source.Description,
                                BasePrice = source.Price,
                                BaseCurrencyCode = "USD",
                                SalesItemBrandId = brandIdsByName[source.Brand!],
                                SalesItemTypeId = typeIdsByName[source.Type!],
                                Code = readableCode,
                                Barcode = BuildBarcodeFromCode(readableCode),
                                AvailableStock = 100,
                                MaxStockThreshold = 200,
                                RestockThreshold = 10,
                                PictureFileName = $"{source.Id}.webp",
                                CreatedAtUtc = now,
                                UpdatedAtUtc = now,
                            }
                        };
                    }).ToArray();

                await context.SalesItemItems.AddRangeAsync(seededSalesItems.Select(entry => entry.Item));
                logger.LogInformation("Seeded sales item with {NumItems} items", seededSalesItems.Length);
                await context.SaveChangesAsync();

                var idrPrices = seededSalesItems
                    .Where(entry => entry.Source.IDRPrice > 0)
                    .Select(entry => new SalesitemPrice
                    {
                        SalesItemId = entry.Item.Id,
                        CurrencyCode = "IDR",
                        Price = entry.Source.IDRPrice,
                    }).ToList();

                if (idrPrices.Any())
                {
                    await context.SalesItemPrices.AddRangeAsync(idrPrices);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Seeded {Count} IDR price overrides", idrPrices.Count);
                }
            }

            await SeedUiControlLayerAsync(context, logger);

            await EnsureReadableIdentifiersAsync(context, logger);
        }

        private static async Task SeedUiControlLayerAsync(SalesItemContext context, ILogger logger)
        {
            var salesModule = await EnsureModuleAsync(context, "SALES", "Sales", 10);
            var orderingModule = await EnsureModuleAsync(context, "ORDERING", "Ordering", 20);

            var dashboardMenu = await EnsureMenuAsync(context, salesModule, null, "Dashboard", "/dashboard", "board", 5);
            var ordersMenu = await EnsureMenuAsync(context, salesModule, null, "Orders", null, "stack", 10);
            var ordersListMenu = await EnsureMenuAsync(context, salesModule, ordersMenu, "List", "/user/orders", "list", 10);
            var ordersCreateMenu = await EnsureMenuAsync(context, salesModule, ordersMenu, "Create", "/checkout", "plus", 20);
            var ordersReturnsMenu = await EnsureMenuAsync(context, salesModule, ordersMenu, "Returns", "/dashboard/returns", "arrow-reset", 30);

            var catalogMenu = await EnsureMenuAsync(context, salesModule, null, "Catalog", null, "box", 20);
            var catalogProductsMenu = await EnsureMenuAsync(context, salesModule, catalogMenu, "Products", "/salesitem", "tag", 10);
            var catalogPricingMenu = await EnsureMenuAsync(context, salesModule, catalogMenu, "Pricing", "/dashboard/catalog/pricing", "money", 20);
            var catalogInventoryMenu = await EnsureMenuAsync(context, salesModule, catalogMenu, "Inventory", "/dashboard/catalog/inventory", "cube", 30);

            var customersMenu = await EnsureMenuAsync(context, salesModule, null, "Customer Service", null, "people", 30);
            var invoicesMenu = await EnsureMenuAsync(context, salesModule, customersMenu, "Invoices", "/dashboard/invoices", "receipt", 10);
            var supportQueueMenu = await EnsureMenuAsync(context, salesModule, customersMenu, "Support Queue", "/dashboard/customers/support", "chat", 20);

            var reportsMenu = await EnsureMenuAsync(context, salesModule, null, "Reports", "/dashboard/reports", "chart", 40);
            var orderingOverviewMenu = await EnsureMenuAsync(context, orderingModule, null, "Overview", "/dashboard/ordering", "truck", 10);
            var fulfillmentMenu = await EnsureMenuAsync(context, orderingModule, null, "Fulfillment", null, "vehicle-truck", 20);
            var fulfillmentQueueMenu = await EnsureMenuAsync(context, orderingModule, fulfillmentMenu, "Queue", "/dashboard/ordering/queue", "clock", 10);
            var fulfillmentDispatchMenu = await EnsureMenuAsync(context, orderingModule, fulfillmentMenu, "Dispatch", "/dashboard/ordering/dispatch", "send", 20);
            var fulfillmentExceptionsMenu = await EnsureMenuAsync(context, orderingModule, fulfillmentMenu, "Exceptions", "/dashboard/ordering/exceptions", "warning", 30);

            await EnsureMenuRolesAsync(context, dashboardMenu, ["Admin", "Staff"]);
            await EnsureMenuRolesAsync(context, ordersMenu, ["Admin", "Staff"]);
            await EnsureMenuRolesAsync(context, ordersListMenu, ["Admin", "Staff"]);
            await EnsureMenuRolesAsync(context, ordersCreateMenu, ["Admin", "Staff"]);
            await EnsureMenuRolesAsync(context, ordersReturnsMenu, ["Admin", "Staff"]);
            await EnsureMenuRolesAsync(context, catalogMenu, ["Admin", "Staff"]);
            await EnsureMenuRolesAsync(context, catalogProductsMenu, ["Admin", "Staff"]);
            await EnsureMenuRolesAsync(context, catalogPricingMenu, ["Admin", "Staff"]);
            await EnsureMenuRolesAsync(context, catalogInventoryMenu, ["Admin", "Staff"]);
            await EnsureMenuRolesAsync(context, customersMenu, ["Admin", "Staff"]);
            await EnsureMenuRolesAsync(context, invoicesMenu, ["Admin", "Staff"]);
            await EnsureMenuRolesAsync(context, supportQueueMenu, ["Admin", "Staff"]);
            await EnsureMenuRolesAsync(context, reportsMenu, ["Admin"]);
            await EnsureMenuRolesAsync(context, orderingOverviewMenu, ["Admin"]);
            await EnsureMenuRolesAsync(context, fulfillmentMenu, ["Admin"]);
            await EnsureMenuRolesAsync(context, fulfillmentQueueMenu, ["Admin"]);
            await EnsureMenuRolesAsync(context, fulfillmentDispatchMenu, ["Admin"]);
            await EnsureMenuRolesAsync(context, fulfillmentExceptionsMenu, ["Admin"]);

            logger.LogInformation("Ensured UI control layer modules and menu tree are seeded.");
        }

        private static async Task<Module> EnsureModuleAsync(SalesItemContext context, string code, string name, int sortOrder)
        {
            var module = await context.Modules.FirstOrDefaultAsync(existingModule => existingModule.Code == code);
            if (module is null)
            {
                module = new Module
                {
                    Code = code,
                    Name = name,
                    SortOrder = sortOrder,
                    IsEnabled = true
                };

                await context.Modules.AddAsync(module);
                await context.SaveChangesAsync();
                return module;
            }

            module.Name = name;
            module.SortOrder = sortOrder;
            module.IsEnabled = true;
            await context.SaveChangesAsync();
            return module;
        }

        private static async Task<Menu> EnsureMenuAsync(
            SalesItemContext context,
            Module module,
            Menu? parentMenu,
            string name,
            string? path,
            string? icon,
            int sortOrder)
        {
            var parentMenuId = parentMenu?.Id;
            var menu = await context.Menus
                .FirstOrDefaultAsync(existingMenu =>
                    existingMenu.ModuleId == module.Id &&
                    existingMenu.ParentMenuId == parentMenuId &&
                    existingMenu.Name == name);

            if (menu is null)
            {
                menu = new Menu
                {
                    ModuleId = module.Id,
                    ParentMenuId = parentMenuId,
                    Name = name,
                    Path = path,
                    Icon = icon,
                    SortOrder = sortOrder,
                    IsEnabled = true
                };

                await context.Menus.AddAsync(menu);
                await context.SaveChangesAsync();
                return menu;
            }

            menu.Path = path;
            menu.Icon = icon;
            menu.SortOrder = sortOrder;
            menu.IsEnabled = true;
            await context.SaveChangesAsync();
            return menu;
        }

        private static async Task EnsureMenuRolesAsync(SalesItemContext context, Menu menu, IReadOnlyCollection<string> roleNames)
        {
            var existingRoleNames = await context.MenuRoles
                .Where(menuRole => menuRole.MenuId == menu.Id)
                .Select(menuRole => menuRole.RoleName)
                .ToListAsync();

            var missingRoleNames = roleNames
                .Where(roleName => existingRoleNames.Contains(roleName, StringComparer.OrdinalIgnoreCase) is false)
                .ToArray();

            if (missingRoleNames.Length == 0)
            {
                return;
            }

            await context.MenuRoles.AddRangeAsync(missingRoleNames.Select(roleName => new MenuRole
            {
                MenuId = menu.Id,
                RoleName = roleName
            }));
            await context.SaveChangesAsync();
        }

        private static async Task EnsureReadableIdentifiersAsync(SalesItemContext context, ILogger logger)
        {
            var itemsToFix = await context.SalesItemItems
                .Where(i => string.IsNullOrWhiteSpace(i.Code) || string.IsNullOrWhiteSpace(i.Barcode))
                .ToListAsync();

            if (!itemsToFix.Any())
            {
                return;
            }

            var brandById = await context.SalesItemBrands.ToDictionaryAsync(x => x.Id, x => x.Brand);
            var typeById = await context.SalesItemTypes.ToDictionaryAsync(x => x.Id, x => x.Type);

            foreach (var item in itemsToFix)
            {
                var brand = brandById.TryGetValue(item.SalesItemBrandId, out var brandName) ? brandName : "GEN";
                var type = typeById.TryGetValue(item.SalesItemTypeId, out var typeName) ? typeName : "ITEM";
                var code = BuildReadableCode(item.Id, brand, type, item.Name);

                item.Code = code;
                item.Barcode = BuildBarcodeFromCode(code);
                item.UpdatedAtUtc = DateTime.UtcNow;
            }

            await context.SaveChangesAsync();
            logger.LogInformation("Backfilled readable code/barcode for {Count} sales items", itemsToFix.Count);
        }

        private static string BuildReadableCode(int id, string brand, string type, string name)
        {
            var brandPart = NormalizeToken(brand, 3);
            var typePart = NormalizeToken(type, 3);
            var namePart = NormalizeToken(name, 4);
            return $"SI-{brandPart}-{typePart}-{namePart}-{id:D4}";
        }

        private static string NormalizeToken(string value, int maxLength)
        {
            var alnum = new string(value
                .ToUpperInvariant()
                .Where(char.IsLetterOrDigit)
                .ToArray());

            if (string.IsNullOrWhiteSpace(alnum))
            {
                return new string('X', maxLength);
            }

            return alnum.Length <= maxLength
                ? alnum.PadRight(maxLength, 'X')
                : alnum[..maxLength];
        }

        private static string BuildBarcodeFromCode(string code)
        {
            var numericBody = new StringBuilder();
            foreach (var ch in code)
            {
                if (char.IsDigit(ch))
                {
                    numericBody.Append(ch);
                }
                else if (char.IsLetter(ch))
                {
                    var mapped = ((ch - 'A' + 1) % 10).ToString();
                    numericBody.Append(mapped);
                }

                if (numericBody.Length == 12)
                {
                    break;
                }
            }

            while (numericBody.Length < 12)
            {
                numericBody.Append('0');
            }

            var body = numericBody.ToString();
            var sum = 0;
            for (var i = 0; i < body.Length; i++)
            {
                var digit = body[i] - '0';
                sum += (i % 2 == 0) ? digit : digit * 3;
            }

            var checkDigit = (10 - (sum % 10)) % 10;
            return body + checkDigit;
        }

        private class SalesItemSourceEntry
        {
            public int Id { get; set; }
            public string? Type { get; set; }
            public string? Brand { get; set; }
            public string? Name { get; set; }
            public string? Description { get; set; }
            public decimal Price { get; set; }
            public decimal IDRPrice { get; set; }
        }
    }
}