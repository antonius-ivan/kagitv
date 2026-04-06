namespace Consolidation.API.Infrastructure.EntityConfigurations;

public class ExchangeRateEntityTypeConfiguration : IEntityTypeConfiguration<ExchangeRate>
{
    public void Configure(EntityTypeBuilder<ExchangeRate> builder)
    {
        builder.ToTable("exchange_rate", SalesItemContext.DefaultSchema);

        builder.Property(e => e.Rate)
            .HasColumnType("decimal(18,8)");

        builder.HasOne(e => e.FromCurrency)
            .WithMany()
            .HasForeignKey(e => e.FromCurrencyCode)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ToCurrency)
            .WithMany()
            .HasForeignKey(e => e.ToCurrencyCode)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.FromCurrencyCode, e.ToCurrencyCode, e.EffectiveUtc });
    }
}
