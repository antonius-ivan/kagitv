using ICLAco.SalesItem.API.Infrastructure.EntityConfigurations;
using ICLAco.SalesItem.API.Model;

namespace ICLAco.SalesItem.API.Infrastructure
{
    public class SalesItemContext : DbContext
    {
        public const string DefaultSchema = "sales";

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
        }
    }

}