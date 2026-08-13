using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

internal class StaticCountryClient : IStaticCountryClient
{
    private readonly BaseClient _client;

    internal StaticCountryClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<StaticCountry>> ListAsync(PaginationParameters? pagination = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("StaticCountry", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiStaticCountry, ct: ct).ConfigureAwait(false);
        return new SevDeskListResponse<StaticCountry> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<StaticCountry> GetAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"StaticCountry/{id}", SevDeskJsonContext.Default.ApiStaticCountry, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }
}
