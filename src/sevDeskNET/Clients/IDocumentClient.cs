using sevDeskNET.Models;

namespace sevDeskNET.Clients;

/// <summary>
/// Client for managing documents in sevDesk.
/// Provides operations for listing, retrieving, uploading, and downloading documents.
/// </summary>
public interface IDocumentClient
{
    /// <summary>
    /// Retrieves a paginated list of documents.
    /// </summary>
    /// <param name="pagination">Optional pagination parameters to control the result set.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paginated list of documents.</returns>
    Task<SevDeskListResponse<Document>> ListAsync(PaginationParameters? pagination = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a single document by its identifier.
    /// </summary>
    /// <param name="id">The document identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested document.</returns>
    Task<Document> GetAsync(int id, CancellationToken ct = default);

    /// <summary>
    /// Uploads a new document from a stream.
    /// </summary>
    /// <param name="stream">The file content stream to upload.</param>
    /// <param name="fileName">The name of the file being uploaded.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created document.</returns>
    Task<Document> UploadAsync(Stream stream, string fileName, CancellationToken ct = default);

    /// <summary>
    /// Downloads the content of a document.
    /// </summary>
    /// <param name="id">The identifier of the document to download.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The document file content as a byte array.</returns>
    Task<byte[]> DownloadAsync(int id, CancellationToken ct = default);
}
