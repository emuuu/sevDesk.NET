namespace sevDesk.NET.Docs.Services;

/// <summary>
/// DelegatingHandler that injects the sevDesk API token from the mutable DocsApiTokenService.
/// Mirrors SevDeskAuthHandler but reads from the docs token service instead of IOptions.
/// </summary>
public class DocsAuthHandler : DelegatingHandler
{
    private readonly DocsApiTokenService _tokenService;

    public DocsAuthHandler(DocsApiTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (_tokenService.HasToken)
        {
            request.Headers.TryAddWithoutValidation("Authorization", _tokenService.ApiToken);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
