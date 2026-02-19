namespace sevDesk.NET;

/// <summary>
/// Parameters for paginating sevDesk API list requests.
/// </summary>
public class PaginationParameters
{
    /// <summary>
    /// Gets or sets the maximum number of items to return (1-1000). Defaults to 100.
    /// </summary>
    public int Limit { get; set; } = 100;

    /// <summary>
    /// Gets or sets the number of items to skip.
    /// </summary>
    public int Offset { get; set; }
}
