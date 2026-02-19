using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

internal class ContactClient : IContactClient
{
    private readonly BaseClient _client;

    internal ContactClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<Contact>> ListAsync(PaginationParameters? pagination = null, string? embed = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("Contact", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiContact,
            qb => qb.AddIfNotNull("embed", embed), ct).ConfigureAwait(false);
        return new SevDeskListResponse<Contact> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<Contact> GetAsync(int id, string? embed = null, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"Contact/{id}", SevDeskJsonContext.Default.SevDeskApiResponseApiContact,
            qb => qb.AddIfNotNull("embed", embed), ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<Contact> CreateAsync(Contact contact, CancellationToken ct = default)
    {
        var api = await _client.PostAsync("Contact", ModelMapper.ToApi(contact),
            SevDeskJsonContext.Default.ApiContact, SevDeskJsonContext.Default.SevDeskApiResponseApiContact, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<Contact> UpdateAsync(int id, Contact contact, CancellationToken ct = default)
    {
        var api = await _client.PutAsync($"Contact/{id}", ModelMapper.ToApi(contact),
            SevDeskJsonContext.Default.ApiContact, SevDeskJsonContext.Default.SevDeskApiResponseApiContact, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) =>
        _client.DeleteAsync($"Contact/{id}", ct);

    public async Task<string> GetNextCustomerNumberAsync(CancellationToken ct = default)
    {
        var json = await _client.GetStringAsync("Contact/Factory/getNextCustomerNumber", ct).ConfigureAwait(false);
        var response = System.Text.Json.JsonSerializer.Deserialize(json, SevDeskJsonContext.Default.ApiGetNextNumberResponse);
        return response?.Objects ?? throw new Exceptions.SevDeskApiException("Failed to get next customer number.");
    }
}
