namespace sevDesk.NET.Models;

/// <summary>
/// Represents a currency exchange rate in sevDesk.
/// </summary>
public class CurrencyExchangeRate
{
    /// <summary>Gets the exchange rate ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the currency (ISO 4217).</summary>
    public string? Currency { get; init; }

    /// <summary>Gets or sets the exchange rate.</summary>
    public decimal? Rate { get; init; }

    /// <summary>Gets or sets the date of the exchange rate.</summary>
    public DateTime? Date { get; init; }
}
