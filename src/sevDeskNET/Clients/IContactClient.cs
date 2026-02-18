using sevDeskNET.Models;

namespace sevDeskNET.Clients;

/// <summary>
/// Client for managing contacts in sevDesk.
/// Provides operations for creating, reading, updating, and deleting contacts,
/// as well as retrieving the next available customer number.
/// </summary>
public interface IContactClient
{
    /// <summary>
    /// Retrieves a paginated list of contacts.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="embed">Optional comma-separated list of related objects to embed in the response.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of contacts.</returns>
    Task<SevDeskListResponse<Contact>> ListAsync(PaginationParameters? pagination = null, string? embed = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single contact by its identifier.
    /// </summary>
    /// <param name="id">The contact identifier.</param>
    /// <param name="embed">Optional comma-separated list of related objects to embed in the response.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested contact.</returns>
    Task<Contact> GetAsync(int id, string? embed = null, CancellationToken ct = default);

    /// <summary>
    /// Creates a new contact.
    /// </summary>
    /// <param name="contact">The contact to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created contact.</returns>
    Task<Contact> CreateAsync(Contact contact, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing contact.
    /// </summary>
    /// <param name="id">The identifier of the contact to update.</param>
    /// <param name="contact">The updated contact data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated contact.</returns>
    Task<Contact> UpdateAsync(int id, Contact contact, CancellationToken ct = default);

    /// <summary>
    /// Deletes a contact by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the contact to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    Task DeleteAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Retrieves the next available customer number.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The next available customer number as a string.</returns>
    Task<string> GetNextCustomerNumberAsync(CancellationToken ct = default);
}
