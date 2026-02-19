using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

internal class VoucherPosClient : IVoucherPosClient
{
    private readonly BaseClient _client;

    internal VoucherPosClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<VoucherPos>> ListAsync(PaginationParameters? pagination = null, int? voucherId = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("VoucherPos", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiVoucherPos,
            qb => { if (voucherId.HasValue) qb.Add("voucher[id]", voucherId.Value.ToString()).Add("voucher[objectName]", "Voucher"); }, ct).ConfigureAwait(false);
        return new SevDeskListResponse<VoucherPos> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<VoucherPos> GetAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"VoucherPos/{id}", SevDeskJsonContext.Default.SevDeskApiResponseApiVoucherPos, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<VoucherPos> CreateAsync(VoucherPos position, CancellationToken ct = default)
    {
        var api = await _client.PostAsync("VoucherPos", ModelMapper.ToApi(position),
            SevDeskJsonContext.Default.ApiVoucherPos, SevDeskJsonContext.Default.SevDeskApiResponseApiVoucherPos, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<VoucherPos> UpdateAsync(int id, VoucherPos position, CancellationToken ct = default)
    {
        var api = await _client.PutAsync($"VoucherPos/{id}", ModelMapper.ToApi(position),
            SevDeskJsonContext.Default.ApiVoucherPos, SevDeskJsonContext.Default.SevDeskApiResponseApiVoucherPos, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) =>
        _client.DeleteAsync($"VoucherPos/{id}", ct);
}
