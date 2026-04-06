using System.ComponentModel.DataAnnotations;

namespace Consolidation.API.Model.Sales;

public class Currency
{
    [Key]
    [MaxLength(3)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string Symbol { get; set; } = string.Empty;

    public int? NumericCode { get; set; }

    public int MinorUnit { get; set; }
}
