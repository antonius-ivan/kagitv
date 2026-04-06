using Consolidation.API.Model.Wisata;

namespace Consolidation.API.Infrastructure.Wisata;

public class WisataContext(DbContextOptions<WisataContext> options) : DbContext(options)
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
            entity.ToTable("Wisata", tableBuilder => tableBuilder.HasComment("master daftar wisata untuk modul interview"));
            entity.HasKey(item => item.WisataId);
            entity.Property(item => item.WisataId).ValueGeneratedOnAdd();
            entity.Property(item => item.Nama).HasMaxLength(150).IsRequired();
            entity.Property(item => item.Kota).HasMaxLength(100).IsRequired();
            entity.Property(item => item.Harga).HasColumnType("decimal(18,2)");
            entity.Property(item => item.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
        });

        builder.Entity<WisataUser>(entity =>
        {
            entity.ToTable("Users", tableBuilder => tableBuilder.HasComment("user login modul wisata"));
            entity.HasKey(user => user.UserId);
            entity.Property(user => user.UserId).ValueGeneratedOnAdd();
            entity.Property(user => user.Username).HasMaxLength(50).IsRequired();
            entity.Property(user => user.PasswordHash).HasMaxLength(255).IsRequired();
            entity.Property(user => user.CreatedDate).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            entity.HasIndex(user => user.Username).IsUnique();
        });

        builder.Entity<WisataUserRefreshToken>(entity =>
        {
            entity.ToTable("UserRefreshToken", tableBuilder => tableBuilder.HasComment("refresh token aktif untuk login wisata"));
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