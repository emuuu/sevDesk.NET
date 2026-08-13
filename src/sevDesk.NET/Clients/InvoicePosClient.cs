using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

internal class InvoicePosClient : IInvoicePosClient
{
    private readonly BaseClient _client;

    internal InvoicePosClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<InvoicePos>> ListAsync(PaginationParameters? pagination = null, int? invoiceId = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("InvoicePos", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiInvoicePos,
            qb => { if (invoiceId.HasValue) qb.Add("invoice[id]", invoiceId.Value.ToString()).Add("invoice[objectName]", "Invoice"); }, ct).ConfigureAwait(false);
        return new SevDeskListResponse<InvoicePos> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<InvoicePos> GetAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"InvoicePos/{id}", SevDeskJsonContext.Default.ApiInvoicePos, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<InvoicePos> CreateAsync(InvoicePos position, CancellationToken ct = default)
    {
        var api = await _client.PostAsync("InvoicePos", ModelMapper.ToApi(position),
            SevDeskJsonContext.Default.ApiInvoicePos, SevDeskJsonContext.Default.SevDeskApiResponseApiInvoicePos, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<InvoicePos> UpdateAsync(int id, InvoicePos position, CancellationToken ct = default)
    {
        var api = await _client.PutAsync($"InvoicePos/{id}", ModelMapper.ToApi(position),
            SevDeskJsonContext.Default.ApiInvoicePos, SevDeskJsonContext.Default.SevDeskApiResponseApiInvoicePos, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) =>
        _client.DeleteAsync($"InvoicePos/{id}", ct);
}
