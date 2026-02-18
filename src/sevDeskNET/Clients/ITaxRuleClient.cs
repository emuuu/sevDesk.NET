using sevDeskNET.Models;

namespace sevDeskNET.Clients;

/// <summary>
/// Client for retrieving tax rules in sevDesk.
/// Provides read-only operations for listing and getting tax rules that define taxation behavior.
/// </summary>
public interface ITaxRuleClient
{
    /// <summary>
    /// Retrieves a paginated list of tax rules.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of tax rules.</returns>
    Task<SevDeskListResponse<TaxRule>> ListAsync(PaginationParameters? pagination = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single tax rule by its identifier.
    /// </summary>
    /// <param name="id">The tax rule identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested tax rule.</returns>
    Task<TaxRule> GetAsync(int id, CancellationToken ct = default);
}
