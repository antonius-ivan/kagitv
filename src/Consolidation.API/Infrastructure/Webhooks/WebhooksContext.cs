using Consolidation.API.Model.Webhooks;

namespace Consolidation.API.Infrastructure.Webhooks;

/// <remarks>
/// Add migrations using the following command inside the 'Consolidation.API' project directory:
///
/// dotnet ef migrations add --context WebhooksContext [migration-name]
/// </remarks>
public class WebhooksContext(DbContextOptions<WebhooksContext> options) : DbContext(options)
{
    public const string DefaultSchema = "webhooksdb";
    private const string SubscriptionComment = "registered webhook endpoints by event type and subscribing user";

    public DbSet<WebhookSubscription> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DefaultSchema);

        modelBuilder.Entity<WebhookSubscription>(eb =>
        {
            eb.ToTable("subscription", tableBuilder => tableBuilder.HasComment(SubscriptionComment));
            eb.HasIndex(s => s.UserId);
            eb.HasIndex(s => s.Type);
        });
    }
}