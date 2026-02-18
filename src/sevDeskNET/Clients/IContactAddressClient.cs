using sevDeskNET.Models;

namespace sevDeskNET.Clients;

/// <summary>
/// Client for managing contact addresses in sevDesk.
/// Provides operations for creating, reading, updating, and deleting addresses
/// associated with contacts.
/// </summary>
public interface IContactAddressClient
{
    /// <summary>
    /// Retrieves a paginated list of contact addresses, optionally filtered by contact.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="contactId">Optional contact identifier to filter addresses by.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of contact addresses.</returns>
    Task<SevDeskListResponse<ContactAddress>> ListAsync(PaginationParameters? pagination = null, int? contactId = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single contact address by its identifier.
    /// </summary>
    /// <param name="id">The contact address identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested contact address.</returns>
    Task<ContactAddress> GetAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Creates a new contact address.
    /// </summary>
    /// <param name="address">The contact address to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created contact address.</returns>
    Task<ContactAddress> CreateAsync(ContactAddress address, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing contact address.
    /// </summary>
    /// <param name="id">The identifier of the contact address to update.</param>
    /// <param name="address">The updated contact address data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated contact address.</returns>
    Task<ContactAddress> UpdateAsync(int id, ContactAddress address, CancellationToken ct = default);

    /// <summary>
    /// Deletes a contact address by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the contact address to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    Task DeleteAsync(int id, CancellationToken ct = default);
}
