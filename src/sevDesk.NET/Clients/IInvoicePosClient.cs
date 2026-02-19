using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

/// <summary>
/// Client for managing invoice positions (line items) in sevDesk.
/// Provides operations for creating, reading, updating, and deleting invoice positions.
/// </summary>
public interface IInvoicePosClient
{
    /// <summary>
    /// Retrieves a paginated list of invoice positions, optionally filtered by invoice.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="invoiceId">Optional invoice identifier to filter positions by.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of invoice positions.</returns>
    Task<SevDeskListResponse<InvoicePos>> ListAsync(PaginationParameters? pagination = null, int? invoiceId = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single invoice position by its identifier.
    /// </summary>
    /// <param name="id">The invoice position identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested invoice position.</returns>
    Task<InvoicePos> GetAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Creates a new invoice position.
    /// </summary>
    /// <param name="position">The invoice position to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created invoice position.</returns>
    Task<InvoicePos> CreateAsync(InvoicePos position, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing invoice position.
    /// </summary>
    /// <param name="id">The identifier of the invoice position to update.</param>
    /// <param name="position">The updated invoice position data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated invoice position.</returns>
    Task<InvoicePos> UpdateAsync(int id, InvoicePos position, CancellationToken ct = default);

    /// <summary>
    /// Deletes an invoice position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the invoice position to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    Task DeleteAsync(int id, CancellationToken ct = default);
}
