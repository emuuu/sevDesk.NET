namespace sevDesk.NET.Tests.Helpers;

/// <summary>
/// Answers requests from a scripted queue. Each entry is either an <see cref="HttpResponseMessage"/>
/// to return or an <see cref="Exception"/> to throw, which lets a test reproduce a transport failure
/// on a specific request of a multi-request call.
/// </summary>
internal class ScriptedHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<object> _steps;

    public List<HttpRequestMessage> Requests { get; } = [];

    public ScriptedHttpMessageHandler(params object[] steps)
        => _steps = new Queue<object>(steps);

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Requests.Add(request);

        if (_steps.Count == 0)
        {
            throw new InvalidOperationException(
                $"Unexpected request #{Requests.Count} to {request.RequestUri}: the script is exhausted.");
        }

        return _steps.Dequeue() switch
        {
            HttpResponseMessage response => Task.FromResult(response),
            Exception exception => Task.FromException<HttpResponseMessage>(exception),
            var step => throw new InvalidOperationException($"Unsupported script step: {step.GetType()}.")
        };
    }
}
