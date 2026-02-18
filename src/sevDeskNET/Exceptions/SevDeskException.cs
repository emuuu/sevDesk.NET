using System.Net;

namespace sevDeskNET.Exceptions;

/// <summary>
/// Base exception for all sevDeskNET operations.
/// </summary>
public class SevDeskException : Exception
{
    /// <summary>
    /// Gets the HTTP status code returned by the sevDesk API, if available.
    /// </summary>
    public HttpStatusCode? StatusCode { get; }

    /// <summary>
    /// Gets the raw response body from the sevDesk API, if available.
    /// </summary>
    public string? RawResponse { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="SevDeskException"/>.
    /// </summary>
    public SevDeskException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of <see cref="SevDeskException"/> with an inner exception.
    /// </summary>
    public SevDeskException(string message, Exception innerException)
        : base(message, innerException) { }

    /// <summary>
    /// Initializes a new instance of <see cref="SevDeskException"/> with API error details.
    /// </summary>
    public SevDeskException(
        string message,
        HttpStatusCode? statusCode,
        string? rawResponse,
        Exception? innerException = null)
        : base(message, innerException!)
    {
        StatusCode = statusCode;
        RawResponse = rawResponse;
    }
}
