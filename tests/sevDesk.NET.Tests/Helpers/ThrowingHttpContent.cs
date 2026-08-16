using System.Net;

namespace sevDesk.NET.Tests.Helpers;

/// <summary>
/// A response body that fails while being read. Reproduces a connection that drops after the API
/// has already answered with a success status — the write happened, its answer never arrived.
/// </summary>
internal sealed class ThrowingHttpContent : HttpContent
{
    private readonly Exception _exception;

    public ThrowingHttpContent(Exception exception) => _exception = exception;

    protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        => Task.FromException(_exception);

    protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context, CancellationToken cancellationToken)
        => Task.FromException(_exception);

    protected override bool TryComputeLength(out long length)
    {
        length = 0;
        return false;
    }
}
