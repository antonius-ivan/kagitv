//COMMENTED 2026-03-10
//using Microsoft.Extensions.Options;
//using System.Text.Json;
//using System.Text;

//namespace ICLAco.SalesItem.API.Infrastructure
//{
//    public class SalesItemContextSeed(
//        IWebHostEnvironment env,
//        IOptions<SalesItemOptions> settings,
//        ILogger<SalesItemContextSeed> logger) : IDbSeeder<SalesItemContext>
//    {
//        public async Task SeedAsync(SalesItemContext context)
//        {
//            var useCustomizationData = settings.Value.UseCustomizationData;
//            var contentRootPath = env.ContentRootPath;
//            var picturePath = env.WebRootPath;

//            // Workaround from https://github.com/npgsql/efcore.pg/issues/292#issuecomment-388608426
//            context.Database.OpenConnection();
//            ((NpgsqlConnection)context.Database.GetDbConnection()).ReloadTypes();

//            if (!context.Currencies.Any())
//            {
//                var currencies = new[]
//                {
//                    new Currency { Code = "USD", Name = "US Dollar",       Symbol = "$",  NumericCode = 840, MinorUnit = 2 },
//                    new Currency { Code = "IDR", Name = "Indonesian Rupiah", Symbol = "Rp", NumericCode = 360, MinorUnit = 0 },
//                    new Currency { Code = "EUR", Name = "Euro",             Symbol = "€",  NumericCode = 978, MinorUnit = 2 },
//                    new Currency { Code = "SGD", Name = "Singapore Dollar", Symbol = "S$", NumericCode = 702, MinorUnit = 2 },
//                    new Currency { Code = "MYR", Name = "Malaysian Ringgit", Symbol = "RM", NumericCode = 458, MinorUnit = 2 },
//                };
//                await context.Currencies.AddRangeAsync(currencies);
//                await context.SaveChangesAsync();
//                logger.LogInformation("Seeded {Count} currencies", currencies.Length);
//            }

//            if (!context.SystemConfigs.Any(c => c.ConfigKey == "culture"))
//            {
//                await context.SystemConfigs.AddAsync(new SystemConfig
//                {
//                    ConfigKey = "culture",
//                    ConfigValue = "id-ID", //en-US
//                    UpdatedAtUtc = DateTime.UtcNow
//                });
//                await context.SaveChangesAsync();
//                logger.LogInformation("Seeded default system config: culture=en-US");
//            }

//            if (!context.UnitMeasurements.Any())
//            {
//                var units = new[]
//                {
//                    new UnitMeasurement { Name = "Kilogram",   Symbol = "kg",  MeasurementType = "weight" },
//                    new UnitMeasurement { Name = "Gram",       Symbol = "g",   MeasurementType = "weight" },
//                    new UnitMeasurement { Name = "Centimeter", Symbol = "cm",  MeasurementType = "length" },
//                    new UnitMeasurement { Name = "Meter",      Symbol = "m",   MeasurementType = "length" },
//                    new UnitMeasurement { Name = "Piece",      Symbol = "pcs", MeasurementType = "quantity" },
//                    new UnitMeasurement { Name = "Box",        Symbol = "box", MeasurementType = "quantity" },
//                };
//                await context.UnitMeasurements.AddRangeAsync(units);
//                await context.SaveChangesAsync();
//                logger.LogInformation("Seeded {Count} unit measurements", units.Length);
//            }

//            if (!context.SalesItemItems.Any())
//            {
//                var sourcePath = Path.Combine(contentRootPath, "Setup", "salesitem.json");
//                var sourceJson = File.ReadAllText(sourcePath);
//                var sourceItems = JsonSerializer.Deserialize<SalesItemSourceEntry[]>(sourceJson) ?? Array.Empty<SalesItemSourceEntry>();

//                context.SalesItemBrands.RemoveRange(context.SalesItemBrands);
//                await context.SalesItemBrands.AddRangeAsync(sourceItems.Select(x => x.Brand).Distinct()
//                    .Where(brandName => brandName != null)
//                    .Select(brandName => new SalesitemBrand(brandName!)));
//                logger.LogInformation("Seeded salesitem with {NumBrands} brands", context.SalesItemBrands.Count());

//                context.SalesItemTypes.RemoveRange(context.SalesItemTypes);
//                await context.SalesItemTypes.AddRangeAsync(sourceItems.Select(x => x.Type).Distinct()
//                    .Where(typeName => typeName != null)
//                    .Select(typeName => new SalesItemType(typeName!)));
//                logger.LogInformation("Seeded salesitem with {NumTypes} types", context.SalesItemTypes.Count());

//                await context.SaveChangesAsync();

//                var brandIdsByName = await context.SalesItemBrands.ToDictionaryAsync(x => x.Brand, x => x.Id);
//                var typeIdsByName = await context.SalesItemTypes.ToDictionaryAsync(x => x.Type, x => x.Id);

//                var now = DateTime.UtcNow;
//                var salesItemItems = sourceItems
//                    .Where(source => source.Name != null && source.Brand != null && source.Type != null)
//                    .Select(source =>
//                    {
//                        var readableCode = BuildReadableCode(source.Id, source.Brand!, source.Type!, source.Name!);
//                        return new SalesItemItem(source.Name!)
//                        {
//                            Id = source.Id,
//                            Description = source.Description,
//                            BasePrice = source.Price,
//                            BaseCurrencyCode = "USD",
//                            SalesItemBrandId = brandIdsByName[source.Brand!],
//                            SalesItemTypeId = typeIdsByName[source.Type!],
//                            Code = readableCode,
//                            Barcode = BuildBarcodeFromCode(readableCode),
//                            AvailableStock = 100,
//                            MaxStockThreshold = 200,
//                            RestockThreshold = 10,
//                            PictureFileName = $"{source.Id}.webp",
//                            CreatedAtUtc = now,
//                            UpdatedAtUtc = now,
//                        };
//                    }).ToArray();

//                await context.SalesItemItems.AddRangeAsync(salesItemItems);
//                logger.LogInformation("Seeded sales item with {NumItems} items", salesItemItems.Length);
//                await context.SaveChangesAsync();

//                // Seed IDR price overrides using the IDRPrice from JSON
//                var idrPriceEntriesFromJson = sourceItems
//                    .Where(source => source.IDRPrice > 0)
//                    .ToDictionary(s => s.Id, s => s.IDRPrice);

//                var itemIds = await context.SalesItemItems
//                    .Select(si => new { si.Id })
//                    .ToListAsync();

//                var idrPrices = itemIds
//                    .Where(si => idrPriceEntriesFromJson.ContainsKey(si.Id))
//                    .Select(si => new SalesitemPrice
//                    {
//                        SalesItemId = si.Id,
//                        CurrencyCode = "IDR",
//                        Price = idrPriceEntriesFromJson[si.Id],
//                    }).ToList();

//                if (idrPrices.Any())
//                {
//                    await context.SalesItemPrices.AddRangeAsync(idrPrices);
//                    await context.SaveChangesAsync();
//                    logger.LogInformation("Seeded {Count} IDR price overrides", idrPrices.Count);
//                }
//            }

//            await EnsureReadableIdentifiersAsync(context, logger);
//        }

//        private static async Task EnsureReadableIdentifiersAsync(SalesItemContext context, ILogger logger)
//        {
//            var itemsToFix = await context.SalesItemItems
//                .Where(i => string.IsNullOrWhiteSpace(i.Code) || string.IsNullOrWhiteSpace(i.Barcode))
//                .ToListAsync();

//            if (!itemsToFix.Any())
//            {
//                return;
//            }

//            var brandById = await context.SalesItemBrands.ToDictionaryAsync(x => x.Id, x => x.Brand);
//            var typeById = await context.SalesItemTypes.ToDictionaryAsync(x => x.Id, x => x.Type);

//            foreach (var item in itemsToFix)
//            {
//                var brand = brandById.TryGetValue(item.SalesItemBrandId, out var brandName) ? brandName : "GEN";
//                var type = typeById.TryGetValue(item.SalesItemTypeId, out var typeName) ? typeName : "ITEM";
//                var code = BuildReadableCode(item.Id, brand, type, item.Name);

//                item.Code = code;
//                item.Barcode = BuildBarcodeFromCode(code);
//                item.UpdatedAtUtc = DateTime.UtcNow;
//            }

//            await context.SaveChangesAsync();
//            logger.LogInformation("Backfilled readable code/barcode for {Count} sales items", itemsToFix.Count);
//        }

//        private static string BuildReadableCode(int id, string brand, string type, string name)
//        {
//            var brandPart = NormalizeToken(brand, 3);
//            var typePart = NormalizeToken(type, 3);
//            var namePart = NormalizeToken(name, 4);
//            return $"SI-{brandPart}-{typePart}-{namePart}-{id:D4}";
//        }

//        private static string NormalizeToken(string value, int maxLength)
//        {
//            var alnum = new string(value
//                .ToUpperInvariant()
//                .Where(char.IsLetterOrDigit)
//                .ToArray());

//            if (string.IsNullOrWhiteSpace(alnum))
//            {
//                return new string('X', maxLength);
//            }

//            return alnum.Length <= maxLength
//                ? alnum.PadRight(maxLength, 'X')
//                : alnum[..maxLength];
//        }

//        private static string BuildBarcodeFromCode(string code)
//        {
//            var numericBody = new StringBuilder();
//            foreach (var ch in code)
//            {
//                if (char.IsDigit(ch))
//                {
//                    numericBody.Append(ch);
//                }
//                else if (char.IsLetter(ch))
//                {
//                    var mapped = ((ch - 'A' + 1) % 10).ToString();
//                    numericBody.Append(mapped);
//                }

//                if (numericBody.Length == 12)
//                {
//                    break;
//                }
//            }

//            while (numericBody.Length < 12)
//            {
//                numericBody.Append('0');
//            }

//            var body = numericBody.ToString();
//            var sum = 0;
//            for (var i = 0; i < body.Length; i++)
//            {
//                var digit = body[i] - '0';
//                sum += (i % 2 == 0) ? digit : digit * 3;
//            }

//            var checkDigit = (10 - (sum % 10)) % 10;
//            return body + checkDigit;
//        }

//        private class SalesItemSourceEntry
//        {
//            public int Id { get; set; }
//            public string? Type { get; set; }
//            public string? Brand { get; set; }
//            public string? Name { get; set; }
//            public string? Description { get; set; }
//            public decimal Price { get; set; }
//            public decimal IDRPrice { get; set; }
//        }
//    }
//}
