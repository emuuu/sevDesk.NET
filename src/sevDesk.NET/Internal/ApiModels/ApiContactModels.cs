using System.Text.Json.Serialization;

namespace sevDesk.NET.Internal.ApiModels;

internal class ApiContact
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("objectName")]
    public string? ObjectName { get; set; }

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

    /// <summary>
    /// True when this object carries more than the bare <c>{id, objectName}</c> reference shape,
    /// i.e. the API expanded it in response to an <c>embed</c> query parameter. Internal, so it is
    /// never serialized back to the API.
    /// </summary>
    internal bool IsEmbedded =>
        CustomerNumber is not null || Surename is not null || Familyname is not null ||
        Name is not null || Name2 is not null || Status is not null || Title is not null ||
        AcademicTitle is not null || Gender is not null || Category is not null ||
        Description is not null || VatNumber is not null || BankAccount is not null ||
        BankNumber is not null || DefaultCashbackTime is not null || DefaultCashbackPercent is not null ||
        DefaultTimeToPay is not null || TaxNumber is not null || TaxOffice is not null ||
        ExemptVat is not null || Birthday is not null || DefaultDiscountPercentage is not null ||
        GovernmentAgency is not null || BuyerReference is not null ||
        Create is not null || Update is not null;
}

internal class ApiAccountingContact
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("objectName")]
    public string? ObjectName { get; set; }

    [JsonPropertyName("contact")]
    public ApiObjectReference? Contact { get; set; }

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
