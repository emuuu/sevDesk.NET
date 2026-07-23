using sevDesk.NET.Models;
using sevDesk.NET.Models.Enums;

namespace sevDesk.NET.Clients;

/// <summary>
/// Client for managing invoices in sevDesk.
/// Provides operations for creating, reading, updating, and deleting invoices,
/// as well as sending, duplicating, cancelling, and booking invoices.
/// </summary>
public interface IInvoiceClient
{
    /// <summary>
    /// Retrieves a paginated list of invoices.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="embed">Optional comma-separated list of related objects to embed in the response.</param>
    /// <param name="filter">Optional server-side filters (update timestamp, status, contact).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of invoices.</returns>
    Task<SevDeskListResponse<Invoice>> ListAsync(PaginationParameters? pagination = null, string? embed = null, InvoiceListFilter? filter = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single invoice by its identifier.
    /// </summary>
    /// <param name="id">The invoice identifier.</param>
    /// <param name="embed">Optional comma-separated list of related objects to embed in the response.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested invoice.</returns>
    Task<Invoice> GetAsync(int id, string? embed = null, CancellationToken ct = default);

    /// <summary>
    /// Creates a new invoice.
    /// </summary>
    /// <param name="invoice">The invoice to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created invoice.</returns>
    Task<Invoice> CreateAsync(Invoice invoice, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing invoice.
    /// </summary>
    /// <param name="id">The identifier of the invoice to update.</param>
    /// <param name="invoice">The updated invoice data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated invoice.</returns>
    Task<Invoice> UpdateAsync(int id, Invoice invoice, CancellationToken ct = default);

    /// <summary>
    /// Deletes an invoice by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the invoice to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    Task DeleteAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Saves an invoice together with its positions in a single transaction.
    /// </summary>
    /// <param name="invoice">The invoice to save.</param>
    /// <param name="positions">The line item positions for the invoice.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The saved invoice.</returns>
    Task<Invoice> SaveInvoiceAsync(Invoice invoice, IEnumerable<InvoicePos> positions, CancellationToken ct = default);

    /// <summary>
    /// Changes the status of an invoice.
    /// </summary>
    /// <param name="id">The identifier of the invoice.</param>
    /// <param name="status">The new status to set.</param>
    /// <param name="ct">Cancellation token.</param>
    Task ChangeStatusAsync(int id, InvoiceStatus status, CancellationToken ct = default);

    /// <summary>
    /// Downloads the PDF representation of an invoice.
    /// </summary>
    /// <param name="id">The identifier of the invoice.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The PDF file content as a byte array.</returns>
    Task<byte[]> GetPdfAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Sends an invoice via email.
    /// </summary>
    /// <param name="id">The identifier of the invoice to send.</param>
    /// <param name="email">The recipient email address.</param>
    /// <param name="subject">The email subject line.</param>
    /// <param name="text">The email body text.</param>
    /// <param name="ct">Cancellation token.</param>
    Task SendViaEmailAsync(int id, string email, string subject, string text, CancellationToken ct = default);

    /// <summary>
    /// Creates a duplicate of an existing invoice.
    /// </summary>
    /// <param name="id">The identifier of the invoice to duplicate.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The duplicated invoice.</returns>
    Task<Invoice> DuplicateAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Cancels an invoice.
    /// </summary>
    /// <param name="id">The identifier of the invoice to cancel.</param>
    /// <param name="ct">Cancellation token.</param>
    Task CancelAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Marks an invoice as sent without actually sending it.
    /// </summary>
    /// <param name="id">The identifier of the invoice to mark as sent.</param>
    /// <param name="ct">Cancellation token.</param>
    Task MarkAsSentAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Books a payment amount against an invoice.
    /// </summary>
    /// <param name="id">The identifier of the invoice to book against.</param>
    /// <param name="amount">The payment amount to book.</param>
    /// <param name="checkAccountId">The identifier of the check account used for payment.</param>
    /// <param name="date">The date of the payment.</param>
    /// <param name="ct">Cancellation token.</param>
    Task BookAmountAsync(int id, decimal amount, int checkAccountId, DateTime date, CancellationToken ct = default);
}
