using System.ComponentModel.DataAnnotations;

namespace ICLAco.SalesItem.API.Model.Wisata;

public class WisataUserRefreshToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    [Required]
    [MaxLength(255)]
    public string RefreshToken { get; set; } = string.Empty;

    public DateTime ExpiredAt { get; set; }

    public WisataUser User { get; set; } = null!;
}