using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

internal class ContactAddressClient : IContactAddressClient
{
    private readonly BaseClient _client;

    internal ContactAddressClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<ContactAddress>> ListAsync(PaginationParameters? pagination = null, int? contactId = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("ContactAddress", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiContactAddress,
            qb => { if (contactId.HasValue) qb.Add("contact[id]", contactId.Value.ToString()).Add("contact[objectName]", "Contact"); }, ct).ConfigureAwait(false);
        return new SevDeskListResponse<ContactAddress> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<ContactAddress> GetAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"ContactAddress/{id}", SevDeskJsonContext.Default.ApiContactAddress, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<ContactAddress> CreateAsync(ContactAddress address, CancellationToken ct = default)
    {
        var api = await _client.PostAsync("ContactAddress", ModelMapper.ToApi(address),
            SevDeskJsonContext.Default.ApiContactAddress, SevDeskJsonContext.Default.SevDeskApiResponseApiContactAddress, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<ContactAddress> UpdateAsync(int id, ContactAddress address, CancellationToken ct = default)
    {
        var api = await _client.PutAsync($"ContactAddress/{id}", ModelMapper.ToApi(address),
            SevDeskJsonContext.Default.ApiContactAddress, SevDeskJsonContext.Default.SevDeskApiResponseApiContactAddress, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) =>
        _client.DeleteAsync($"ContactAddress/{id}", ct);
}
