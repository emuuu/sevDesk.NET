using Microsoft.Extensions.Options;

namespace sevDeskNET;

/// <summary>
/// HTTP message handler that adds the sevDesk API token to every request.
/// </summary>
public class SevDeskAuthHandler : DelegatingHandler
{
    private readonly SevDeskOptions _options;

    /// <summary>
    /// Initializes a new instance of <see cref="SevDeskAuthHandler"/>.
    /// </summary>
    /// <param name="options">sevDesk configuration options.</param>
    public SevDeskAuthHandler(IOptions<SevDeskOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _options = options.Value;
    }

    /// <inheritdoc />
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.TryAddWithoutValidation("Authorization", _options.ApiToken);
        return base.SendAsync(request, cancellationToken);
    }
}
