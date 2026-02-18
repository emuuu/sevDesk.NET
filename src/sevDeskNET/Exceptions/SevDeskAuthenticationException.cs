using System.Net;

namespace sevDeskNET.Exceptions;

/// <summary>
/// Exception thrown when the sevDesk API returns a 401 Unauthorized response.
/// </summary>
public class SevDeskAuthenticationException : SevDeskApiException
{
    /// <summary>
    /// Initializes a new instance of <see cref="SevDeskAuthenticationException"/>.
    /// </summary>
    public SevDeskAuthenticationException(string message, string? rawResponse = null)
        : base(message, HttpStatusCode.Unauthorized, rawResponse) { }
}
