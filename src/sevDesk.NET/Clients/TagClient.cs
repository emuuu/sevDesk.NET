using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

internal class TagClient : ITagClient
{
    private readonly BaseClient _client;

    internal TagClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<Tag>> ListAsync(PaginationParameters? pagination = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("Tag", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiTag, ct: ct).ConfigureAwait(false);
        return new SevDeskListResponse<Tag> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<Tag> GetAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"Tag/{id}", SevDeskJsonContext.Default.ApiTag, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<Tag> CreateAsync(Tag tag, CancellationToken ct = default)
    {
        var api = await _client.PostAsync("Tag", ModelMapper.ToApi(tag),
            SevDeskJsonContext.Default.ApiTag, SevDeskJsonContext.Default.SevDeskApiResponseApiTag, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) =>
        _client.DeleteAsync($"Tag/{id}", ct);
}
