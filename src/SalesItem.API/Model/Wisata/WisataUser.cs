using System.ComponentModel.DataAnnotations;

namespace ICLAco.SalesItem.API.Model.Wisata;

public class WisataUser
{
    public int UserId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public ICollection<WisataUserRefreshToken> RefreshTokens { get; set; } = [];
}