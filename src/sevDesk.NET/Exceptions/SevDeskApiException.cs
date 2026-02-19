using System.Net;

namespace sevDesk.NET.Exceptions;

/// <summary>
/// Exception thrown when the sevDesk API returns an error response.
/// </summary>
public class SevDeskApiException : SevDeskException
{
    /// <summary>
    /// Initializes a new instance of <see cref="SevDeskApiException"/>.
    /// </summary>
    public SevDeskApiException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of <see cref="SevDeskApiException"/> with API error details.
    /// </summary>
    public SevDeskApiException(
        string message,
        HttpStatusCode? statusCode,
        string? rawResponse,
        Exception? innerException = null)
        : base(message, statusCode, rawResponse, innerException) { }
}
