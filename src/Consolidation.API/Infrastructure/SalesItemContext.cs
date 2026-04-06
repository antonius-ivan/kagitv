using Consolidation.API.Model.Sales;
using Consolidation.API.Infrastructure.EntityConfigurations;

namespace Consolidation.API.Infrastructure
{
    public class SalesItemContext : DbContext
    {
        public const string DefaultSchema = "sales";
        private const string CurrencyComment = "master currency list with ISO codes, symbols, and minor-unit precision";
        private const string ExchangeRateComment = "time-based exchange rates between source and target currencies";
        private const string SalesItemComment = "product catalog items with pricing, stock thresholds, and serialized embedding payload";
        private const string SalesItemBrandComment = "brand reference data for catalog items";
        private const string SalesItemTypeComment = "type or category reference data for catalog items";
        private const string SalesItemPriceComment = "currency-specific price entries with optional effective date ranges";
        private const string SalesItemDimensionComment = "physical dimensions, weight, and units for a catalog item";
        private const string UnitMeasurementComment = "reference units for length, weight, and quantity measurements";
        private const string SystemConfigComment = "key-value configuration entries used by the sales catalog subsystem";
        private const string ModuleComment = "ui control modules used to group ERP navigation areas";
        private const string MenuComment = "recursive menu tree used for role-filtered navigation in the UI control layer";
        private const string MenuRoleComment = "role-to-menu visibility mappings used to filter the UI menu tree";

        public SalesItemContext(DbContextOptions<SalesItemContext> options) : base(options)
        {

        }

        public required DbSet<SalesItemItem> SalesItemItems { get; set; }
        public required DbSet<SalesitemBrand> SalesItemBrands { get; set; }
        public required DbSet<SalesItemType> SalesItemTypes { get; set; }
        public required DbSet<Currency> Currencies { get; set; }
        public required DbSet<ExchangeRate> ExchangeRates { get; set; }
        public required DbSet<SalesitemPrice> SalesItemPrices { get; set; }
        public required DbSet<SystemConfig> SystemConfigs { get; set; }
        public required DbSet<UnitMeasurement> UnitMeasurements { get; set; }
        public required DbSet<SalesitemDimension> SalesItemDimensions { get; set; }
        public required DbSet<Module> Modules { get; set; }
        public required DbSet<Menu> Menus { get; set; }
        public required DbSet<MenuRole> MenuRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema(DefaultSchema);
            builder.ApplyConfiguration(new CurrencyEntityTypeConfiguration());
            builder.ApplyConfiguration(new ExchangeRateEntityTypeConfiguration());
            builder.ApplyConfiguration(new SystemConfigEntityTypeConfiguration());
            builder.ApplyConfiguration(new SalesItemBrandEntityTypeConfiguration());
            builder.ApplyConfiguration(new SalesItemTypeEntityTypeConfiguration());
            builder.ApplyConfiguration(new SalesItemItemEntityTypeConfiguration());
            builder.ApplyConfiguration(new SalesItemPriceEntityTypeConfiguration());
            builder.ApplyConfiguration(new UnitMeasurementEntityTypeConfiguration());
            builder.ApplyConfiguration(new SalesItemDimensionEntityTypeConfiguration());
            builder.ApplyConfiguration(new ModuleEntityTypeConfiguration());
            builder.ApplyConfiguration(new MenuEntityTypeConfiguration());
            builder.ApplyConfiguration(new MenuRoleEntityTypeConfiguration());

            builder.Entity<Currency>()
                .ToTable("currency", tableBuilder => tableBuilder.HasComment(CurrencyComment));
            builder.Entity<ExchangeRate>()
                .ToTable("exchange_rate", tableBuilder => tableBuilder.HasComment(ExchangeRateComment));
            builder.Entity<SalesItemItem>()
                .ToTable("salesitem", tableBuilder => tableBuilder.HasComment(SalesItemComment));
            builder.Entity<SalesitemBrand>()
                .ToTable("salesitem_brand", tableBuilder => tableBuilder.HasComment(SalesItemBrandComment));
            builder.Entity<SalesItemType>()
                .ToTable("salesitem_type", tableBuilder => tableBuilder.HasComment(SalesItemTypeComment));
            builder.Entity<SalesitemPrice>()
                .ToTable("salesitem_price", tableBuilder => tableBuilder.HasComment(SalesItemPriceComment));
            builder.Entity<SalesitemDimension>()
                .ToTable("salesitem_dimension", tableBuilder => tableBuilder.HasComment(SalesItemDimensionComment));
            builder.Entity<UnitMeasurement>()
                .ToTable("unit_measurement", tableBuilder => tableBuilder.HasComment(UnitMeasurementComment));
            builder.Entity<SystemConfig>()
                .ToTable("system_config", tableBuilder => tableBuilder.HasComment(SystemConfigComment));
            builder.Entity<Module>()
                .ToTable("module", tableBuilder => tableBuilder.HasComment(ModuleComment));
            builder.Entity<Menu>()
                .ToTable("menu", tableBuilder => tableBuilder.HasComment(MenuComment));
            builder.Entity<MenuRole>()
                .ToTable("menu_role", tableBuilder => tableBuilder.HasComment(MenuRoleComment));
        }
    }

}