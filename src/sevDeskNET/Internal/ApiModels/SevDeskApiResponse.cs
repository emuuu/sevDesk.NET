using System.Text.Json.Serialization;

namespace sevDeskNET.Internal.ApiModels;

internal class SevDeskApiResponse<T>
{
    [JsonPropertyName("objects")]
    public T? Objects { get; set; }
}

internal class SevDeskApiListResponse<T>
{
    [JsonPropertyName("objects")]
    public List<T>? Objects { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}
