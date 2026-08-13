using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

internal class DocumentClient : IDocumentClient
{
    private readonly BaseClient _client;

    internal DocumentClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<Document>> ListAsync(PaginationParameters? pagination = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("Document", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiDocument, ct: ct).ConfigureAwait(false);
        return new SevDeskListResponse<Document> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<Document> GetAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"Document/{id}", SevDeskJsonContext.Default.ApiDocument, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<Document> UploadAsync(Stream stream, string fileName, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent();
        using var streamContent = new StreamContent(stream);
        content.Add(streamContent, "file", fileName);
        var api = await _client.PostMultipartAsync("Document/Factory/upload", content,
            SevDeskJsonContext.Default.SevDeskApiResponseApiDocument, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task<byte[]> DownloadAsync(int id, CancellationToken ct = default) =>
        _client.GetBytesAsync($"Document/{id}/download", ct);
}
