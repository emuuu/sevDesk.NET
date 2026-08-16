using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

/// <summary>
/// Client for managing credit notes in sevDesk.
/// Provides operations for creating, reading, updating, and deleting credit notes,
/// as well as creating from invoices, generating PDFs, and sending via email.
/// </summary>
public interface ICreditNoteClient
{
    /// <summary>
    /// Retrieves a paginated list of credit notes.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="embed">Optional comma-separated list of related objects to embed in the response.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of credit notes.</returns>
    Task<SevDeskListResponse<CreditNote>> ListAsync(PaginationParameters? pagination = null, string? embed = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single credit note by its identifier.
    /// </summary>
    /// <param name="id">The credit note identifier.</param>
    /// <param name="embed">Optional comma-separated list of related objects to embed in the response.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested credit note.</returns>
    Task<CreditNote> GetAsync(int id, string? embed = null, CancellationToken ct = default);

    /// <summary>
    /// Creates a new credit note.
    /// </summary>
    /// <param name="creditNote">The credit note to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created credit note.</returns>
    Task<CreditNote> CreateAsync(CreditNote creditNote, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing credit note.
    /// </summary>
    /// <param name="id">The identifier of the credit note to update.</param>
    /// <param name="creditNote">The updated credit note data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated credit note.</returns>
    Task<CreditNote> UpdateAsync(int id, CreditNote creditNote, CancellationToken ct = default);

    /// <summary>
    /// Deletes a credit note by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the credit note to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    Task DeleteAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Saves a credit note together with its positions in a single transaction and reads the saved
    /// credit note back.
    /// </summary>
    /// <remarks>
    /// The read-back is a second request. If it fails, the credit note has already been created and
    /// <see cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException"/> reports that, so the call
    /// must not be repeated. Use
    /// <see cref="SaveCreditNoteReferenceAsync(CreditNote, IEnumerable{CreditNotePos}, CancellationToken)"/>
    /// to skip the read-back entirely.
    /// </remarks>
    /// <param name="creditNote">The credit note to save.</param>
    /// <param name="positions">The line item positions for the credit note.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The saved credit note.</returns>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException">
    /// The credit note was created, but reading it back failed. Do not save it again.
    /// </exception>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskApiException">
    /// The credit note was not created. Retrying is safe.
    /// </exception>
    Task<CreditNote> SaveCreditNoteAsync(CreditNote creditNote, IEnumerable<CreditNotePos> positions, CancellationToken ct = default);

    /// <summary>
    /// Saves a credit note together with its positions in a single transaction and returns only the
    /// reference to it, without reading the credit note back.
    /// </summary>
    /// <remarks>
    /// One request instead of two. Use this when the identifier of the new credit note is all that
    /// is needed; it removes the read-back and with it the ambiguous failure window that
    /// <see cref="SaveCreditNoteAsync(CreditNote, IEnumerable{CreditNotePos}, CancellationToken)"/>
    /// has to report through
    /// <see cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException"/>.
    /// </remarks>
    /// <param name="creditNote">The credit note to save.</param>
    /// <param name="positions">The line item positions for the credit note.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A reference carrying the identifier of the saved credit note.</returns>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException">
    /// The credit note was created, but its identifier could not be read from the response. Do not
    /// save it again; look it up instead.
    /// </exception>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskApiException">
    /// The credit note was not created. Retrying is safe.
    /// </exception>
    Task<SevDeskObjectReference> SaveCreditNoteReferenceAsync(CreditNote creditNote, IEnumerable<CreditNotePos> positions, CancellationToken ct = default);

    /// <summary>
    /// Creates a credit note from an existing invoice and reads the created credit note back.
    /// </summary>
    /// <remarks>
    /// The read-back is a second request. If it fails, the credit note has already been created and
    /// <see cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException"/> reports that, so the call
    /// must not be repeated. Use <see cref="CreateFromInvoiceReferenceAsync(int, CancellationToken)"/>
    /// to skip the read-back entirely.
    /// </remarks>
    /// <param name="invoiceId">The identifier of the invoice to create a credit note from.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created credit note.</returns>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException">
    /// The credit note was created, but reading it back failed. Do not create it again.
    /// </exception>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskApiException">
    /// The credit note was not created. Retrying is safe.
    /// </exception>
    Task<CreditNote> CreateFromInvoiceAsync(int invoiceId, CancellationToken ct = default);

    /// <summary>
    /// Creates a credit note from an existing invoice and returns only the reference to it, without
    /// reading the credit note back.
    /// </summary>
    /// <remarks>
    /// One request instead of two. Use this when the identifier of the new credit note is all that
    /// is needed; it removes the read-back and with it the ambiguous failure window that
    /// <see cref="CreateFromInvoiceAsync(int, CancellationToken)"/> has to report through
    /// <see cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException"/>.
    /// </remarks>
    /// <param name="invoiceId">The identifier of the invoice to create a credit note from.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A reference carrying the identifier of the created credit note.</returns>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException">
    /// The credit note was created, but its identifier could not be read from the response. Do not
    /// create it again; look it up instead.
    /// </exception>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskApiException">
    /// The credit note was not created. Retrying is safe.
    /// </exception>
    Task<SevDeskObjectReference> CreateFromInvoiceReferenceAsync(int invoiceId, CancellationToken ct = default);

    /// <summary>
    /// Downloads the PDF representation of a credit note.
    /// </summary>
    /// <param name="id">The identifier of the credit note.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The PDF file content as a byte array.</returns>
    Task<byte[]> GetPdfAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Sends a credit note via email.
    /// </summary>
    /// <param name="id">The identifier of the credit note to send.</param>
    /// <param name="email">The recipient email address.</param>
    /// <param name="subject">The email subject line.</param>
    /// <param name="text">The email body text.</param>
    /// <param name="ct">Cancellation token.</param>
    Task SendViaEmailAsync(int id, string email, string subject, string text, CancellationToken ct = default);
}
