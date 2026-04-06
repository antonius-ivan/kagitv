using System.ComponentModel.DataAnnotations;

namespace ICLAco.SalesItem.API.Model;

public class SalesItemType
{
    public SalesItemType(string type) {
        Type = type;
    }

    public int Id { get; set; }

    [Required]
    public string Type { get; set; }
}
