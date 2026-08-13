using System.Text.Json.Serialization;

namespace sevDesk.NET.Internal.ApiModels;

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

    [JsonPropertyName("titel")]
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

    [JsonPropertyName("exemptVat")]
    public string? ExemptVat { get; set; }

    [JsonPropertyName("birthday")]
    public string? Birthday { get; set; }

    [JsonPropertyName("defaultDiscountPercentage")]
    public string? DefaultDiscountPercentage { get; set; }

    [JsonPropertyName("governmentAgency")]
    public string? GovernmentAgency { get; set; }

    [JsonPropertyName("buyerReference")]
    public string? BuyerReference { get; set; }

    [JsonPropertyName("create")]
    public string? Create { get; set; }

    [JsonPropertyName("update")]
    public string? Update { get; set; }
}

internal class ApiAccountingContact
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("objectName")]
    public string? ObjectName { get; set; }

    [JsonPropertyName("contactName")]
    public string? ContactName { get; set; }

    [JsonPropertyName("debitorNumber")]
    public string? DebitorNumber { get; set; }

    [JsonPropertyName("creditorNumber")]
    public string? CreditorNumber { get; set; }

    [JsonPropertyName("create")]
    public string? Create { get; set; }

    [JsonPropertyName("update")]
    public string? Update { get; set; }
}

internal class ApiStaticCountry
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("objectName")]
    public string? ObjectName { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("nameEn")]
    public string? NameEn { get; set; }

    [JsonPropertyName("translationCode")]
    public string? TranslationCode { get; set; }

    [JsonPropertyName("locale")]
    public string? Locale { get; set; }

    [JsonPropertyName("priority")]
    public int? Priority { get; set; }
}
