using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

internal class CurrencyExchangeRateClient : ICurrencyExchangeRateClient
{
    private readonly BaseClient _client;

    internal CurrencyExchangeRateClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<CurrencyExchangeRate>> ListAsync(PaginationParameters? pagination = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("CurrencyExchangeRate", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiCurrencyExchangeRate, ct: ct).ConfigureAwait(false);
        return new SevDeskListResponse<CurrencyExchangeRate> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<CurrencyExchangeRate> GetAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"CurrencyExchangeRate/{id}", SevDeskJsonContext.Default.ApiCurrencyExchangeRate, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }
}
