using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

/// <summary>
/// Client for managing parts (products/articles) in sevDesk.
/// Provides operations for creating, reading, updating, and deleting parts.
/// </summary>
public interface IPartClient
{
    /// <summary>
    /// Retrieves a paginated list of parts.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of parts.</returns>
    Task<SevDeskListResponse<Part>> ListAsync(PaginationParameters? pagination = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single part by its identifier.
    /// </summary>
    /// <param name="id">The part identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested part.</returns>
    Task<Part> GetAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Creates a new part.
    /// </summary>
    /// <param name="part">The part to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created part.</returns>
    Task<Part> CreateAsync(Part part, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing part.
    /// </summary>
    /// <param name="id">The identifier of the part to update.</param>
    /// <param name="part">The updated part data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated part.</returns>
    Task<Part> UpdateAsync(int id, Part part, CancellationToken ct = default);

    /// <summary>
    /// Deletes a part by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the part to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    Task DeleteAsync(int id, CancellationToken ct = default);
}
