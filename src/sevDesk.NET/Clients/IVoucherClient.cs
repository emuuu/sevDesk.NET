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
    /// Saves a voucher together with its positions in a single transaction, optionally attaching a file.
    /// </summary>
    /// <param name="voucher">The voucher to save.</param>
    /// <param name="positions">The line item positions for the voucher.</param>
    /// <param name="filename">Optional filename of an attached document.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The saved voucher.</returns>
    Task<Voucher> SaveVoucherAsync(Voucher voucher, IEnumerable<VoucherPos> positions, string? filename = null, CancellationToken ct = default);

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
