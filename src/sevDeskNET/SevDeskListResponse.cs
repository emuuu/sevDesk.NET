namespace sevDeskNET;

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
    /// Gets the total number of items available (across all pages).
    /// </summary>
    public int Total { get; init; }
}
