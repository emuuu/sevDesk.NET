using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

/// <summary>
/// Client for managing communication ways (phone, email, website, etc.) in sevDesk.
/// Provides operations for creating, reading, updating, and deleting communication ways
/// associated with contacts.
/// </summary>
public interface ICommunicationWayClient
{
    /// <summary>
    /// Retrieves a paginated list of communication ways, optionally filtered by contact.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="contactId">Optional contact identifier to filter communication ways by.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of communication ways.</returns>
    Task<SevDeskListResponse<CommunicationWay>> ListAsync(PaginationParameters? pagination = null, int? contactId = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single communication way by its identifier.
    /// </summary>
    /// <param name="id">The communication way identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested communication way.</returns>
    Task<CommunicationWay> GetAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Creates a new communication way.
    /// </summary>
    /// <param name="communicationWay">The communication way to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created communication way.</returns>
    Task<CommunicationWay> CreateAsync(CommunicationWay communicationWay, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing communication way.
    /// </summary>
    /// <param name="id">The identifier of the communication way to update.</param>
    /// <param name="communicationWay">The updated communication way data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated communication way.</returns>
    Task<CommunicationWay> UpdateAsync(int id, CommunicationWay communicationWay, CancellationToken ct = default);

    /// <summary>
    /// Deletes a communication way by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the communication way to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    Task DeleteAsync(int id, CancellationToken ct = default);
}
