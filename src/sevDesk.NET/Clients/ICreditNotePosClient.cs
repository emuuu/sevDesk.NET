using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

/// <summary>
/// Client for managing credit note positions (line items) in sevDesk.
/// Provides operations for creating, reading, updating, and deleting credit note positions.
/// </summary>
public interface ICreditNotePosClient
{
    /// <summary>
    /// Retrieves a paginated list of credit note positions, optionally filtered by credit note.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="creditNoteId">Optional credit note identifier to filter positions by.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of credit note positions.</returns>
    Task<SevDeskListResponse<CreditNotePos>> ListAsync(PaginationParameters? pagination = null, int? creditNoteId = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single credit note position by its identifier.
    /// </summary>
    /// <param name="id">The credit note position identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested credit note position.</returns>
    Task<CreditNotePos> GetAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Creates a new credit note position.
    /// </summary>
    /// <param name="position">The credit note position to create.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created credit note position.</returns>
    Task<CreditNotePos> CreateAsync(CreditNotePos position, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing credit note position.
    /// </summary>
    /// <param name="id">The identifier of the credit note position to update.</param>
    /// <param name="position">The updated credit note position data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated credit note position.</returns>
    Task<CreditNotePos> UpdateAsync(int id, CreditNotePos position, CancellationToken ct = default);

    /// <summary>
    /// Deletes a credit note position by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the credit note position to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    Task DeleteAsync(int id, CancellationToken ct = default);
}
