using System.ComponentModel.DataAnnotations;

namespace ICLAco.SalesItem.API.Model;

public class UnitMeasurement
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string Symbol { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string MeasurementType { get; set; } = string.Empty;
}
