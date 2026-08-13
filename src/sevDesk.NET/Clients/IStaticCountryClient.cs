using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

/// <summary>
/// Client for retrieving countries from the sevDesk country catalogue.
/// Provides read-only operations for resolving the country references carried by
/// contact addresses and invoices.
/// </summary>
public interface IStaticCountryClient
{
    /// <summary>
    /// Retrieves a paginated list of countries.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of countries.</returns>
    Task<SevDeskListResponse<StaticCountry>> ListAsync(PaginationParameters? pagination = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single country by its identifier.
    /// </summary>
    /// <param name="id">The country identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested country.</returns>
    Task<StaticCountry> GetAsync(int id, CancellationToken ct = default);
}
