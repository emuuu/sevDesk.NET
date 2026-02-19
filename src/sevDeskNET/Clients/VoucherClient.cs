using sevDeskNET.Internal;
using sevDeskNET.Internal.ApiModels;
using sevDeskNET.Models;
using sevDeskNET.Models.Enums;

namespace sevDeskNET.Clients;

internal class VoucherClient : IVoucherClient
{
    private readonly BaseClient _client;

    internal VoucherClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<Voucher>> ListAsync(PaginationParameters? pagination = null, string? embed = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("Voucher", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiVoucher,
            qb => qb.AddIfNotNull("embed", embed), ct).ConfigureAwait(false);
        return new SevDeskListResponse<Voucher> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<Voucher> GetAsync(int id, string? embed = null, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"Voucher/{id}", SevDeskJsonContext.Default.SevDeskApiResponseApiVoucher,
            qb => qb.AddIfNotNull("embed", embed), ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<Voucher> CreateAsync(Voucher voucher, CancellationToken ct = default)
    {
        var api = await _client.PostAsync("Voucher", ModelMapper.ToApi(voucher),
            SevDeskJsonContext.Default.ApiVoucher, SevDeskJsonContext.Default.SevDeskApiResponseApiVoucher, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<Voucher> UpdateAsync(int id, Voucher voucher, CancellationToken ct = default)
    {
        var api = await _client.PutAsync($"Voucher/{id}", ModelMapper.ToApi(voucher),
            SevDeskJsonContext.Default.ApiVoucher, SevDeskJsonContext.Default.SevDeskApiResponseApiVoucher, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) =>
        _client.DeleteAsync($"Voucher/{id}", ct);

    public async Task<Voucher> SaveVoucherAsync(Voucher voucher, IEnumerable<VoucherPos> positions, string? filename = null, CancellationToken ct = default)
    {
        var request = new ApiSaveVoucherRequest
        {
            Voucher = ModelMapper.ToApi(voucher),
            VoucherPosSave = positions.Select(ModelMapper.ToApi).ToList(),
            Filename = filename
        };
        var json = await _client.PostAndReadStringAsync("Voucher/Factory/saveVoucher", request,
            SevDeskJsonContext.Default.ApiSaveVoucherRequest, ct).ConfigureAwait(false);
        var id = BaseClient.ParseFactoryResponseId(json, "voucher");
        return await GetAsync(id, ct: ct).ConfigureAwait(false);
    }

    public Task BookAmountAsync(int id, decimal amount, int checkAccountId, DateTime date, CancellationToken ct = default) =>
        _client.PostNoContentAsync($"Voucher/{id}/bookAmount", new ApiBookAmountRequest
        {
            Amount = amount,
            Date = date.ToString("yyyy-MM-dd"),
            Type = "N",
            CheckAccount = new ApiObjectReference { Id = checkAccountId, ObjectName = "CheckAccount" }
        }, SevDeskJsonContext.Default.ApiBookAmountRequest, ct);

    public Task MarkAsPaidAsync(int id, CancellationToken ct = default) =>
        _client.PutNoContentAsync($"Voucher/{id}/changeStatus", new ApiChangeStatusRequest { Value = (int)VoucherStatus.Paid },
            SevDeskJsonContext.Default.ApiChangeStatusRequest, ct);

    public Task MarkAsOpenAsync(int id, CancellationToken ct = default) =>
        _client.PutNoContentAsync($"Voucher/{id}/changeStatus", new ApiChangeStatusRequest { Value = (int)VoucherStatus.Unpaid },
            SevDeskJsonContext.Default.ApiChangeStatusRequest, ct);

    public async Task<Document> UploadFileAsync(Stream stream, string fileName, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent();
        using var streamContent = new StreamContent(stream);
        content.Add(streamContent, "file", fileName);
        var api = await _client.PostMultipartAsync("Voucher/Factory/uploadTempFile", content,
            SevDeskJsonContext.Default.SevDeskApiResponseApiDocument, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }
}
