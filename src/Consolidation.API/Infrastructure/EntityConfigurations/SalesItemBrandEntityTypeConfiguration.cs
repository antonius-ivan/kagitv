namespace Consolidation.API.Infrastructure.EntityConfigurations
{
    public class SalesItemBrandEntityTypeConfiguration : IEntityTypeConfiguration<SalesitemBrand>
    {
        public void Configure(EntityTypeBuilder<SalesitemBrand> builder)
        {
            builder.ToTable("salesitem_brand", SalesItemContext.DefaultSchema);

            builder.Property(cb => cb.Brand)
                .HasMaxLength(100);
        }
    }
}
