using System.ComponentModel.DataAnnotations;

namespace Consolidation.API.Model.Wisata;

public class WisataItem
{
    public int WisataId { get; set; }

    [Required]
    [MaxLength(150)]
    public string Nama { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Kota { get; set; } = string.Empty;

    public decimal Harga { get; set; }

    public DateTime CreatedDate { get; set; }
}