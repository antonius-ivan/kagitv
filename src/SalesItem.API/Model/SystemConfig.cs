using System.ComponentModel.DataAnnotations;

namespace ICLAco.SalesItem.API.Model;

public class SystemConfig
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string ConfigKey { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ConfigValue { get; set; } = string.Empty;

    public DateTime UpdatedAtUtc { get; set; }
}