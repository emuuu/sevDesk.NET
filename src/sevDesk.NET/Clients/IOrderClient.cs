using sevDesk.NET.Models;
using sevDesk.NET.Models.Enums;

namespace sevDesk.NET.Clients;

/// <summary>
/// Client for managing orders in sevDesk.
/// Provides operations for creating, reading, updating, and deleting orders,
/// as well as sending, duplicating, and changing order status.
/// </summary>
public interface IOrderClient
{
    /// <summary>
    /// Retrieves a paginated list of orders.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="embed">Optional comma-separated list of related objects to embed in the response.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of orders.</returns>
    Task<SevDeskListResponse<Order>> ListAsync(PaginationParameters? pagination = null, string? embed = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single order by its identifier.
    /// </summary>
    /// <param name="id">The order identifier.</param>
    /// <param name="embed">Optional comma-separated list of related objects to embed in the response.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested order.</returns>
    Task<Order> GetAsync(int id, string? embed = null, CancellationToken ct = default);

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="order">The order to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created order.</returns>
    Task<Order> CreateAsync(Order order, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    /// <param name="id">The identifier of the order to update.</param>
    /// <param name="order">The updated order data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated order.</returns>
    Task<Order> UpdateAsync(int id, Order order, CancellationToken ct = default);

    /// <summary>
    /// Deletes an order by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the order to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    Task DeleteAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Saves an order together with its positions in a single transaction.
    /// </summary>
    /// <param name="order">The order to save.</param>
    /// <param name="positions">The line item positions for the order.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The saved order.</returns>
    Task<Order> SaveOrderAsync(Order order, IEnumerable<OrderPos> positions, CancellationToken ct = default);

    /// <summary>
    /// Changes the status of an order.
    /// </summary>
    /// <param name="id">The identifier of the order.</param>
    /// <param name="status">The new status to set.</param>
    /// <param name="ct">Cancellation token.</param>
    Task ChangeStatusAsync(int id, OrderStatus status, CancellationToken ct = default);

    /// <summary>
    /// Downloads the PDF representation of an order.
    /// </summary>
    /// <param name="id">The identifier of the order.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The PDF file content as a byte array.</returns>
    Task<byte[]> GetPdfAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Sends an order via email.
    /// </summary>
    /// <param name="id">The identifier of the order to send.</param>
    /// <param name="email">The recipient email address.</param>
    /// <param name="subject">The email subject line.</param>
    /// <param name="text">The email body text.</param>
    /// <param name="ct">Cancellation token.</param>
    Task SendViaEmailAsync(int id, string email, string subject, string text, CancellationToken ct = default);

    /// <summary>
    /// Creates a duplicate of an existing order.
    /// </summary>
    /// <param name="id">The identifier of the order to duplicate.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The duplicated order.</returns>
    Task<Order> DuplicateAsync(int id, CancellationToken ct = default);
}
