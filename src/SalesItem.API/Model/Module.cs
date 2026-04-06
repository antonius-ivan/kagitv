using System.ComponentModel.DataAnnotations;

namespace ICLAco.SalesItem.API.Model;

public class Module
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; } = true;

    public int SortOrder { get; set; }

    public ICollection<Menu> Menus { get; set; } = [];
}