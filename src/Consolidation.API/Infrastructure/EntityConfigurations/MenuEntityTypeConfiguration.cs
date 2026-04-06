namespace Consolidation.API.Infrastructure.EntityConfigurations;

public class MenuEntityTypeConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("menu", SalesItemContext.DefaultSchema);

        builder.HasKey(menu => menu.Id);

        builder.Property(menu => menu.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(menu => menu.Path)
            .HasMaxLength(200);

        builder.Property(menu => menu.Icon)
            .HasMaxLength(60);

        builder.Property(menu => menu.IsEnabled)
            .HasDefaultValue(true);

        builder.HasOne(menu => menu.ParentMenu)
            .WithMany(parentMenu => parentMenu.Children)
            .HasForeignKey(menu => menu.ParentMenuId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(menu => menu.MenuRoles)
            .WithOne(menuRole => menuRole.Menu)
            .HasForeignKey(menuRole => menuRole.MenuId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(menu => new { menu.ModuleId, menu.ParentMenuId, menu.SortOrder });
    }
}