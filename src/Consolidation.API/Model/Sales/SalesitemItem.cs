using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Consolidation.API.Model.Sales;

public class SalesItemItem
{
    public int Id { get; set; }

    [MaxLength(40)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(32)]
    public string Barcode { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }

    public decimal BasePrice { get; set; }

    [MaxLength(3)]
    public string BaseCurrencyCode { get; set; } = string.Empty;

    public Currency? BaseCurrency { get; set; }

    public string? PictureFileName { get; set; }

    public int SalesItemTypeId { get; set; }

    public SalesItemType? SalesItemType { get; set; }

    public int SalesItemBrandId { get; set; }

    public SalesitemBrand? SalesItemBrand { get; set; }

    // Per-currency price overrides
    private readonly List<SalesitemPrice> _prices = new();
    public IReadOnlyCollection<SalesitemPrice> Prices => _prices.AsReadOnly();

    // Physical dimensions & weight
    public SalesitemDimension? Dimension { get; set; }

    public DateTime? CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

    // Quantity in stock
    public int AvailableStock { get; set; }

    // Available stock at which we should reorder
    public int RestockThreshold { get; set; }


    // Maximum number of units that can be in-stock at any time (due to physicial/logistical constraints in warehouses)
    public int MaxStockThreshold { get; set; }

    /// <summary>Optional serialized embedding payload for the catalog item's description.</summary>
    [JsonIgnore]
    public string? Embedding { get; set; }

    /// <summary>
    /// True if item is on reorder
    /// </summary>
    public bool OnReorder { get; set; }

    public SalesItemItem(string name) { Name = name; }


    /// <summary>
    /// Decrements the quantity of a particular item in inventory and ensures the restockThreshold hasn't
    /// been breached. If so, a RestockRequest is generated in CheckThreshold. 
    /// 
    /// If there is sufficient stock of an item, then the integer returned at the end of this call should be the same as quantityDesired. 
    /// In the event that there is not sufficient stock available, the method will remove whatever stock is available and return that quantity to the client.
    /// In this case, it is the responsibility of the client to determine if the amount that is returned is the same as quantityDesired.
    /// It is invalid to pass in a negative number. 
    /// </summary>
    /// <param name="quantityDesired"></param>
    /// <returns>int: Returns the number actually removed from stock. </returns>
    /// 
    public int RemoveStock(int quantityDesired)
    {
        if (AvailableStock == 0)
        {
            //throw new SalesItemDomainException($"Empty stock, product item {Name} is sold out");
        }

        if (quantityDesired <= 0)
        {
            //throw new SalesItemDomainException($"Item units desired should be greater than zero");
        }

        int removed = Math.Min(quantityDesired, this.AvailableStock);

        this.AvailableStock -= removed;

        return removed;
    }

    /// <summary>
    /// Increments the quantity of a particular item in inventory.
    /// <param name="quantity"></param>
    /// <returns>int: Returns the quantity that has been added to stock</returns>
    /// </summary>
    public int AddStock(int quantity)
    {
        int original = this.AvailableStock;

        // The quantity that the client is trying to add to stock is greater than what can be physically accommodated in the Warehouse
        if ((this.AvailableStock + quantity) > this.MaxStockThreshold)
        {
            // For now, this method only adds new units up maximum stock threshold. In an expanded version of this application, we
            //could include tracking for the remaining units and store information about overstock elsewhere. 
            this.AvailableStock += (this.MaxStockThreshold - this.AvailableStock);
        }
        else
        {
            this.AvailableStock += quantity;
        }

        this.OnReorder = false;

        return this.AvailableStock - original;
    }
}
