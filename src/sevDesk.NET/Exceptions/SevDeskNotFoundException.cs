using System.Net;

namespace sevDesk.NET.Exceptions;

/// <summary>
/// Exception thrown when the sevDesk API returns a 404 Not Found response.
/// </summary>
public class SevDeskNotFoundException : SevDeskApiException
{
    /// <summary>
    /// Initializes a new instance of <see cref="SevDeskNotFoundException"/>.
    /// </summary>
    public SevDeskNotFoundException(string message, string? rawResponse = null)
        : base(message, HttpStatusCode.NotFound, rawResponse) { }
}
