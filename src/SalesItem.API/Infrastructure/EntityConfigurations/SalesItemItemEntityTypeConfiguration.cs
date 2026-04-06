
namespace ICLAco.SalesItem.API.Infrastructure.EntityConfigurations
{
    public class SalesItemItemEntityTypeConfiguration : IEntityTypeConfiguration<SalesItemItem>
    {
        public void Configure(EntityTypeBuilder<SalesItemItem> builder)
        {
            builder.ToTable("salesitem", SalesItemContext.DefaultSchema);

            builder.Property(ci => ci.Name)
                .HasMaxLength(50);

            builder.Property(ci => ci.Code)
                .HasMaxLength(40);

            builder.Property(ci => ci.Barcode)
                .HasMaxLength(32);

            builder.Property(ci => ci.Embedding)
                .HasColumnType("nvarchar(max)");

            builder.Property(ci => ci.BasePrice)
                .HasColumnType("decimal(18,4)");

            builder.Property(ci => ci.BaseCurrencyCode)
                .HasMaxLength(3);

            builder.HasOne(ci => ci.BaseCurrency)
                .WithMany()
                .HasForeignKey(ci => ci.BaseCurrencyCode)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(ci => ci.Prices)
                .WithOne(p => p.SalesItem)
                .HasForeignKey(p => p.SalesItemId);

            builder.HasOne(ci => ci.SalesItemBrand)
                .WithMany();

            builder.HasOne(ci => ci.SalesItemType)
                .WithMany();

            builder.HasIndex(ci => ci.Name);
            builder.HasIndex(ci => ci.Code);
            builder.HasIndex(ci => ci.Barcode);
            builder.HasIndex(ci => ci.BaseCurrencyCode);
        }
    }
}
