using System.Net;

namespace sevDesk.NET.Exceptions;

/// <summary>
/// Exception thrown when the sevDesk API returns a validation error (HTTP 422).
/// </summary>
public class SevDeskValidationException : SevDeskApiException
{
    /// <summary>
    /// Initializes a new instance of <see cref="SevDeskValidationException"/>.
    /// </summary>
    public SevDeskValidationException(string message, string? rawResponse = null)
        : base(message, HttpStatusCode.UnprocessableEntity, rawResponse) { }
}
