using sevDesk.NET.Internal;
using sevDesk.NET.Internal.ApiModels;
using sevDesk.NET.Models;

namespace sevDesk.NET.Clients;

internal class CategoryClient : ICategoryClient
{
    private readonly BaseClient _client;

    internal CategoryClient(BaseClient client) => _client = client;

    public async Task<SevDeskListResponse<Category>> ListAsync(PaginationParameters? pagination = null, string? objectType = null, CancellationToken ct = default)
    {
        var (items, total) = await _client.GetListAsync("Category", pagination, SevDeskJsonContext.Default.SevDeskApiListResponseApiCategory,
            qb => qb.AddIfNotNull("objectType", objectType), ct).ConfigureAwait(false);
        return new SevDeskListResponse<Category> { Items = items.Select(ModelMapper.ToPublic).ToList(), Total = total };
    }

    public async Task<Category> GetAsync(int id, CancellationToken ct = default)
    {
        var api = await _client.GetAsync($"Category/{id}", SevDeskJsonContext.Default.ApiCategory, ct: ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<Category> CreateAsync(Category category, CancellationToken ct = default)
    {
        var api = await _client.PostAsync("Category", ModelMapper.ToApi(category),
            SevDeskJsonContext.Default.ApiCategory, SevDeskJsonContext.Default.SevDeskApiResponseApiCategory, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public async Task<Category> UpdateAsync(int id, Category category, CancellationToken ct = default)
    {
        var api = await _client.PutAsync($"Category/{id}", ModelMapper.ToApi(category),
            SevDeskJsonContext.Default.ApiCategory, SevDeskJsonContext.Default.SevDeskApiResponseApiCategory, ct).ConfigureAwait(false);
        return ModelMapper.ToPublic(api);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) =>
        _client.DeleteAsync($"Category/{id}", ct);
}
