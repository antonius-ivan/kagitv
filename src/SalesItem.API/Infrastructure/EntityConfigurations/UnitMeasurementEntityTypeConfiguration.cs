namespace ICLAco.SalesItem.API.Infrastructure.EntityConfigurations;

public class UnitMeasurementEntityTypeConfiguration : IEntityTypeConfiguration<UnitMeasurement>
{
    public void Configure(EntityTypeBuilder<UnitMeasurement> builder)
    {
        builder.ToTable("unit_measurement", SalesItemContext.DefaultSchema);

        builder.Property(u => u.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.Symbol)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(u => u.MeasurementType)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(u => u.Symbol)
            .IsUnique();

        builder.HasIndex(u => u.MeasurementType);
    }
}
