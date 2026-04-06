using System.ComponentModel.DataAnnotations;

namespace Consolidation.API.Model.Sales;

public class Menu
{
    public int Id { get; set; }

    public int ModuleId { get; set; }

    public int? ParentMenuId { get; set; }

    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Path { get; set; }

    [MaxLength(60)]
    public string? Icon { get; set; }

    public bool IsEnabled { get; set; } = true;

    public int SortOrder { get; set; }

    public Module Module { get; set; } = null!;

    public Menu? ParentMenu { get; set; }

    public ICollection<Menu> Children { get; set; } = [];

    public ICollection<MenuRole> MenuRoles { get; set; } = [];
}