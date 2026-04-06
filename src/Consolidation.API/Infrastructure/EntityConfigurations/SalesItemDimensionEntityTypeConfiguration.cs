namespace Consolidation.API.Infrastructure.EntityConfigurations;

public class SalesItemDimensionEntityTypeConfiguration : IEntityTypeConfiguration<SalesitemDimension>
{
    public void Configure(EntityTypeBuilder<SalesitemDimension> builder)
    {
        builder.ToTable("salesitem_dimension", SalesItemContext.DefaultSchema);

        builder.HasKey(d => d.SalesItemId);

        builder.Property(d => d.Length).HasColumnType("decimal(18,4)");
        builder.Property(d => d.Width).HasColumnType("decimal(18,4)");
        builder.Property(d => d.Height).HasColumnType("decimal(18,4)");
        builder.Property(d => d.Weight).HasColumnType("decimal(18,4)");

        builder.HasOne(d => d.SalesItem)
            .WithOne(si => si.Dimension)
            .HasForeignKey<SalesitemDimension>(d => d.SalesItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.DimensionUnit)
            .WithMany()
            .HasForeignKey(d => d.DimensionUnitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.WeightUnit)
            .WithMany()
            .HasForeignKey(d => d.WeightUnitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.QuantityUnit)
            .WithMany()
            .HasForeignKey(d => d.QuantityUnitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(d => d.DimensionUnitId);
        builder.HasIndex(d => d.WeightUnitId);
        builder.HasIndex(d => d.QuantityUnitId);
    }
}
