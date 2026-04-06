namespace ICLAco.SalesItem.API.Infrastructure.EntityConfigurations;

public class SalesItemPriceEntityTypeConfiguration : IEntityTypeConfiguration<SalesitemPrice>
{
    public void Configure(EntityTypeBuilder<SalesitemPrice> builder)
    {
        builder.ToTable("salesitem_price", SalesItemContext.DefaultSchema);

        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,4)");

        builder.HasOne(p => p.SalesItem)
            .WithMany(si => si.Prices)
            .HasForeignKey(p => p.SalesItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Currency)
            .WithMany()
            .HasForeignKey(p => p.CurrencyCode)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => new { p.SalesItemId, p.CurrencyCode })
            .IsUnique();
    }
}
