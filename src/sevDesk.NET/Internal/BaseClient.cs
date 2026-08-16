using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using sevDesk.NET.Exceptions;
using sevDesk.NET.Internal.ApiModels;

namespace sevDesk.NET.Internal;

internal class BaseClient
{
    private readonly HttpClient _httpClient;

    internal BaseClient(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _httpClient = httpClient;
    }

    internal async Task<(List<TApi> Items, int? Total)> GetListAsync<TApi>(
        string path,
        PaginationParameters? pagination,
        JsonTypeInfo<SevDeskApiListResponse<TApi>> typeInfo,
        Action<QueryBuilder>? configureQuery = null,
        CancellationToken ct = default)
    {
        var qb = new QueryBuilder().AddPagination(pagination);
        configureQuery?.Invoke(qb);
        var url = qb.Build(path);

        using var response = await _httpClient.GetAsync(url, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, ct).ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync(typeInfo, ct).ConfigureAwait(false)
            ?? throw new SevDeskApiException("Failed to deserialize sevDesk list response.");

        return (result.Objects ?? [], result.Total);
    }

    /// <summary>
    /// Reads a single object. The sevDesk API is inconsistent about the shape of <c>objects</c>:
    /// <c>GET /{Entity}/{id}</c> answers with a single-element array, while the create and update
    /// endpoints answer with a bare object. Both are accepted here.
    /// </summary>
    /// <param name="path">Request path, relative to the configured base address.</param>
    /// <param name="typeInfo">Type info of the entity itself, not of the <c>objects</c> envelope.</param>
    /// <param name="configureQuery">Optional callback to add query string parameters.</param>
    /// <param name="ct">Cancellation token.</param>
    internal async Task<TApi> GetAsync<TApi>(
        string path,
        JsonTypeInfo<TApi> typeInfo,
        Action<QueryBuilder>? configureQuery = null,
        CancellationToken ct = default)
    {
        var url = path;
        if (configureQuery is not null)
        {
            var qb = new QueryBuilder();
            configureQuery(qb);
            url = qb.Build(path);
        }

        using var response = await _httpClient.GetAsync(url, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, ct).ConfigureAwait(false);

        using var stream = await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
        using var document = await JsonDocument.ParseAsync(stream, cancellationToken: ct).ConfigureAwait(false);

        if (!document.RootElement.TryGetProperty("objects", out var objects))
        {
            throw new SevDeskApiException("Failed to deserialize sevDesk response: missing 'objects'.");
        }

        var element = objects.ValueKind switch
        {
            JsonValueKind.Object => objects,
            JsonValueKind.Array when objects.GetArrayLength() > 0 => objects[0],
            JsonValueKind.Array or JsonValueKind.Null => throw new SevDeskNotFoundException("Object not found."),
            _ => throw new SevDeskApiException($"Unexpected 'objects' shape in sevDesk response: {objects.ValueKind}.")
        };

        return element.Deserialize(typeInfo) ?? throw new SevDeskNotFoundException("Object not found.");
    }

    internal async Task<TResult> PostAsync<TRequest, TResult>(
        string path,
        TRequest body,
        JsonTypeInfo<TRequest> requestTypeInfo,
        JsonTypeInfo<SevDeskApiResponse<TResult>> responseTypeInfo,
        CancellationToken ct = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(path, body, requestTypeInfo, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, ct).ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync(responseTypeInfo, ct).ConfigureAwait(false)
            ?? throw new SevDeskApiException("Failed to deserialize sevDesk response.");

        return result.Objects ?? throw new SevDeskApiException("Response contained no objects.");
    }

    internal async Task<TResult> PutAsync<TRequest, TResult>(
        string path,
        TRequest body,
        JsonTypeInfo<TRequest> requestTypeInfo,
        JsonTypeInfo<SevDeskApiResponse<TResult>> responseTypeInfo,
        CancellationToken ct = default)
    {
        using var response = await _httpClient.PutAsJsonAsync(path, body, requestTypeInfo, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, ct).ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync(responseTypeInfo, ct).ConfigureAwait(false)
            ?? throw new SevDeskApiException("Failed to deserialize sevDesk response.");

        return result.Objects ?? throw new SevDeskApiException("Response contained no objects.");
    }

    internal async Task DeleteAsync(string path, CancellationToken ct = default)
    {
        using var response = await _httpClient.DeleteAsync(path, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, ct).ConfigureAwait(false);
    }

    internal async Task<byte[]> GetBytesAsync(string path, CancellationToken ct = default)
    {
        using var response = await _httpClient.GetAsync(path, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, ct).ConfigureAwait(false);
        return await response.Content.ReadAsByteArrayAsync(ct).ConfigureAwait(false);
    }

    internal async Task<TResult> PostMultipartAsync<TResult>(
        string path,
        MultipartFormDataContent content,
        JsonTypeInfo<SevDeskApiResponse<TResult>> responseTypeInfo,
        CancellationToken ct = default)
    {
        using var response = await _httpClient.PostAsync(path, content, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, ct).ConfigureAwait(false);

        var result = await response.Content.ReadFromJsonAsync(responseTypeInfo, ct).ConfigureAwait(false)
            ?? throw new SevDeskApiException("Failed to deserialize sevDesk response.");

        return result.Objects ?? throw new SevDeskApiException("Response contained no objects.");
    }

    internal async Task PostNoContentAsync<TRequest>(
        string path,
        TRequest body,
        JsonTypeInfo<TRequest> requestTypeInfo,
        CancellationToken ct = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(path, body, requestTypeInfo, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, ct).ConfigureAwait(false);
    }

    internal async Task PutNoContentAsync<TRequest>(
        string path,
        TRequest body,
        JsonTypeInfo<TRequest> requestTypeInfo,
        CancellationToken ct = default)
    {
        using var response = await _httpClient.PutAsJsonAsync(path, body, requestTypeInfo, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, ct).ConfigureAwait(false);
    }

    internal async Task<string> GetStringAsync(string path, CancellationToken ct = default)
    {
        using var response = await _httpClient.GetAsync(path, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, ct).ConfigureAwait(false);
        return await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Posts to a sevDesk <c>Factory</c> endpoint and parses the identifier out of the answer.
    /// </summary>
    /// <remarks>
    /// The post is the write. Anything that fails up to and including the status check means the
    /// object was not created, and the original exception propagates so the caller may retry.
    /// Anything that fails afterwards — reading the body, parsing the identifier — happened after
    /// the object came into existence and is rethrown as <see cref="SevDeskWriteSucceededException"/>
    /// with <see cref="SevDeskWriteSucceededException.ObjectId"/> unset, because a retry would
    /// create a duplicate.
    /// <para>
    /// The request is sent with <see cref="HttpCompletionOption.ResponseHeadersRead"/> on purpose.
    /// Under the default option the handler buffers the whole body before returning, so a connection
    /// dropping mid-body would surface as a transport error before the status code is ever seen —
    /// indistinguishable from a write that never arrived.
    /// </para>
    /// </remarks>
    /// <param name="path">Factory request path, relative to the configured base address.</param>
    /// <param name="body">The request payload.</param>
    /// <param name="requestTypeInfo">Type info of the request payload.</param>
    /// <param name="entityName">The property name carrying the object in the factory answer, e.g. <c>"invoice"</c>.</param>
    /// <param name="objectName">The sevDesk object type name, e.g. <c>"Invoice"</c>.</param>
    /// <param name="ct">Cancellation token.</param>
    internal async Task<FactoryWriteResult> PostFactoryAsync<TRequest>(
        string path,
        TRequest body,
        JsonTypeInfo<TRequest> requestTypeInfo,
        string entityName,
        string objectName,
        CancellationToken ct = default)
    {
        string json;

        using (var request = new HttpRequestMessage(HttpMethod.Post, path) { Content = JsonContent.Create(body, requestTypeInfo) })
        using (var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false))
        {
            // Everything up to here failing means the write did not happen.
            await EnsureSuccessAsync(response, ct).ConfigureAwait(false);

            // From here on the object exists in sevDesk and must not be written a second time.
            try
            {
                json = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new SevDeskWriteSucceededException(objectName, null, null, ex);
            }
        }

        try
        {
            return new FactoryWriteResult(ParseFactoryResponseId(json, entityName), json);
        }
        catch (Exception ex)
        {
            throw new SevDeskWriteSucceededException(objectName, null, json, ex);
        }
    }

    /// <summary>
    /// Runs the read-back that follows a confirmed factory write and reports any failure as
    /// <see cref="SevDeskWriteSucceededException"/>, carrying the identifier of the object that
    /// already exists.
    /// </summary>
    /// <param name="write">The confirmed write this read-back belongs to.</param>
    /// <param name="objectName">The sevDesk object type name, e.g. <c>"Invoice"</c>.</param>
    /// <param name="read">The read-back to run.</param>
    internal static async Task<TResult> ReadBackAfterWriteAsync<TResult>(
        FactoryWriteResult write,
        string objectName,
        Func<Task<TResult>> read)
    {
        try
        {
            return await read().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new SevDeskWriteSucceededException(objectName, write.Id, write.RawResponse, ex);
        }
    }

    internal async Task<string> PostStringContentAsync(string path, string jsonBody, CancellationToken ct = default)
    {
        using var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync(path, content, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, ct).ConfigureAwait(false);
        return await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
    }

    internal async Task<TResult> PostRawResponseAsync<TRequest, TResult>(
        string path,
        TRequest body,
        JsonTypeInfo<TRequest> requestTypeInfo,
        JsonTypeInfo<TResult> responseTypeInfo,
        CancellationToken ct = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(path, body, requestTypeInfo, ct).ConfigureAwait(false);
        await EnsureSuccessAsync(response, ct).ConfigureAwait(false);

        return await response.Content.ReadFromJsonAsync(responseTypeInfo, ct).ConfigureAwait(false)
            ?? throw new SevDeskApiException("Failed to deserialize sevDesk response.");
    }

    internal static int ParseFactoryResponseId(string json, string entityName)
    {
        using var doc = JsonDocument.Parse(json);
        if (!doc.RootElement.TryGetProperty("objects", out var objects) ||
            !objects.TryGetProperty(entityName, out var entity) ||
            !entity.TryGetProperty("id", out var idElement))
        {
            throw new SevDeskApiException($"Unexpected response format: missing objects.{entityName}.id");
        }

        return idElement.ValueKind == JsonValueKind.Number
            ? idElement.GetInt32()
            : int.TryParse(idElement.GetString(), CultureInfo.InvariantCulture, out var id)
                ? id
                : throw new SevDeskApiException($"Could not parse ID from factory response for {entityName}.");
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var rawBody = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        var detail = SevDeskErrorHelper.TryParseErrorDetail(rawBody);
        var message = $"sevDesk API returned {(int)response.StatusCode}: {detail}";

        throw response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => new SevDeskAuthenticationException(message, rawBody),
            HttpStatusCode.NotFound => new SevDeskNotFoundException(message, rawBody),
            HttpStatusCode.UnprocessableEntity => new SevDeskValidationException(message, rawBody),
            _ => new SevDeskApiException(message, response.StatusCode, rawBody)
        };
    }
}
