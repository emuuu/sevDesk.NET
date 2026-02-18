using sevDeskNET.Internal;
using sevDeskNET.Internal.ApiModels;
using sevDeskNET.Models;

namespace sevDeskNET.Clients;

internal class CheckAccountTransactionClient : ICheckAccountTransactionClient
{
    private readonly BaseClient _client;

    internal CheckAccountTransactionClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<CheckAccountTransaction>> ListAsync(PaginationParameters? pagination = null, int? checkAccountId = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("CheckAccountTransaction", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiCheckAccountTransaction,
            qb => { if (checkAccountId.HasValue) qb.Add("checkAccount[id]", checkAccountId.Value.ToString()).Add("checkAccount[objectName]", "CheckAccount"); }, ct).ConfigureAwait(false);
        return new SevDeskListResponse<CheckAccountTransaction> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<CheckAccountTransaction> GetAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"CheckAccountTransaction/{id}", SevDeskJsonContext.Default.SevDeskApiResponseApiCheckAccountTransaction, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<CheckAccountTransaction> CreateAsync(CheckAccountTransaction transaction, CancellationToken ct = default)
    {
        var api = await _client.PostAsync("CheckAccountTransaction", ModelMapper.ToApi(transaction),
            SevDeskJsonContext.Default.ApiCheckAccountTransaction, SevDeskJsonContext.Default.SevDeskApiResponseApiCheckAccountTransaction, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<CheckAccountTransaction> UpdateAsync(int id, CheckAccountTransaction transaction, CancellationToken ct = default)
    {
        var api = await _client.PutAsync($"CheckAccountTransaction/{id}", ModelMapper.ToApi(transaction),
            SevDeskJsonContext.Default.ApiCheckAccountTransaction, SevDeskJsonContext.Default.SevDeskApiResponseApiCheckAccountTransaction, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) =>
        _client.DeleteAsync($"CheckAccountTransaction/{id}", ct);
}
