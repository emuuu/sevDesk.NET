using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

internal class CreditNotePosClient : ICreditNotePosClient
{
    private readonly BaseClient _client;

    internal CreditNotePosClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<CreditNotePos>> ListAsync(PaginationParameters? pagination = null, int? creditNoteId = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("CreditNotePos", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiCreditNotePos,
            qb => { if (creditNoteId.HasValue) qb.Add("creditNote[id]", creditNoteId.Value.ToString()).Add("creditNote[objectName]", "CreditNote"); }, ct).ConfigureAwait(false);
        return new SevDeskListResponse<CreditNotePos> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<CreditNotePos> GetAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"CreditNotePos/{id}", SevDeskJsonContext.Default.ApiCreditNotePos, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<CreditNotePos> CreateAsync(CreditNotePos position, CancellationToken ct = default)
    {
        var api = await _client.PostAsync("CreditNotePos", ModelMapper.ToApi(position),
            SevDeskJsonContext.Default.ApiCreditNotePos, SevDeskJsonContext.Default.SevDeskApiResponseApiCreditNotePos, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<CreditNotePos> UpdateAsync(int id, CreditNotePos position, CancellationToken ct = default)
    {
        var api = await _client.PutAsync($"CreditNotePos/{id}", ModelMapper.ToApi(position),
            SevDeskJsonContext.Default.ApiCreditNotePos, SevDeskJsonContext.Default.SevDeskApiResponseApiCreditNotePos, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) =>
        _client.DeleteAsync($"CreditNotePos/{id}", ct);
}
