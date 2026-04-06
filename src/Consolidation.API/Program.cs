using Consolidation.API;
using Consolidation.API.Infrastructure;
using Consolidation.API.Infrastructure.Identity;
using Consolidation.API.Infrastructure.Ordering;
using Consolidation.API.Infrastructure.Wisata;
using Consolidation.API.Infrastructure.Webhooks;
using Consolidation.API.Model.Identity;
using ICLAco.ServiceDefaults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
builder.Services.AddDataProtection();

// Temporary staffing-driven centralization: startup migration and seeding run only here.
builder.AddCentralizedDatabaseManagement();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/database-management/status", () => Results.Ok(new
{
    mode = "centralized",
    owner = "Consolidation.API",
    schemas = new[] { "identity", "ordering", "sales", "wisata", "webhooksdb", "public" }
}));

app.Run();

static class CentralizedDatabaseManagementExtensions
{
    public static void AddCentralizedDatabaseManagement(this IHostApplicationBuilder builder)
    {
        builder.AddIdentityDatabaseManagement();
        builder.AddOrderingDatabaseManagement();
        builder.AddSalesDatabaseManagement();
        builder.AddWisataDatabaseManagement();
        builder.AddWebhooksDatabaseManagement();
    }

    private static void AddIdentityDatabaseManagement(this IHostApplicationBuilder builder)
    {
        var connectionString = GetRequiredConnectionString(builder.Configuration, "identitydb");

        builder.Services.AddDbContext<ApplicationDbContext>(dbContextOptionsBuilder =>
        {
            dbContextOptionsBuilder.UseSqlServer(connectionString, sqlServerOptionsBuilder =>
            {
                sqlServerOptionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory", ApplicationDbContext.DefaultSchema);
            });

            dbContextOptionsBuilder.UseSnakeCaseNamingConvention();
            dbContextOptionsBuilder.UseOpenIddict();
        });

        builder.Services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<ApplicationDbContext>();
            });

        builder.Services.AddScoped<UsersSeed>();
        builder.Services.AddMigration<ApplicationDbContext>((context, services) =>
            services.GetRequiredService<UsersSeed>().SeedAsync(context));
    }

    private static void AddOrderingDatabaseManagement(this IHostApplicationBuilder builder)
    {
        var connectionString = GetRequiredConnectionString(builder.Configuration, "orderingdb");

        builder.Services.AddDbContext<OrderingContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlServerOptionsBuilder =>
            {
                sqlServerOptionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory", OrderingContext.DefaultSchema);
            });
            options.UseSnakeCaseNamingConvention();
        });

        builder.Services.AddScoped<OrderingContextSeed>();
        builder.Services.AddMigration<OrderingContext>((context, services) =>
            services.GetRequiredService<OrderingContextSeed>().SeedAsync(context));
    }

    private static void AddSalesDatabaseManagement(this IHostApplicationBuilder builder)
    {
        var connectionString = GetRequiredConnectionString(builder.Configuration, "salesitemdb");

        builder.Services.AddDbContext<SalesItemContext>(dbContextOptionsBuilder =>
        {
            dbContextOptionsBuilder.UseSqlServer(connectionString, sqlServerOptionsBuilder =>
            {
                sqlServerOptionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory", SalesItemContext.DefaultSchema);
            });

            dbContextOptionsBuilder.UseSnakeCaseNamingConvention();
        });

        builder.Services.Configure<SalesItemOptions>(builder.Configuration);
        builder.Services.AddScoped<SalesItemContextSeed>();
        builder.Services.AddMigration<SalesItemContext>((context, services) =>
            services.GetRequiredService<SalesItemContextSeed>().SeedAsync(context));
    }

    private static void AddWisataDatabaseManagement(this IHostApplicationBuilder builder)
    {
        var connectionString = GetRequiredConnectionString(builder.Configuration, "wisatadb");

        builder.Services.AddDbContext<WisataContext>(dbContextOptionsBuilder =>
        {
            dbContextOptionsBuilder.UseSqlServer(connectionString, sqlServerOptionsBuilder =>
            {
                sqlServerOptionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory", WisataContext.DefaultSchema);
            });
        });

        builder.Services.AddScoped<WisataContextSeed>();
        builder.Services.AddMigration<WisataContext>((context, services) =>
            services.GetRequiredService<WisataContextSeed>().SeedAsync(context));
    }

    private static void AddWebhooksDatabaseManagement(this IHostApplicationBuilder builder)
    {
        var connectionString = GetRequiredConnectionString(builder.Configuration, "webhooksdb");

        builder.Services.AddDbContext<WebhooksContext>(dbContextOptionsBuilder =>
        {
            dbContextOptionsBuilder.UseSqlServer(connectionString, sqlServerOptionsBuilder =>
            {
                sqlServerOptionsBuilder.MigrationsHistoryTable("__ef_migrations_history", WebhooksContext.DefaultSchema);
            });

            dbContextOptionsBuilder.UseSnakeCaseNamingConvention();
        });

        builder.Services.AddMigration<WebhooksContext>();
    }

    private static string GetRequiredConnectionString(IConfiguration configuration, string name)
    {
        return configuration.GetConnectionString(name)
            ?? throw new InvalidOperationException($"Connection string '{name}' was not found.");
    }
}
