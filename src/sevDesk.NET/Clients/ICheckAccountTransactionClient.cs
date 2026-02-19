using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

/// <summary>
/// Client for managing check account transactions in sevDesk.
/// Provides operations for creating, reading, updating, and deleting transactions
/// associated with check accounts.
/// </summary>
public interface ICheckAccountTransactionClient
{
    /// <summary>
    /// Retrieves a paginated list of check account transactions, optionally filtered by check account.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="checkAccountId">Optional check account identifier to filter transactions by.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of check account transactions.</returns>
    Task<SevDeskListResponse<CheckAccountTransaction>> ListAsync(PaginationParameters? pagination = null, int? checkAccountId = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single check account transaction by its identifier.
    /// </summary>
    /// <param name="id">The transaction identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested check account transaction.</returns>
    Task<CheckAccountTransaction> GetAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Creates a new check account transaction.
    /// </summary>
    /// <param name="transaction">The transaction to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created check account transaction.</returns>
    Task<CheckAccountTransaction> CreateAsync(CheckAccountTransaction transaction, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing check account transaction.
    /// </summary>
    /// <param name="id">The identifier of the transaction to update.</param>
    /// <param name="transaction">The updated transaction data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated check account transaction.</returns>
    Task<CheckAccountTransaction> UpdateAsync(int id, CheckAccountTransaction transaction, CancellationToken ct = default);

    /// <summary>
    /// Deletes a check account transaction by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the transaction to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    Task DeleteAsync(int id, CancellationToken ct = default);
}
