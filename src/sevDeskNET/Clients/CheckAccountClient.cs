using System.Text.Json;
using sevDeskNET.Internal;
using sevDeskNET.Internal.ApiModels;
using sevDeskNET.Models;

namespace sevDeskNET.Clients;

internal class CheckAccountClient : ICheckAccountClient
{
    private readonly BaseClient _client;

    internal CheckAccountClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<CheckAccount>> ListAsync(PaginationParameters? pagination = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("CheckAccount", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiCheckAccount, ct: ct).ConfigureAwait(false);
        return new SevDeskListResponse<CheckAccount> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<CheckAccount> GetAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"CheckAccount/{id}", SevDeskJsonContext.Default.SevDeskApiResponseApiCheckAccount, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<CheckAccount> CreateAsync(CheckAccount account, CancellationToken ct = default)
    {
        var api = await _client.PostAsync("CheckAccount", ModelMapper.ToApi(account),
            SevDeskJsonContext.Default.ApiCheckAccount, SevDeskJsonContext.Default.SevDeskApiResponseApiCheckAccount, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<CheckAccount> UpdateAsync(int id, CheckAccount account, CancellationToken ct = default)
    {
        var api = await _client.PutAsync($"CheckAccount/{id}", ModelMapper.ToApi(account),
            SevDeskJsonContext.Default.ApiCheckAccount, SevDeskJsonContext.Default.SevDeskApiResponseApiCheckAccount, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) =>
        _client.DeleteAsync($"CheckAccount/{id}", ct);

    public async Task<decimal> GetBalanceAsync(int id, DateTime? date = null, CancellationToken ct = default)
    {
        var path = $"CheckAccount/{id}/getBalanceAtDate";
        if (date.HasValue)
            path += $"?date={date.Value:yyyy-MM-dd}";
        var json = await _client.GetStringAsync(path, ct).ConfigureAwait(false);
        var response = JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.ApiBalanceResponse);
        return response?.Objects ?? throw new Exceptions.SevDeskApiException("Failed to get check account balance.");
    }
}
