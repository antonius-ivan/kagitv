namespace ICLAco.SalesItem.API.Infrastructure.EntityConfigurations;

public class CurrencyEntityTypeConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable("currency", SalesItemContext.DefaultSchema);

        builder.HasKey(c => c.Code);

        builder.Property(c => c.Code)
            .HasMaxLength(3)
            .ValueGeneratedNever();

        builder.Property(c => c.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Symbol)
            .HasMaxLength(10)
            .IsRequired();

        builder.HasIndex(c => c.Code)
            .IsUnique();
    }
}
