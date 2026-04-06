namespace Consolidation.API.Model.Sales;

public class ExchangeRate
{
    public int Id { get; set; }

    public string FromCurrencyCode { get; set; } = string.Empty;
    public Currency? FromCurrency { get; set; }

    public string ToCurrencyCode { get; set; } = string.Empty;
    public Currency? ToCurrency { get; set; }

    public decimal Rate { get; set; }

    public DateTime EffectiveUtc { get; set; }
}
