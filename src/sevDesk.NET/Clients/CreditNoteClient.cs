using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

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
        var api = await _client.GetAsync($"CreditNote/{id}", SevDeskJsonContext.Default.ApiCreditNote,
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
        var write = await PostSaveCreditNoteAsync(creditNote, positions, ct).ConfigureAwait(false);
        return await BaseClient.ReadBackAfterWriteAsync(write, "CreditNote", () => GetAsync(write.Id, ct: ct)).ConfigureAwait(false);
    }

    public async Task<SevDeskObjectReference> SaveCreditNoteReferenceAsync(CreditNote creditNote, IEnumerable<CreditNotePos> positions, CancellationToken ct = default)
    {
        var write = await PostSaveCreditNoteAsync(creditNote, positions, ct).ConfigureAwait(false);
        return new SevDeskObjectReference { Id = write.Id, ObjectName = "CreditNote" };
    }

    public async Task<CreditNote> CreateFromInvoiceAsync(int invoiceId, CancellationToken ct = default)
    {
        var write = await PostCreateFromInvoiceAsync(invoiceId, ct).ConfigureAwait(false);
        return await BaseClient.ReadBackAfterWriteAsync(write, "CreditNote", () => GetAsync(write.Id, ct: ct)).ConfigureAwait(false);
    }

    public async Task<SevDeskObjectReference> CreateFromInvoiceReferenceAsync(int invoiceId, CancellationToken ct = default)
    {
        var write = await PostCreateFromInvoiceAsync(invoiceId, ct).ConfigureAwait(false);
        return new SevDeskObjectReference { Id = write.Id, ObjectName = "CreditNote" };
    }

    private Task<FactoryWriteResult> PostSaveCreditNoteAsync(CreditNote creditNote, IEnumerable<CreditNotePos> positions, CancellationToken ct)
    {
        var request = new ApiSaveCreditNoteRequest
        {
            CreditNote = ModelMapper.ToApi(creditNote),
            CreditNotePosSave = positions.Select(ModelMapper.ToApi).ToList()
        };
        return _client.PostFactoryAsync("CreditNote/Factory/saveCreditNote", request,
            SevDeskJsonContext.Default.ApiSaveCreditNoteRequest, "creditNote", "CreditNote", ct);
    }

    private Task<FactoryWriteResult> PostCreateFromInvoiceAsync(int invoiceId, CancellationToken ct)
    {
        var request = new ApiCreateFromInvoiceRequest
        {
            Invoice = new ApiObjectReference { Id = invoiceId, ObjectName = "Invoice" }
        };
        return _client.PostFactoryAsync("CreditNote/Factory/createFromInvoice", request,
            SevDeskJsonContext.Default.ApiCreateFromInvoiceRequest, "creditNote", "CreditNote", ct);
    }

    public Task<byte[]> GetPdfAsync(int id, CancellationToken ct = default) =>
        _client.GetBytesAsync($"CreditNote/{id}/getPdf", ct);

    public Task SendViaEmailAsync(int id, string email, string subject, string text, CancellationToken ct = default) =>
        _client.PostNoContentAsync($"CreditNote/{id}/sendViaEmail", new ApiSendEmailRequest { ToEmail = email, Subject = subject, Text = text },
            SevDeskJsonContext.Default.ApiSendEmailRequest, ct);
}
