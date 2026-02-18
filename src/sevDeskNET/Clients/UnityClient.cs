using sevDeskNET.Internal;
using sevDeskNET.Internal.ApiModels;
using sevDeskNET.Models;

namespace sevDeskNET.Clients;

internal class UnityClient : IUnityClient
{
    private readonly BaseClient _client;

    internal UnityClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<Unity>> ListAsync(PaginationParameters? pagination = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("Unity", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiUnity, ct: ct).ConfigureAwait(false);
        return new SevDeskListResponse<Unity> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<Unity> GetAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"Unity/{id}", SevDeskJsonContext.Default.SevDeskApiResponseApiUnity, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }
}
