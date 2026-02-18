using sevDeskNET.Internal;
using sevDeskNET.Internal.ApiModels;
using sevDeskNET.Models;

namespace sevDeskNET.Clients;

internal class TaxRuleClient : ITaxRuleClient
{
    private readonly BaseClient _client;

    internal TaxRuleClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<TaxRule>> ListAsync(PaginationParameters? pagination = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("TaxRule", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiTaxRule, ct: ct).ConfigureAwait(false);
        return new SevDeskListResponse<TaxRule> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<TaxRule> GetAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"TaxRule/{id}", SevDeskJsonContext.Default.SevDeskApiResponseApiTaxRule, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }
}
