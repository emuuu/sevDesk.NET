using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

/// <summary>
/// Client for managing order positions (line items) in sevDesk.
/// Provides operations for creating, reading, updating, and deleting order positions.
/// </summary>
public interface IOrderPosClient
{
    /// <summary>
    /// Retrieves a paginated list of order positions, optionally filtered by order.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="orderId">Optional order identifier to filter positions by.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of order positions.</returns>
    Task<SevDeskListResponse<OrderPos>> ListAsync(PaginationParameters? pagination = null, int? orderId = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single order position by its identifier.
    /// </summary>
    /// <param name="id">The order position identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested order position.</returns>
    Task<OrderPos> GetAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Creates a new order position.
    /// </summary>
    /// <param name="position">The order position to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created order position.</returns>
    Task<OrderPos> CreateAsync(OrderPos position, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing order position.
    /// </summary>
    /// <param name="id">The identifier of the order position to update.</param>
    /// <param name="position">The updated order position data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated order position.</returns>
    Task<OrderPos> UpdateAsync(int id, OrderPos position, CancellationToken ct = default);

    /// <summary>
    /// Deletes an order position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the order position to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    Task DeleteAsync(int id, CancellationToken ct = default);
}
