using Consolidation.API.Model.Ordering;
using Microsoft.EntityFrameworkCore;

namespace Consolidation.API.Infrastructure.Ordering;

public class OrderingContext : DbContext
{
    public const string DefaultSchema = "ordering";

    public OrderingContext(DbContextOptions<OrderingContext> options)
        : base(options)
    {
    }

    public DbSet<CardType> CardTypes => Set<CardType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DefaultSchema);

        modelBuilder.Entity<CardType>(builder =>
        {
            builder.ToTable("card_type");
            builder.HasKey(cardType => cardType.Id);

            builder.Property(cardType => cardType.Id)
                .ValueGeneratedNever();

            builder.Property(cardType => cardType.Name)
                .HasMaxLength(200)
                .IsRequired();
        });
    }
}