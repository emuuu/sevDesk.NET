using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

/// <summary>
/// Client for retrieving currency exchange rates in sevDesk.
/// Provides read-only operations for listing and getting exchange rates between currencies.
/// </summary>
public interface ICurrencyExchangeRateClient
{
    /// <summary>
    /// Retrieves a paginated list of currency exchange rates.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of currency exchange rates.</returns>
    Task<SevDeskListResponse<CurrencyExchangeRate>> ListAsync(PaginationParameters? pagination = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single currency exchange rate by its identifier.
    /// </summary>
    /// <param name="id">The currency exchange rate identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested currency exchange rate.</returns>
    Task<CurrencyExchangeRate> GetAsync(int id, CancellationToken ct = default);
}
