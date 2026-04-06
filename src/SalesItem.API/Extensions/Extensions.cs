using ICLAco.SalesItem.API.Infrastructure;
using ICLAco.SalesItem.API.Infrastructure.Wisata;
using ICLAco.SalesItem.API.Model.Wisata;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SalesItem.API.Extensions
{
    public static class Extensions
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            builder.AddDefaultAuthentication();

            var salesItemConnectionString = builder.Configuration.GetConnectionString("salesitemdb")
                ?? throw new InvalidOperationException("Connection string 'salesitemdb' was not found.");

            builder.Services.AddDbContext<SalesItemContext>(dbContextOptionsBuilder =>
            {
                dbContextOptionsBuilder.UseSqlServer(salesItemConnectionString, sqlServerOptionsBuilder =>
                {
                    sqlServerOptionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory", SalesItemContext.DefaultSchema);
                });

                dbContextOptionsBuilder.UseSnakeCaseNamingConvention();
            });

            //COMMENTED 2026-03-10 Temporary centralization: startup migration and seeding are managed by Consolidation.API.
            //builder.Services.AddMigration<SalesItemContext, SalesItemContextSeed>();

            builder.Services.AddScoped<IMenuTreeService, MenuTreeService>();

            var wisataConnectionString = builder.Configuration.GetConnectionString("wisatadb")
                ?? builder.Configuration.GetConnectionString("salesitemdb")
                ?? throw new InvalidOperationException("Connection string 'wisatadb' was not found.");

            builder.Services.AddDbContext<WisataDbContext>(dbContextOptionsBuilder =>
            {
                dbContextOptionsBuilder.UseSqlServer(wisataConnectionString, sqlServerOptionsBuilder =>
                {
                    sqlServerOptionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory", WisataDbContext.DefaultSchema);
                });
            });

            builder.Services.Configure<WisataJwtOptions>(builder.Configuration.GetSection(WisataJwtOptions.SectionName));
            builder.Services.AddScoped<IPasswordHasher<WisataUser>, PasswordHasher<WisataUser>>();
            builder.Services.AddScoped<WisataAuthService>();

            var wisataJwtOptions = builder.Configuration.GetSection(WisataJwtOptions.SectionName).Get<WisataJwtOptions>()
                ?? throw new InvalidOperationException("WisataJwt configuration is missing.");
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(wisataJwtOptions.SigningKey));

            builder.Services.AddAuthentication()
                .AddJwtBearer("WisataJwt", options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = wisataJwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = wisataJwtOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        NameClaimType = JwtRegisteredClaimNames.UniqueName,
                        RoleClaimType = ClaimTypes.Role
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("WisataJwtPolicy", policy =>
                {
                    policy.AddAuthenticationSchemes("WisataJwt");
                    policy.RequireAuthenticatedUser();
                });
            });
        }

    }
}
