using sevDeskNET.Models;

namespace sevDeskNET.Clients;

/// <summary>
/// Client for retrieving units of measurement in sevDesk.
/// Provides read-only operations for listing and getting unities (e.g., pieces, hours, kilograms).
/// </summary>
public interface IUnityClient
{
    /// <summary>
    /// Retrieves a paginated list of unities.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of unities.</returns>
    Task<SevDeskListResponse<Unity>> ListAsync(PaginationParameters? pagination = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single unity by its identifier.
    /// </summary>
    /// <param name="id">The unity identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested unity.</returns>
    Task<Unity> GetAsync(int id, CancellationToken ct = default);
}
