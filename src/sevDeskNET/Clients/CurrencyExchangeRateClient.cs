using sevDeskNET.Internal;
using sevDeskNET.Internal.ApiModels;
using sevDeskNET.Models;

namespace sevDeskNET.Clients;

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
        var api = await _client.GetAsync($"CurrencyExchangeRate/{id}", SevDeskJsonContext.Default.SevDeskApiResponseApiCurrencyExchangeRate, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }
}
