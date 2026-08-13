using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

/// <summary>
/// Client for retrieving accounting contacts in sevDesk.
/// Provides read-only access to the debitor and creditor numbers assigned to contacts.
/// </summary>
public interface IAccountingContactClient
{
    /// <summary>
    /// Retrieves a paginated list of accounting contacts.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="contactId">Optional contact identifier to only return that contact's accounting contact.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of accounting contacts.</returns>
    Task<SevDeskListResponse<AccountingContact>> ListAsync(PaginationParameters? pagination = null, int? contactId = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single accounting contact by its identifier.
    /// </summary>
    /// <param name="id">The accounting contact identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested accounting contact.</returns>
    Task<AccountingContact> GetAsync(int id, CancellationToken ct = default);
}
