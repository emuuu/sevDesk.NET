namespace sevDesk.NET;

/// <summary>
/// Configuration options for the sevDesk API client.
/// </summary>
public class SevDeskOptions
{
    /// <summary>
    /// Gets or sets the sevDesk API token (32-character hex string).
    /// </summary>
    public required string ApiToken { get; set; }

    /// <summary>
    /// Gets or sets an optional custom base URL for testing purposes.
    /// </summary>
    public string? CustomBaseUrl { get; set; }

    /// <summary>
    /// Gets the effective base URL for the sevDesk API.
    /// </summary>
    public string BaseUrl => CustomBaseUrl ?? "https://my.sevdesk.de/api/v1";
}
