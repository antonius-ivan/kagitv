using ICLAco.SalesItem.API.Model.Wisata;

namespace ICLAco.SalesItem.API.Infrastructure.Wisata;

public class WisataDbContext(DbContextOptions<WisataDbContext> options) : DbContext(options)
{
    public const string DefaultSchema = "wisata";

    public DbSet<WisataItem> Wisata => Set<WisataItem>();

    public DbSet<WisataUser> Users => Set<WisataUser>();

    public DbSet<WisataUserRefreshToken> UserRefreshTokens => Set<WisataUserRefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(DefaultSchema);

        builder.Entity<WisataItem>(entity =>
        {
            entity.ToTable("Wisata");
            entity.HasKey(item => item.WisataId);
            entity.Property(item => item.WisataId).ValueGeneratedOnAdd();
            entity.Property(item => item.Nama).HasMaxLength(150).IsRequired();
            entity.Property(item => item.Kota).HasMaxLength(100).IsRequired();
            entity.Property(item => item.Harga).HasColumnType("decimal(18,2)");
            entity.Property(item => item.CreatedDate).HasColumnType("datetime");
        });

        builder.Entity<WisataUser>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(user => user.UserId);
            entity.Property(user => user.UserId).ValueGeneratedOnAdd();
            entity.Property(user => user.Username).HasMaxLength(50).IsRequired();
            entity.Property(user => user.PasswordHash).HasMaxLength(255).IsRequired();
            entity.Property(user => user.CreatedDate).HasColumnType("datetime");
            entity.HasIndex(user => user.Username).IsUnique();
        });

        builder.Entity<WisataUserRefreshToken>(entity =>
        {
            entity.ToTable("UserRefreshToken");
            entity.HasKey(token => token.Id);
            entity.Property(token => token.Id).ValueGeneratedOnAdd();
            entity.Property(token => token.RefreshToken).HasMaxLength(255).IsRequired();
            entity.Property(token => token.ExpiredAt).HasColumnType("datetime");
            entity.HasIndex(token => token.RefreshToken).IsUnique();
            entity.HasOne(token => token.User)
                .WithMany(user => user.RefreshTokens)
                .HasForeignKey(token => token.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}