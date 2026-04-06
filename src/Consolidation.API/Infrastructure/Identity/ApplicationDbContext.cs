using Consolidation.API.Model.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Consolidation.API.Infrastructure.Identity;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public const string DefaultSchema = "identity";
    private const string AspNetRoleComment = "application roles used for authorization (e.g., Admin, User)";
    private const string AspNetRoleClaimComment = "claims attached to roles for role-based permissions";
    private const string AspNetUserComment = "application users stored by ASP.NET Identity";
    private const string AspNetUserClaimComment = "claims assigned directly to users (user-specific permissions or attributes)";
    private const string AspNetUserLoginComment = "external login providers linked to a user (Google, Facebook, etc.)";
    private const string AspNetUserRoleComment = "many-to-many (BridgeTable) relationship between users and roles";
    private const string AspNetUserTokenComment = "tokens associated with users (password reset tokens, refresh tokens, etc.)";
    private const string OpenIddictApplicationComment = "registered clients (Angular, mobile apps, APIs)";
    private const string OpenIddictAuthorizationComment = "user consent / login sessions";
    private const string OpenIddictScopeComment = "allowed API scopes";
    private const string OpenIddictTokenComment = "access tokens, refresh tokens";

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
            .ToTable("open_iddict_application", tableBuilder => tableBuilder.HasComment(OpenIddictApplicationComment));
        builder.Entity<OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreAuthorization>()
            .ToTable("open_iddict_authorization", tableBuilder => tableBuilder.HasComment(OpenIddictAuthorizationComment));
        builder.Entity<OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreScope>()
            .ToTable("open_iddict_scope", tableBuilder => tableBuilder.HasComment(OpenIddictScopeComment));
        builder.Entity<OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreToken>()
            .ToTable("open_iddict_token", tableBuilder => tableBuilder.HasComment(OpenIddictTokenComment));
        builder.Entity<ApplicationUser>()
            .ToTable("asp_net_user", tableBuilder => tableBuilder.HasComment(AspNetUserComment));
        builder.Entity<IdentityRole>()
            .ToTable("asp_net_role", tableBuilder => tableBuilder.HasComment(AspNetRoleComment));
        builder.Entity<IdentityUserRole<string>>()
            .ToTable("asp_net_user_role", tableBuilder => tableBuilder.HasComment(AspNetUserRoleComment));
        builder.Entity<IdentityUserClaim<string>>()
            .ToTable("asp_net_user_claim", tableBuilder => tableBuilder.HasComment(AspNetUserClaimComment));
        builder.Entity<IdentityUserLogin<string>>()
            .ToTable("asp_net_user_login", tableBuilder => tableBuilder.HasComment(AspNetUserLoginComment));
        builder.Entity<IdentityRoleClaim<string>>()
            .ToTable("asp_net_role_claim", tableBuilder => tableBuilder.HasComment(AspNetRoleClaimComment));
        builder.Entity<IdentityUserToken<string>>()
            .ToTable("asp_net_user_token", tableBuilder => tableBuilder.HasComment(AspNetUserTokenComment));
    }
}