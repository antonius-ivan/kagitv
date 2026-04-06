namespace Consolidation.API.Model.Sales;

public class SalesitemDimension
{
    public int SalesItemId { get; set; }
    public SalesItemItem? SalesItem { get; set; }

    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }

    public int? DimensionUnitId { get; set; }
    public UnitMeasurement? DimensionUnit { get; set; }

    public decimal? Weight { get; set; }

    public int? WeightUnitId { get; set; }
    public UnitMeasurement? WeightUnit { get; set; }

    public int? QuantityUnitId { get; set; }
    public UnitMeasurement? QuantityUnit { get; set; }
}
