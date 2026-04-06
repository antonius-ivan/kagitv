namespace ICLAco.Identity.API.Data;

/// <remarks>
/// Add migrations using the following command inside the 'Identity.API' project directory:
///
/// dotnet ef migrations add [migration-name]
/// </remarks>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public const string DefaultSchema = "identity";

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema(DefaultSchema);
        builder.UseOpenIddict();
        builder.Entity<OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreApplication>()
            .ToTable("open_iddict_application");
        builder.Entity<OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreAuthorization>()
            .ToTable("open_iddict_authorization");
        builder.Entity<OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreScope>()
            .ToTable("open_iddict_scope");
        builder.Entity<OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreToken>()
            .ToTable("open_iddict_token");
        builder.Entity<ApplicationUser>().ToTable("asp_net_user");
        builder.Entity<IdentityRole>().ToTable("asp_net_role");
        builder.Entity<IdentityUserRole<string>>().ToTable("asp_net_user_role");
        builder.Entity<IdentityUserClaim<string>>().ToTable("asp_net_user_claim");
        builder.Entity<IdentityUserLogin<string>>().ToTable("asp_net_user_login");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("asp_net_role_claim");
        builder.Entity<IdentityUserToken<string>>().ToTable("asp_net_user_token");
    }
}
