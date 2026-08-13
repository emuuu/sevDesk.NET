using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

internal class AccountingContactClient : IAccountingContactClient
{
    private readonly BaseClient _client;

    internal AccountingContactClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<AccountingContact>> ListAsync(PaginationParameters? pagination = null, int? contactId = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("AccountingContact", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiAccountingContact,
            qb =>
            {
                if (contactId.HasValue)
                {
                    qb.Add("contact[id]", contactId.Value.ToString());
                    qb.Add("contact[objectName]", "Contact");
                }
            }, ct).ConfigureAwait(false);
        return new SevDeskListResponse<AccountingContact> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<AccountingContact> GetAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"AccountingContact/{id}", SevDeskJsonContext.Default.SevDeskApiResponseApiAccountingContact, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }
}
