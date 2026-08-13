using System.Text.Json.Serialization;

namespace sevDesk.NET.Internal.ApiModels;

internal class SevDeskApiResponse<T>
{
    [JsonPropertyName("objects")]
    public T? Objects { get; set; }
}

internal class SevDeskApiListResponse<T>
{
    [JsonPropertyName("objects")]
    public List<T>? Objects { get; set; }

    /// <summary>
    /// The <c>total</c> field the API only sends for <c>countAll=true</c>, and not reliably even
    /// then. <see langword="null"/> means it was absent or JSON <c>null</c>; the API sends the
    /// value as a JSON string, but a JSON number is read as well.
    /// </summary>
    [JsonPropertyName("total")]
    public int? Total { get; set; }
}
