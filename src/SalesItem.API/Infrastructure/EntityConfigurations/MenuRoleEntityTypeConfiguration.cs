namespace ICLAco.SalesItem.API.Infrastructure.EntityConfigurations;

public class MenuRoleEntityTypeConfiguration : IEntityTypeConfiguration<MenuRole>
{
    public void Configure(EntityTypeBuilder<MenuRole> builder)
    {
        builder.ToTable("menu_role", SalesItemContext.DefaultSchema);

        builder.HasKey(menuRole => new { menuRole.MenuId, menuRole.RoleName });

        builder.Property(menuRole => menuRole.RoleName)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(menuRole => menuRole.RoleName);
    }
}