namespace sevDesk.NET;

/// <summary>
/// Represents a paginated list response from the sevDesk API.
/// </summary>
/// <typeparam name="T">The type of items in the response.</typeparam>
public class SevDeskListResponse<T>
{
    /// <summary>
    /// Gets the list of items returned by the API.
    /// </summary>
    public required IReadOnlyList<T> Items { get; init; }

    /// <summary>
    /// Gets the total number of items available across all pages, or <see langword="null"/> when
    /// the API did not report one.
    /// </summary>
    /// <remarks>
    /// The sevDesk API sends <c>total</c> only for <c>countAll=true</c> — which every
    /// <c>ListAsync</c> requests — and not reliably on every page even then. The two cases are
    /// therefore distinct: <see langword="null"/> means the server reported no total and the size
    /// of the result set is unknown, while <c>0</c> means the server reported an empty result set.
    /// Treating a missing total as <c>0</c> makes a full page look like the end of the data, so
    /// callers paginating to completion should fall back to "keep going while a page comes back
    /// full" rather than assume a count.
    /// </remarks>
    public int? Total { get; init; }
}
