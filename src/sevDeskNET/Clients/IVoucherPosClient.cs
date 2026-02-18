using sevDeskNET.Models;

namespace sevDeskNET.Clients;

/// <summary>
/// Client for managing voucher positions (line items) in sevDesk.
/// Provides operations for creating, reading, updating, and deleting voucher positions.
/// </summary>
public interface IVoucherPosClient
{
    /// <summary>
    /// Retrieves a paginated list of voucher positions, optionally filtered by voucher.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="voucherId">Optional voucher identifier to filter positions by.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of voucher positions.</returns>
    Task<SevDeskListResponse<VoucherPos>> ListAsync(PaginationParameters? pagination = null, int? voucherId = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single voucher position by its identifier.
    /// </summary>
    /// <param name="id">The voucher position identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested voucher position.</returns>
    Task<VoucherPos> GetAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Creates a new voucher position.
    /// </summary>
    /// <param name="position">The voucher position to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created voucher position.</returns>
    Task<VoucherPos> CreateAsync(VoucherPos position, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing voucher position.
    /// </summary>
    /// <param name="id">The identifier of the voucher position to update.</param>
    /// <param name="position">The updated voucher position data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated voucher position.</returns>
    Task<VoucherPos> UpdateAsync(int id, VoucherPos position, CancellationToken ct = default);

    /// <summary>
    /// Deletes a voucher position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the voucher position to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    Task DeleteAsync(int id, CancellationToken ct = default);
}
