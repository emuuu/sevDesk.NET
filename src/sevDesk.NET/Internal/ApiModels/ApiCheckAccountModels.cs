using System.Text.Json.Serialization;

namespace sevDesk.NET.Internal.ApiModels;

internal class ApiCheckAccount
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public int? Type { get; set; }

    [JsonPropertyName("iban")]
    public string? Iban { get; set; }

    [JsonPropertyName("bic")]
    public string? Bic { get; set; }

    [JsonPropertyName("bankName")]
    public string? BankName { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("defaultAccount")]
    public bool? DefaultAccount { get; set; }

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("create")]
    public string? Create { get; set; }

    [JsonPropertyName("update")]
    public string? Update { get; set; }
}

internal class ApiCheckAccountTransaction
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("checkAccount")]
    public ApiObjectReference? CheckAccount { get; set; }

    [JsonPropertyName("valueDate")]
    public string? ValueDate { get; set; }

    [JsonPropertyName("entryDate")]
    public string? EntryDate { get; set; }

    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }

    [JsonPropertyName("payeeName")]
    public string? PayeeName { get; set; }

    [JsonPropertyName("purpose")]
    public string? Purpose { get; set; }

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("create")]
    public string? Create { get; set; }

    [JsonPropertyName("update")]
    public string? Update { get; set; }
}

internal class ApiBalanceResponse
{
    [JsonPropertyName("objects")]
    public decimal Objects { get; set; }
}
