namespace sevDesk.NET.Docs.Services;

public sealed class DocsApiTokenService
{
    private string _apiToken = "";

    public string ApiToken => _apiToken;
    public bool HasToken => !string.IsNullOrWhiteSpace(_apiToken);

    public event Action? ApiTokenChanged;

    public void UpdateApiToken(string token)
    {
        _apiToken = token?.Trim() ?? "";
        ApiTokenChanged?.Invoke();
    }
}
