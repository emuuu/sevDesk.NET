using System.Text.Json.Serialization;

namespace sevDesk.NET.Internal.ApiModels;

internal class ApiPart
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("partNumber")]
    public string? PartNumber { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("unity")]
    public ApiObjectReference? Unity { get; set; }

    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    [JsonPropertyName("priceGross")]
    public decimal? PriceGross { get; set; }

    [JsonPropertyName("priceNet")]
    public decimal? PriceNet { get; set; }

    [JsonPropertyName("taxRate")]
    public decimal? TaxRate { get; set; }

    [JsonPropertyName("internalComment")]
    public string? InternalComment { get; set; }

    [JsonPropertyName("stockEnabled")]
    public string? StockEnabled { get; set; }

    [JsonPropertyName("stock")]
    public decimal? Stock { get; set; }

    [JsonPropertyName("category")]
    public ApiObjectReference? Category { get; set; }

    [JsonPropertyName("create")]
    public string? Create { get; set; }

    [JsonPropertyName("update")]
    public string? Update { get; set; }
}

internal class ApiCommunicationWay
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("contact")]
    public ApiObjectReference? Contact { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("key")]
    public ApiObjectReference? Key { get; set; }

    [JsonPropertyName("main")]
    public string? Main { get; set; }

    [JsonPropertyName("create")]
    public string? Create { get; set; }

    [JsonPropertyName("update")]
    public string? Update { get; set; }
}

internal class ApiContactAddress
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("contact")]
    public ApiObjectReference? Contact { get; set; }

    [JsonPropertyName("street")]
    public string? Street { get; set; }

    [JsonPropertyName("zip")]
    public string? Zip { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("country")]
    public ApiObjectReference? Country { get; set; }

    [JsonPropertyName("category")]
    public ApiObjectReference? Category { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("name2")]
    public string? Name2 { get; set; }

    [JsonPropertyName("name3")]
    public string? Name3 { get; set; }

    [JsonPropertyName("name4")]
    public string? Name4 { get; set; }

    [JsonPropertyName("create")]
    public string? Create { get; set; }

    [JsonPropertyName("update")]
    public string? Update { get; set; }
}

internal class ApiTag
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("object")]
    public ApiObjectReference? Object { get; set; }

    [JsonPropertyName("objectType")]
    public string? ObjectType { get; set; }

    [JsonPropertyName("create")]
    public string? Create { get; set; }
}

internal class ApiCategory
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("objectType")]
    public string? ObjectType { get; set; }

    [JsonPropertyName("priority")]
    public int? Priority { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [JsonPropertyName("postingAccount")]
    public string? PostingAccount { get; set; }

    [JsonPropertyName("translationCode")]
    public string? TranslationCode { get; set; }

    [JsonPropertyName("create")]
    public string? Create { get; set; }

    [JsonPropertyName("update")]
    public string? Update { get; set; }
}

internal class ApiUnity
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("translationCode")]
    public string? TranslationCode { get; set; }

    [JsonPropertyName("unitySystem")]
    public string? UnitySystem { get; set; }

    [JsonPropertyName("uneceTradeUnitCode")]
    public string? UneceTradeUnitCode { get; set; }

    [JsonPropertyName("create")]
    public string? Create { get; set; }

    [JsonPropertyName("update")]
    public string? Update { get; set; }
}

internal class ApiTaxRule
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("objectName")]
    public string? ObjectName { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("countryClient")]
    public ApiObjectReference? CountryClient { get; set; }

    [JsonPropertyName("countryContactType")]
    public string? CountryContactType { get; set; }
}

internal class ApiCurrencyExchangeRate
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("objectName")]
    public string? ObjectName { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("rate")]
    public decimal? Rate { get; set; }

    [JsonPropertyName("date")]
    public string? Date { get; set; }
}

internal class ApiDocument
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("filename")]
    public string? Filename { get; set; }

    [JsonPropertyName("extension")]
    public string? Extension { get; set; }

    [JsonPropertyName("size")]
    public long? Size { get; set; }

    [JsonPropertyName("mimeType")]
    public string? MimeType { get; set; }

    [JsonPropertyName("object")]
    public ApiObjectReference? Object { get; set; }

    [JsonPropertyName("folder")]
    public ApiObjectReference? Folder { get; set; }

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("create")]
    public string? Create { get; set; }

    [JsonPropertyName("update")]
    public string? Update { get; set; }
}
