namespace ICLAco.SalesItem.API.Infrastructure.EntityConfigurations;

public class SystemConfigEntityTypeConfiguration : IEntityTypeConfiguration<SystemConfig>
{
    public void Configure(EntityTypeBuilder<SystemConfig> builder)
    {
        builder.ToTable("system_config", SalesItemContext.DefaultSchema);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.ConfigKey)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.ConfigValue)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.UpdatedAtUtc)
            .HasColumnType("datetime2")
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.HasIndex(c => c.ConfigKey)
            .IsUnique();
    }
}