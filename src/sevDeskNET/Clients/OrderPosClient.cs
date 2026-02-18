using sevDeskNET.Internal;
using sevDeskNET.Internal.ApiModels;
using sevDeskNET.Models;

namespace sevDeskNET.Clients;

internal class OrderPosClient : IOrderPosClient
{
    private readonly BaseClient _client;

    internal OrderPosClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<OrderPos>> ListAsync(PaginationParameters? pagination = null, int? orderId = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("OrderPos", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiOrderPos,
            qb => { if (orderId.HasValue) qb.Add("order[id]", orderId.Value.ToString()).Add("order[objectName]", "Order"); }, ct).ConfigureAwait(false);
        return new SevDeskListResponse<OrderPos> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<OrderPos> GetAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"OrderPos/{id}", SevDeskJsonContext.Default.SevDeskApiResponseApiOrderPos, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<OrderPos> CreateAsync(OrderPos position, CancellationToken ct = default)
    {
        var api = await _client.PostAsync("OrderPos", ModelMapper.ToApi(position),
            SevDeskJsonContext.Default.ApiOrderPos, SevDeskJsonContext.Default.SevDeskApiResponseApiOrderPos, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<OrderPos> UpdateAsync(int id, OrderPos position, CancellationToken ct = default)
    {
        var api = await _client.PutAsync($"OrderPos/{id}", ModelMapper.ToApi(position),
            SevDeskJsonContext.Default.ApiOrderPos, SevDeskJsonContext.Default.SevDeskApiResponseApiOrderPos, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) =>
        _client.DeleteAsync($"OrderPos/{id}", ct);
}
