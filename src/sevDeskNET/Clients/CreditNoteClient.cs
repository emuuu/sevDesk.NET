using sevDeskNET.Internal;
using sevDeskNET.Internal.ApiModels;
using sevDeskNET.Models;

namespace sevDeskNET.Clients;

internal class CreditNoteClient : ICreditNoteClient
{
    private readonly BaseClient _client;

    internal CreditNoteClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<CreditNote>> ListAsync(PaginationParameters? pagination = null, string? embed = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("CreditNote", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiCreditNote,
            qb => qb.AddIfNotNull("embed", embed), ct).ConfigureAwait(false);
        return new SevDeskListResponse<CreditNote> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<CreditNote> GetAsync(int id, string? embed = null, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"CreditNote/{id}", SevDeskJsonContext.Default.SevDeskApiResponseApiCreditNote,
            qb => qb.AddIfNotNull("embed", embed), ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<CreditNote> CreateAsync(CreditNote creditNote, CancellationToken ct = default)
    {
        var api = await _client.PostAsync("CreditNote", ModelMapper.ToApi(creditNote),
            SevDeskJsonContext.Default.ApiCreditNote, SevDeskJsonContext.Default.SevDeskApiResponseApiCreditNote, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<CreditNote> UpdateAsync(int id, CreditNote creditNote, CancellationToken ct = default)
    {
        var api = await _client.PutAsync($"CreditNote/{id}", ModelMapper.ToApi(creditNote),
            SevDeskJsonContext.Default.ApiCreditNote, SevDeskJsonContext.Default.SevDeskApiResponseApiCreditNote, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) =>
        _client.DeleteAsync($"CreditNote/{id}", ct);

    public async Task<CreditNote> SaveCreditNoteAsync(CreditNote creditNote, IEnumerable<CreditNotePos> positions, CancellationToken ct = default)
    {
        var request = new ApiSaveCreditNoteRequest
        {
            CreditNote = ModelMapper.ToApi(creditNote),
            CreditNotePosSave = positions.Select(ModelMapper.ToApi).ToList()
        };
        var json = await _client.PostAndReadStringAsync("CreditNote/Factory/saveCreditNote", request,
            SevDeskJsonContext.Default.ApiSaveCreditNoteRequest, ct).ConfigureAwait(false);
        var id = BaseClient.ParseFactoryResponseId(json, "creditNote");
        return await GetAsync(id, ct: ct).ConfigureAwait(false);
    }

    public async Task<CreditNote> CreateFromInvoiceAsync(int invoiceId, CancellationToken ct = default)
    {
        var request = new ApiCreateFromInvoiceRequest
        {
            Invoice = new ApiObjectReference { Id = invoiceId, ObjectName = "Invoice" }
        };
        var json = await _client.PostAndReadStringAsync("CreditNote/Factory/createFromInvoice", request,
            SevDeskJsonContext.Default.ApiCreateFromInvoiceRequest, ct).ConfigureAwait(false);
        var id = BaseClient.ParseFactoryResponseId(json, "creditNote");
        return await GetAsync(id, ct: ct).ConfigureAwait(false);
    }

    public Task<byte[]> GetPdfAsync(int id, CancellationToken ct = default) =>
        _client.GetBytesAsync($"CreditNote/{id}/getPdf", ct);

    public Task SendViaEmailAsync(int id, string email, string subject, string text, CancellationToken ct = default) =>
        _client.PostNoContentAsync($"CreditNote/{id}/sendViaEmail", new ApiSendEmailRequest { ToEmail = email, Subject = subject, Text = text },
            SevDeskJsonContext.Default.ApiSendEmailRequest, ct);
}
