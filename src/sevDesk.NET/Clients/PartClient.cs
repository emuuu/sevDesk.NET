using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

internal class PartClient : IPartClient
{
    private readonly BaseClient _client;

    internal PartClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<Part>> ListAsync(PaginationParameters? pagination = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("Part", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiPart, ct: ct).ConfigureAwait(false);
        return new SevDeskListResponse<Part> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<Part> GetAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"Part/{id}", SevDeskJsonContext.Default.ApiPart, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<Part> CreateAsync(Part part, CancellationToken ct = default)
    {
        var api = await _client.PostAsync("Part", ModelMapper.ToApi(part),
            SevDeskJsonContext.Default.ApiPart, SevDeskJsonContext.Default.SevDeskApiResponseApiPart, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<Part> UpdateAsync(int id, Part part, CancellationToken ct = default)
    {
        var api = await _client.PutAsync($"Part/{id}", ModelMapper.ToApi(part),
            SevDeskJsonContext.Default.ApiPart, SevDeskJsonContext.Default.SevDeskApiResponseApiPart, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) =>
        _client.DeleteAsync($"Part/{id}", ct);
}
