namespace Consolidation.API.Infrastructure.EntityConfigurations;

public class ModuleEntityTypeConfiguration : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.ToTable("module", SalesItemContext.DefaultSchema);

        builder.HasKey(module => module.Id);

        builder.Property(module => module.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(module => module.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(module => module.IsEnabled)
            .HasDefaultValue(true);

        builder.HasIndex(module => module.Code)
            .IsUnique();

        builder.HasMany(module => module.Menus)
            .WithOne(menu => menu.Module)
            .HasForeignKey(menu => menu.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}