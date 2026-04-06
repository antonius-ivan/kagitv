using System.ComponentModel.DataAnnotations;

namespace ICLAco.SalesItem.API.Model;

public class SalesitemBrand
{
    public SalesitemBrand(string brand) {
        Brand = brand;
    }

    public int Id { get; set; }

    [Required]
    public string Brand { get; set; }
}
