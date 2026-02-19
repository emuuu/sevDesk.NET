using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

/// <summary>
/// Client for managing tags in sevDesk.
/// Provides operations for creating, reading, and deleting tags.
/// </summary>
public interface ITagClient
{
    /// <summary>
    /// Retrieves a paginated list of tags.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of tags.</returns>
    Task<SevDeskListResponse<Tag>> ListAsync(PaginationParameters? pagination = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single tag by its identifier.
    /// </summary>
    /// <param name="id">The tag identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested tag.</returns>
    Task<Tag> GetAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Creates a new tag.
    /// </summary>
    /// <param name="tag">The tag to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created tag.</returns>
    Task<Tag> CreateAsync(Tag tag, CancellationToken ct = default);

    /// <summary>
    /// Deletes a tag by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the tag to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    Task DeleteAsync(int id, CancellationToken ct = default);
}
