namespace sevDesk.NET.Tests.Helpers;

/// <summary>
/// Answers requests from a queue of responses and records each request body as a string while
/// the request is still alive. Needed for factory writes, whose request message is disposed
/// before the call returns.
/// </summary>
internal class RecordingHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<HttpResponseMessage> _responses;

    public List<(HttpMethod Method, Uri? Uri, string? Body)> Requests { get; } = [];

    public RecordingHttpMessageHandler(params HttpResponseMessage[] responses)
        => _responses = new Queue<HttpResponseMessage>(responses);

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var body = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);
        Requests.Add((request.Method, request.RequestUri, body));

        return _responses.Count > 0
            ? _responses.Dequeue()
            : throw new InvalidOperationException($"Unexpected request #{Requests.Count} to {request.RequestUri}.");
    }
}
