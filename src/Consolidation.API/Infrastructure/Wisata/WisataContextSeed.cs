using Consolidation.API.Model.Wisata;

namespace Consolidation.API.Infrastructure.Wisata;

public class WisataContextSeed(ILogger<WisataContextSeed> logger) : IDbSeeder<WisataContext>
{
    private const string DefaultPassword = "P@ssw0rd!123";
    private const string AdminPasswordHash = "AQAAAAIAAYagAAAAEDKawVqikvfq8Xcng1QIKZpUnmGLzIZEPevfl4WATzBpcnxWfyks0WRifWjhL+CJiQ==";
    private const string OperatorPasswordHash = "AQAAAAIAAYagAAAAEKe/G3+uTMlWpz6CT6N0lvXfCec09UUCBtCnjFQXp6tc/4mgBpEu5A7NyiFtZuxhkA==";
    private const string ViewerPasswordHash = "AQAAAAIAAYagAAAAEBnM8GIUhHkL3QfhhZ+RjaVvnsZFMrd74DcNtWqy56wy+VLm9Mz80qOg5LEGA00SWw==";

    public async Task SeedAsync(WisataContext context)
    {
        if (!await context.Wisata.AnyAsync())
        {
            await context.Wisata.AddRangeAsync(CreateWisataSeed());
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {Count} wisata rows.", await context.Wisata.CountAsync());
        }

        if (!await context.Users.AnyAsync())
        {
            var users = CreateUsersSeed();
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {Count} wisata users with default password {Password}.", users.Count, DefaultPassword);
        }

        if (await context.UserRefreshTokens.AnyAsync())
        {
            context.UserRefreshTokens.RemoveRange(context.UserRefreshTokens);
            await context.SaveChangesAsync();
        }
    }

    public static string GetDefaultPassword() => DefaultPassword;

    private static List<WisataItem> CreateWisataSeed()
    {
        var createdDate = new DateTime(2025, 12, 19, 8, 0, 0, DateTimeKind.Utc);

        return
        [
            new WisataItem { Nama = "Pantai Boom", Kota = "Banyuwangi", Harga = 25000m, CreatedDate = createdDate },
            new WisataItem { Nama = "Candi Borobudur", Kota = "Magelang", Harga = 100000m, CreatedDate = createdDate },
            new WisataItem { Nama = "Gunung Bromo", Kota = "Pasuruan", Harga = 75000m, CreatedDate = createdDate },
            new WisataItem { Nama = "Pantai Parangtritis", Kota = "Bantul", Harga = 20000m, CreatedDate = createdDate },
            new WisataItem { Nama = "Kawah Ijen", Kota = "Banyuwangi", Harga = 35000m, CreatedDate = createdDate },
            new WisataItem { Nama = "Taman Mini Indonesia Indah", Kota = "Jakarta Timur", Harga = 25000m, CreatedDate = createdDate },
            new WisataItem { Nama = "Pulau Komodo", Kota = "Labuan Bajo", Harga = 150000m, CreatedDate = createdDate },
            new WisataItem { Nama = "Raja Ampat", Kota = "Sorong", Harga = 250000m, CreatedDate = createdDate },
            new WisataItem { Nama = "Pantai Tanjung Tinggi", Kota = "Belitung", Harga = 30000m, CreatedDate = createdDate },
            new WisataItem { Nama = "Bukit Teletubbies", Kota = "Bromo", Harga = 20000m, CreatedDate = createdDate },
            new WisataItem { Nama = "Goa Pindul", Kota = "Gunungkidul", Harga = 40000m, CreatedDate = createdDate },
            new WisataItem { Nama = "Pantai Losari", Kota = "Makassar", Harga = 15000m, CreatedDate = createdDate }
        ];
    }

    private static List<WisataUser> CreateUsersSeed()
    {
        return
        [
            new WisataUser { Username = "admin", PasswordHash = AdminPasswordHash, CreatedDate = DateTime.UtcNow },
            new WisataUser { Username = "operator", PasswordHash = OperatorPasswordHash, CreatedDate = DateTime.UtcNow },
            new WisataUser { Username = "viewer", PasswordHash = ViewerPasswordHash, CreatedDate = DateTime.UtcNow }
        ];
    }
}