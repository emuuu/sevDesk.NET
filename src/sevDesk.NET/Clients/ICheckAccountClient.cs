using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

/// <summary>
/// Client for managing check accounts (bank accounts) in sevDesk.
/// Provides operations for creating, reading, updating, and deleting check accounts,
/// as well as retrieving account balances.
/// </summary>
public interface ICheckAccountClient
{
    /// <summary>
    /// Retrieves a paginated list of check accounts.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of check accounts.</returns>
    Task<SevDeskListResponse<CheckAccount>> ListAsync(PaginationParameters? pagination = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single check account by its identifier.
    /// </summary>
    /// <param name="id">The check account identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested check account.</returns>
    Task<CheckAccount> GetAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Creates a new check account.
    /// </summary>
    /// <param name="account">The check account to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created check account.</returns>
    Task<CheckAccount> CreateAsync(CheckAccount account, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing check account.
    /// </summary>
    /// <param name="id">The identifier of the check account to update.</param>
    /// <param name="account">The updated check account data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated check account.</returns>
    Task<CheckAccount> UpdateAsync(int id, CheckAccount account, CancellationToken ct = default);

    /// <summary>
    /// Deletes a check account by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the check account to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    Task DeleteAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Retrieves the balance of a check account, optionally at a specific date.
    /// </summary>
    /// <param name="id">The identifier of the check account.</param>
    /// <param name="date">Optional date to retrieve the balance for. If null, returns the current balance.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The account balance as a decimal value.</returns>
    Task<decimal> GetBalanceAsync(int id, DateTime? date = null, CancellationToken ct = default);
}
