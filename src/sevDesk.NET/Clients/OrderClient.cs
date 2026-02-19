using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;
using sevDesk.NET.Models.Enums;

namespace sevDesk.NET.Clients;

internal class OrderClient : IOrderClient
{
    private readonly BaseClient _client;

    internal OrderClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<Order>> ListAsync(PaginationParameters? pagination = null, string? embed = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("Order", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiOrder,
            qb => qb.AddIfNotNull("embed", embed), ct).ConfigureAwait(false);
        return new SevDeskListResponse<Order> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<Order> GetAsync(int id, string? embed = null, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"Order/{id}", SevDeskJsonContext.Default.SevDeskApiResponseApiOrder,
            qb => qb.AddIfNotNull("embed", embed), ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<Order> CreateAsync(Order order, CancellationToken ct = default)
    {
        var api = await _client.PostAsync("Order", ModelMapper.ToApi(order),
            SevDeskJsonContext.Default.ApiOrder, SevDeskJsonContext.Default.SevDeskApiResponseApiOrder, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<Order> UpdateAsync(int id, Order order, CancellationToken ct = default)
    {
        var api = await _client.PutAsync($"Order/{id}", ModelMapper.ToApi(order),
            SevDeskJsonContext.Default.ApiOrder, SevDeskJsonContext.Default.SevDeskApiResponseApiOrder, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) =>
        _client.DeleteAsync($"Order/{id}", ct);

    public async Task<Order> SaveOrderAsync(Order order, IEnumerable<OrderPos> positions, CancellationToken ct = default)
    {
        var request = new ApiSaveOrderRequest
        {
            Order = ModelMapper.ToApi(order),
            OrderPosSave = positions.Select(ModelMapper.ToApi).ToList()
        };
        var json = await _client.PostAndReadStringAsync("Order/Factory/saveOrder", request,
            SevDeskJsonContext.Default.ApiSaveOrderRequest, ct).ConfigureAwait(false);
        var id = BaseClient.ParseFactoryResponseId(json, "order");
        return await GetAsync(id, ct: ct).ConfigureAwait(false);
    }

    public Task ChangeStatusAsync(int id, OrderStatus status, CancellationToken ct = default) =>
        _client.PutNoContentAsync($"Order/{id}/changeStatus", new ApiChangeStatusRequest { Value = (int)status },
            SevDeskJsonContext.Default.ApiChangeStatusRequest, ct);

    public Task<byte[]> GetPdfAsync(int id, CancellationToken ct = default) =>
        _client.GetBytesAsync($"Order/{id}/getPdf", ct);

    public Task SendViaEmailAsync(int id, string email, string subject, string text, CancellationToken ct = default) =>
        _client.PostNoContentAsync($"Order/{id}/sendViaEmail", new ApiSendEmailRequest { ToEmail = email, Subject = subject, Text = text },
            SevDeskJsonContext.Default.ApiSendEmailRequest, ct);

    public async Task<Order> DuplicateAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.PostAsync($"Order/{id}/duplicate", new ApiOrder(),
            SevDeskJsonContext.Default.ApiOrder, SevDeskJsonContext.Default.SevDeskApiResponseApiOrder, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }
}
