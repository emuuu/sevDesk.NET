using sevDesk.NET.Models.Enums;

namespace sevDesk.NET;

/// <summary>
/// Optional server-side filters for <see cref="Clients.IInvoiceClient.ListAsync"/>.
/// </summary>
public class InvoiceListFilter
{
    /// <summary>
    /// Gets or sets a server-side delta filter on the invoice's <c>update</c> timestamp.
    /// Only invoices updated after this point in time are returned. Sent to the API as Unix seconds.
    /// </summary>
    public DateTimeOffset? UpdateAfter { get; set; }

    /// <summary>
    /// Gets or sets the invoice status to filter by.
    /// </summary>
    public InvoiceStatus? Status { get; set; }

    /// <summary>
    /// Gets or sets the contact ID to filter by.
    /// </summary>
    public int? ContactId { get; set; }

    /// <summary>
    /// Gets or sets the inclusive lower bound for the invoice's <c>invoiceDate</c>.
    /// Sent to the API as <c>startDate</c> in Unix seconds.
    /// </summary>
    public DateTimeOffset? InvoiceDateFrom { get; set; }

    /// <summary>
    /// Gets or sets the inclusive upper bound for the invoice's <c>invoiceDate</c>.
    /// Sent to the API as <c>endDate</c> in Unix seconds.
    /// </summary>
    public DateTimeOffset? InvoiceDateTo { get; set; }
}
