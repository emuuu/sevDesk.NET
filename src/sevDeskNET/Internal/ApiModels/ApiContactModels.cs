using System.Text.Json.Serialization;

namespace sevDeskNET.Internal.ApiModels;

internal class ApiContact
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("customerNumber")]
    public string? CustomerNumber { get; set; }

    [JsonPropertyName("surename")]
    public string? Surename { get; set; }

    [JsonPropertyName("familyname")]
    public string? Familyname { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("name2")]
    public string? Name2 { get; set; }

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("academicTitle")]
    public string? AcademicTitle { get; set; }

    [JsonPropertyName("gender")]
    public string? Gender { get; set; }

    [JsonPropertyName("category")]
    public ApiObjectReference? Category { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("vatNumber")]
    public string? VatNumber { get; set; }

    [JsonPropertyName("bankAccount")]
    public string? BankAccount { get; set; }

    [JsonPropertyName("bankNumber")]
    public string? BankNumber { get; set; }

    [JsonPropertyName("defaultCashbackTime")]
    public int? DefaultCashbackTime { get; set; }

    [JsonPropertyName("defaultCashbackPercent")]
    public decimal? DefaultCashbackPercent { get; set; }

    [JsonPropertyName("defaultTimeToPay")]
    public int? DefaultTimeToPay { get; set; }

    [JsonPropertyName("taxNumber")]
    public string? TaxNumber { get; set; }

    [JsonPropertyName("taxOffice")]
    public string? TaxOffice { get; set; }

    [JsonPropertyName("create")]
    public string? Create { get; set; }

    [JsonPropertyName("update")]
    public string? Update { get; set; }
}
