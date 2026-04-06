using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Consolidation.API.Model.Identity;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string CardNumber { get; set; } = string.Empty;

    [Required]
    public string SecurityNumber { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "Expiration should match a valid MM/YY value")]
    public string Expiration { get; set; } = string.Empty;

    [Required]
    public string CardHolderName { get; set; } = string.Empty;

    public int CardType { get; set; }

    [Required]
    public string Street { get; set; } = string.Empty;

    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty;

    [Required]
    public string Country { get; set; } = string.Empty;

    [Required]
    public string ZipCode { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;
}