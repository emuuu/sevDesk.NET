using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;
using sevDesk.NET.Models.Enums;

namespace sevDesk.NET.Clients;

internal class InvoiceClient : IInvoiceClient
{
    private readonly BaseClient _client;

    internal InvoiceClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<Invoice>> ListAsync(PaginationParameters? pagination = null, string? embed = null, InvoiceListFilter? filter = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("Invoice", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiInvoice,
            qb =>
            {
                qb.AddIfNotNull("embed", embed);
                if (filter is not null)
                {
                    if (filter.UpdateAfter.HasValue)
                    {
                        qb.Add("updateAfter", filter.UpdateAfter.Value.ToUnixTimeSeconds().ToString());
                    }

                    qb.AddIfNotNull("status", (int?)filter.Status);

                    if (filter.ContactId.HasValue)
                    {
                        qb.Add("contact[id]", filter.ContactId.Value.ToString());
                        qb.Add("contact[objectName]", "Contact");
                    }
                }
            }, ct).ConfigureAwait(false);
        return new SevDeskListResponse<Invoice> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<Invoice> GetAsync(int id, string? embed = null, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"Invoice/{id}", SevDeskJsonContext.Default.SevDeskApiResponseApiInvoice,
            qb => qb.AddIfNotNull("embed", embed), ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<Invoice> CreateAsync(Invoice invoice, CancellationToken ct = default)
    {
        var api = await _client.PostAsync("Invoice", ModelMapper.ToApi(invoice),
            SevDeskJsonContext.Default.ApiInvoice, SevDeskJsonContext.Default.SevDeskApiResponseApiInvoice, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<Invoice> UpdateAsync(int id, Invoice invoice, CancellationToken ct = default)
    {
        var api = await _client.PutAsync($"Invoice/{id}", ModelMapper.ToApi(invoice),
            SevDeskJsonContext.Default.ApiInvoice, SevDeskJsonContext.Default.SevDeskApiResponseApiInvoice, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) =>
        _client.DeleteAsync($"Invoice/{id}", ct);

    public async Task<Invoice> SaveInvoiceAsync(Invoice invoice, IEnumerable<InvoicePos> positions, CancellationToken ct = default)
    {
        var request = new ApiSaveInvoiceRequest
        {
            Invoice = ModelMapper.ToApi(invoice),
            InvoicePosSave = positions.Select(ModelMapper.ToApi).ToList()
        };
        var json = await _client.PostAndReadStringAsync("Invoice/Factory/saveInvoice", request,
            SevDeskJsonContext.Default.ApiSaveInvoiceRequest, ct).ConfigureAwait(false);
        var id = BaseClient.ParseFactoryResponseId(json, "invoice");
        return await GetAsync(id, ct: ct).ConfigureAwait(false);
    }

    public Task ChangeStatusAsync(int id, InvoiceStatus status, CancellationToken ct = default) =>
        _client.PutNoContentAsync($"Invoice/{id}/changeStatus", new ApiChangeStatusRequest { Value = (int)status },
            SevDeskJsonContext.Default.ApiChangeStatusRequest, ct);

    public Task<byte[]> GetPdfAsync(int id, CancellationToken ct = default) =>
        _client.GetBytesAsync($"Invoice/{id}/getPdf", ct);

    public Task SendViaEmailAsync(int id, string email, string subject, string text, CancellationToken ct = default) =>
        _client.PostNoContentAsync($"Invoice/{id}/sendViaEmail", new ApiSendEmailRequest { ToEmail = email, Subject = subject, Text = text },
            SevDeskJsonContext.Default.ApiSendEmailRequest, ct);

    public async Task<Invoice> DuplicateAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.PostAsync($"Invoice/{id}/duplicate", new ApiInvoice(),
            SevDeskJsonContext.Default.ApiInvoice, SevDeskJsonContext.Default.SevDeskApiResponseApiInvoice, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task CancelAsync(int id, CancellationToken ct = default) =>
        _client.PostNoContentAsync($"Invoice/{id}/cancel", new ApiInvoice(),
            SevDeskJsonContext.Default.ApiInvoice, ct);

    public Task MarkAsSentAsync(int id, CancellationToken ct = default) =>
        _client.PutNoContentAsync($"Invoice/{id}/changeStatus", new ApiChangeStatusRequest { Value = (int)InvoiceStatus.Open },
            SevDeskJsonContext.Default.ApiChangeStatusRequest, ct);

    public Task BookAmountAsync(int id, decimal amount, int checkAccountId, DateTime date, CancellationToken ct = default) =>
        _client.PostNoContentAsync($"Invoice/{id}/bookAmount", new ApiBookAmountRequest
        {
            Amount = amount,
            Date = date.ToString("yyyy-MM-dd"),
            Type = "N",
            CheckAccount = new ApiObjectReference { Id = checkAccountId, ObjectName = "CheckAccount" }
        }, SevDeskJsonContext.Default.ApiBookAmountRequest, ct);
}
