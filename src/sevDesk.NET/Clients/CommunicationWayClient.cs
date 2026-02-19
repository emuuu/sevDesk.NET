using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

internal class CommunicationWayClient : ICommunicationWayClient
{
    private readonly BaseClient _client;

    internal CommunicationWayClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<CommunicationWay>> ListAsync(PaginationParameters? pagination = null, int? contactId = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("CommunicationWay", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiCommunicationWay,
            qb => { if (contactId.HasValue) qb.Add("contact[id]", contactId.Value.ToString()).Add("contact[objectName]", "Contact"); }, ct).ConfigureAwait(false);
        return new SevDeskListResponse<CommunicationWay> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<CommunicationWay> GetAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"CommunicationWay/{id}", SevDeskJsonContext.Default.SevDeskApiResponseApiCommunicationWay, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<CommunicationWay> CreateAsync(CommunicationWay communicationWay, CancellationToken ct = default)
    {
        var api = await _client.PostAsync("CommunicationWay", ModelMapper.ToApi(communicationWay),
            SevDeskJsonContext.Default.ApiCommunicationWay, SevDeskJsonContext.Default.SevDeskApiResponseApiCommunicationWay, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<CommunicationWay> UpdateAsync(int id, CommunicationWay communicationWay, CancellationToken ct = default)
    {
        var api = await _client.PutAsync($"CommunicationWay/{id}", ModelMapper.ToApi(communicationWay),
            SevDeskJsonContext.Default.ApiCommunicationWay, SevDeskJsonContext.Default.SevDeskApiResponseApiCommunicationWay, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) =>
        _client.DeleteAsync($"CommunicationWay/{id}", ct);
}
