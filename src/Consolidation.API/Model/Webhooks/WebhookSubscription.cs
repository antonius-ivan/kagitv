using System.ComponentModel.DataAnnotations;

namespace Consolidation.API.Model.Webhooks;

public class WebhookSubscription
{
    public int Id { get; set; }

    public WebhookType Type { get; set; }

    public DateTime Date { get; set; }

    [Required]
    public string DestUrl { get; set; } = string.Empty;

    public string? Token { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
}