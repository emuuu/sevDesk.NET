using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

/// <summary>
/// Client for managing vouchers in sevDesk.
/// Provides operations for creating, reading, updating, and deleting vouchers,
/// as well as saving with positions, booking amounts, and uploading files.
/// </summary>
public interface IVoucherClient
{
    /// <summary>
    /// Retrieves a paginated list of vouchers.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="embed">Optional comma-separated list of related objects to embed in the response.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of vouchers.</returns>
    Task<SevDeskListResponse<Voucher>> ListAsync(PaginationParameters? pagination = null, string? embed = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single voucher by its identifier.
    /// </summary>
    /// <param name="id">The voucher identifier.</param>
    /// <param name="embed">Optional comma-separated list of related objects to embed in the response.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested voucher.</returns>
    Task<Voucher> GetAsync(int id, string? embed = null, CancellationToken ct = default);

    /// <summary>
    /// Creates a new voucher.
    /// </summary>
    /// <param name="voucher">The voucher to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created voucher.</returns>
    Task<Voucher> CreateAsync(Voucher voucher, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing voucher.
    /// </summary>
    /// <param name="id">The identifier of the voucher to update.</param>
    /// <param name="voucher">The updated voucher data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated voucher.</returns>
    Task<Voucher> UpdateAsync(int id, Voucher voucher, CancellationToken ct = default);

    /// <summary>
    /// Deletes a voucher by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the voucher to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    Task DeleteAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Saves a voucher together with its positions in a single transaction, optionally attaching a
    /// file, and reads the saved voucher back.
    /// </summary>
    /// <remarks>
    /// The read-back is a second request. If it fails, the voucher has already been created and
    /// <see cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException"/> reports that, so the call
    /// must not be repeated. Use
    /// <see cref="SaveVoucherReferenceAsync(Voucher, IEnumerable{VoucherPos}, string, CancellationToken)"/>
    /// to skip the read-back entirely.
    /// </remarks>
    /// <param name="voucher">The voucher to save.</param>
    /// <param name="positions">The line item positions for the voucher.</param>
    /// <param name="filename">Optional filename of an attached document.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The saved voucher.</returns>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException">
    /// The voucher was created, but reading it back failed. Do not save it again.
    /// </exception>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskApiException">
    /// The voucher was not created. Retrying is safe.
    /// </exception>
    Task<Voucher> SaveVoucherAsync(Voucher voucher, IEnumerable<VoucherPos> positions, string? filename = null, CancellationToken ct = default);

    /// <summary>
    /// Saves a voucher together with its positions in a single transaction, optionally attaching a
    /// file, and returns only the reference to it, without reading the voucher back.
    /// </summary>
    /// <remarks>
    /// One request instead of two. Use this when the identifier of the new voucher is all that is
    /// needed; it removes the read-back and with it the ambiguous failure window that
    /// <see cref="SaveVoucherAsync(Voucher, IEnumerable{VoucherPos}, string, CancellationToken)"/>
    /// has to report through
    /// <see cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException"/>.
    /// </remarks>
    /// <param name="voucher">The voucher to save.</param>
    /// <param name="positions">The line item positions for the voucher.</param>
    /// <param name="filename">Optional filename of an attached document.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A reference carrying the identifier of the saved voucher.</returns>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskWriteSucceededException">
    /// The voucher was created, but its identifier could not be read from the response. Do not save
    /// it again; look it up instead.
    /// </exception>
    /// <exception cref="sevDesk.NET.Exceptions.SevDeskApiException">
    /// The voucher was not created. Retrying is safe.
    /// </exception>
    Task<SevDeskObjectReference> SaveVoucherReferenceAsync(Voucher voucher, IEnumerable<VoucherPos> positions, string? filename = null, CancellationToken ct = default);

    /// <summary>
    /// Books a payment amount against a voucher.
    /// </summary>
    /// <param name="id">The identifier of the voucher to book against.</param>
    /// <param name="amount">The payment amount to book.</param>
    /// <param name="checkAccountId">The identifier of the check account used for payment.</param>
    /// <param name="date">The date of the payment.</param>
    /// <param name="ct">Cancellation token.</param>
    Task BookAmountAsync(int id, decimal amount, int checkAccountId, DateTime date, CancellationToken ct = default);

    /// <summary>
    /// Marks a voucher as paid.
    /// </summary>
    /// <param name="id">The identifier of the voucher to mark as paid.</param>
    /// <param name="ct">Cancellation token.</param>
    Task MarkAsPaidAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Marks a voucher as open.
    /// </summary>
    /// <param name="id">The identifier of the voucher to mark as open.</param>
    /// <param name="ct">Cancellation token.</param>
    Task MarkAsOpenAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Uploads a file and creates a document associated with a voucher.
    /// </summary>
    /// <param name="stream">The file content stream.</param>
    /// <param name="fileName">The name of the file being uploaded.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created document.</returns>
    Task<Document> UploadFileAsync(Stream stream, string fileName, CancellationToken ct = default);
}
