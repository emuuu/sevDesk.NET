using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

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
        var api = await _client.GetAsync($"TaxRule/{id}", SevDeskJsonContext.Default.ApiTaxRule, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }
}
