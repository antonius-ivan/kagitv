using System.ComponentModel.DataAnnotations;

namespace ICLAco.SalesItem.API.Model;

public class MenuRole
{
    public int MenuId { get; set; }

    [Required]
    [MaxLength(256)]
    public string RoleName { get; set; } = string.Empty;

    public Menu Menu { get; set; } = null!;
}