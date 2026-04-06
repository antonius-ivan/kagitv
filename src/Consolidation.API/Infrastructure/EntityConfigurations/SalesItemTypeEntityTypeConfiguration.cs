namespace Consolidation.API.Infrastructure.EntityConfigurations
{
    public class SalesItemTypeEntityTypeConfiguration : IEntityTypeConfiguration<SalesItemType>
    {
        public void Configure(EntityTypeBuilder<SalesItemType> builder)
        {
            builder.ToTable("salesitem_type", SalesItemContext.DefaultSchema);

            builder.Property(cb => cb.Type)
                .HasMaxLength(100);
        }
    }
}
