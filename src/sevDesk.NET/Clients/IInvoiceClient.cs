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
    /// <param name="filter">Optional server-side filters (update timestamp, status, contact, invoice date range).</param>
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
    /// Retrieves the document-level discounts and surcharges of an invoice.
    /// </summary>
    /// <remarks>
    /// To read them together with the invoice, or for many invoices at once, pass
    /// <c>embed: "discounts"</c> to <see cref="GetAsync"/> or <see cref="ListAsync"/> and use
    /// <see cref="Invoice.Discounts"/> instead.
    /// </remarks>
    /// <param name="id">The invoice identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The discounts of the invoice; empty if it has none.</returns>
    /// <exception cref="NotSupportedException">
    /// The implementation predates document-level discounts and does not override this member.
    /// The client returned by <see cref="SevDeskClient"/> always does.
    /// </exception>
    Task<IReadOnlyList<DocumentDiscount>> GetDiscountsAsync(int id, CancellationToken ct = default) =>
        throw new NotSupportedException(DiscountsNotSupported);

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
    /// Saves an invoice together with its positions in a single transaction and reads the saved
    /// invoice back.
    /// </summary>
    /// <remarks>
    /// The read-back is a second request. If it fails, the invoice has already been created and
    /// <see cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException"/> reports that, so the call
    /// must not be repeated. Use
    /// <see cref="SaveInvoiceReferenceAsync(Invoice, IEnumerable{InvoicePos}, CancellationToken)"/>
    /// to skip the read-back entirely.
    /// </remarks>
    /// <param name="invoice">The invoice to save.</param>
    /// <param name="positions">The line item positions for the invoice.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The saved invoice.</returns>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException">
    /// The invoice was created, but reading it back failed. Do not save it again.
    /// </exception>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskApiException">
    /// The invoice was not created. Retrying is safe.
    /// </exception>
    Task<Invoice> SaveInvoiceAsync(Invoice invoice, IEnumerable<InvoicePos> positions, CancellationToken ct = default);

    /// <summary>
    /// Saves an invoice together with its positions and document-level discounts in a single
    /// transaction and reads the saved invoice back.
    /// </summary>
    /// <remarks>
    /// Behaves exactly like
    /// <see cref="SaveInvoiceAsync(Invoice, IEnumerable{InvoicePos}, CancellationToken)"/>,
    /// including the two-phase contract of
    /// <see cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException"/>. The discounts are
    /// sent in the <c>discountSave</c> array and are added to the invoice; discounts it already
    /// has are not removed. sevDesk calculates the discount amounts and the sums itself.
    /// </remarks>
    /// <param name="invoice">The invoice to save.</param>
    /// <param name="positions">The line item positions for the invoice.</param>
    /// <param name="discounts">
    /// The document-level discounts and surcharges to add. <see langword="null"/> or empty sends
    /// the same request as the overload without discounts.
    /// </param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The saved invoice.</returns>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException">
    /// The invoice was created, but reading it back failed. Do not save it again.
    /// </exception>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskApiException">
    /// The invoice was not created. Retrying is safe.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// <paramref name="discounts"/> is not empty and the implementation predates document-level
    /// discounts and does not override this member. Nothing was sent. The client returned by
    /// <see cref="SevDeskClient"/> always overrides it.
    /// </exception>
    Task<Invoice> SaveInvoiceAsync(Invoice invoice, IEnumerable<InvoicePos> positions, IEnumerable<DocumentDiscount>? discounts, CancellationToken ct = default) =>
        discounts is null || !discounts.Any()
            ? SaveInvoiceAsync(invoice, positions, ct)
            : throw new NotSupportedException(DiscountsNotSupported);

    /// <summary>
    /// Saves an invoice together with its positions in a single transaction and returns only the
    /// reference to it, without reading the invoice back.
    /// </summary>
    /// <remarks>
    /// One request instead of two. Use this when the identifier of the new invoice is all that is
    /// needed; it removes the read-back and with it the ambiguous failure window that
    /// <see cref="SaveInvoiceAsync(Invoice, IEnumerable{InvoicePos}, CancellationToken)"/> has to
    /// report through
    /// <see cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException"/>. Call
    /// <see cref="GetAsync(int, string, CancellationToken)"/> separately if the full invoice is
    /// needed later.
    /// </remarks>
    /// <param name="invoice">The invoice to save.</param>
    /// <param name="positions">The line item positions for the invoice.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A reference carrying the identifier of the saved invoice.</returns>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException">
    /// The invoice was created, but its identifier could not be read from the response. Do not save
    /// it again; look it up instead.
    /// </exception>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskApiException">
    /// The invoice was not created. Retrying is safe.
    /// </exception>
    Task<SevDeskObjectReference> SaveInvoiceReferenceAsync(Invoice invoice, IEnumerable<InvoicePos> positions, CancellationToken ct = default);

    /// <summary>
    /// Saves an invoice together with its positions and document-level discounts in a single
    /// transaction and returns only the reference to it, without reading the invoice back.
    /// </summary>
    /// <remarks>
    /// Behaves exactly like
    /// <see cref="SaveInvoiceReferenceAsync(Invoice, IEnumerable{InvoicePos}, CancellationToken)"/>.
    /// The discounts are sent in the <c>discountSave</c> array and are added to the invoice;
    /// discounts it already has are not removed.
    /// </remarks>
    /// <param name="invoice">The invoice to save.</param>
    /// <param name="positions">The line item positions for the invoice.</param>
    /// <param name="discounts">
    /// The document-level discounts and surcharges to add. <see langword="null"/> or empty sends
    /// the same request as the overload without discounts.
    /// </param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A reference carrying the identifier of the saved invoice.</returns>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException">
    /// The invoice was created, but its identifier could not be read from the response. Do not save
    /// it again; look it up instead.
    /// </exception>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskApiException">
    /// The invoice was not created. Retrying is safe.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// <paramref name="discounts"/> is not empty and the implementation predates document-level
    /// discounts and does not override this member. Nothing was sent. The client returned by
    /// <see cref="SevDeskClient"/> always overrides it.
    /// </exception>
    Task<SevDeskObjectReference> SaveInvoiceReferenceAsync(Invoice invoice, IEnumerable<InvoicePos> positions, IEnumerable<DocumentDiscount>? discounts, CancellationToken ct = default) =>
        discounts is null || !discounts.Any()
            ? SaveInvoiceReferenceAsync(invoice, positions, ct)
            : throw new NotSupportedException(DiscountsNotSupported);

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

    // Default implementations of the members added in 3.2.0, so that implementations written
    // against 3.1.0 keep compiling. They refuse to drop discounts silently.
    private const string DiscountsNotSupported =
        "This IInvoiceClient implementation does not support document-level discounts. " +
        "Implement GetDiscountsAsync and the SaveInvoiceAsync/SaveInvoiceReferenceAsync overloads " +
        "that take discounts, or use the client created by SevDeskClient.";
}
