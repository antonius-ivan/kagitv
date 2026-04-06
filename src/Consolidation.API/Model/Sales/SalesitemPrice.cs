namespace Consolidation.API.Model.Sales;

public class SalesitemPrice
{
    public int Id { get; set; }

    public int SalesItemId { get; set; }
    public SalesItemItem? SalesItem { get; set; }

    public string CurrencyCode { get; set; } = string.Empty;
    public Currency? Currency { get; set; }

    public decimal Price { get; set; }

    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
}
