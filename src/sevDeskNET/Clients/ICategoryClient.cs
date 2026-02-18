using sevDeskNET.Models;

namespace sevDeskNET.Clients;

/// <summary>
/// Client for managing categories in sevDesk.
/// Provides operations for creating, reading, updating, and deleting categories,
/// with optional filtering by object type.
/// </summary>
public interface ICategoryClient
{
    /// <summary>
    /// Retrieves a paginated list of categories, optionally filtered by object type.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="objectType">Optional object type to filter categories by (e.g., "Invoice", "Order").</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of categories.</returns>
    Task<SevDeskListResponse<Category>> ListAsync(PaginationParameters? pagination = null, string? objectType = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single category by its identifier.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested category.</returns>
    Task<Category> GetAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Creates a new category.
    /// </summary>
    /// <param name="category">The category to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created category.</returns>
    Task<Category> CreateAsync(Category category, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="id">The identifier of the category to update.</param>
    /// <param name="category">The updated category data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated category.</returns>
    Task<Category> UpdateAsync(int id, Category category, CancellationToken ct = default);

    /// <summary>
    /// Deletes a category by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the category to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    Task DeleteAsync(int id, CancellationToken ct = default);
}
