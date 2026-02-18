namespace sevDeskNET.Models;

/// <summary>
/// Represents a currency exchange rate in sevDesk.
/// </summary>
public class CurrencyExchangeRate
{
    /// <summary>Gets the exchange rate ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the source currency (ISO 4217).</summary>
    public string? CurrencyFrom { get; init; }

    /// <summary>Gets or sets the target currency (ISO 4217).</summary>
    public string? CurrencyTo { get; init; }

    /// <summary>Gets or sets the exchange rate.</summary>
    public decimal? Rate { get; init; }

    /// <summary>Gets or sets the date of the exchange rate.</summary>
    public DateTime? Date { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }
}
