using System.Text.Json.Serialization;

namespace sevDesk.NET.Internal.ApiModels;

internal class ApiVoucher
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("voucherDate")]
    public string? VoucherDate { get; set; }

    [JsonPropertyName("supplier")]
    public ApiObjectReference? Supplier { get; set; }

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("voucherType")]
    public string? VoucherType { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("payDate")]
    public string? PayDate { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("sumNet")]
    public string? SumNet { get; set; }

    [JsonPropertyName("sumGross")]
    public string? SumGross { get; set; }

    [JsonPropertyName("sumTax")]
    public string? SumTax { get; set; }

    [JsonPropertyName("taxType")]
    public string? TaxType { get; set; }

    [JsonPropertyName("creditDebit")]
    public string? CreditDebit { get; set; }

    [JsonPropertyName("document")]
    public ApiObjectReference? Document { get; set; }

    [JsonPropertyName("costCentre")]
    public ApiObjectReference? CostCentre { get; set; }

    [JsonPropertyName("paidAmount")]
    public decimal? PaidAmount { get; set; }

    [JsonPropertyName("taxSet")]
    public ApiObjectReference? TaxSet { get; set; }

    [JsonPropertyName("create")]
    public string? Create { get; set; }

    [JsonPropertyName("update")]
    public string? Update { get; set; }
}

internal class ApiVoucherPos
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("voucher")]
    public ApiObjectReference? Voucher { get; set; }

    [JsonPropertyName("accountingType")]
    public ApiObjectReference? AccountingType { get; set; }

    [JsonPropertyName("estimatedAccountingType")]
    public ApiObjectReference? EstimatedAccountingType { get; set; }

    [JsonPropertyName("net")]
    public decimal? Net { get; set; }

    [JsonPropertyName("taxRate")]
    public decimal? TaxRate { get; set; }

    [JsonPropertyName("isAsset")]
    public bool? IsAsset { get; set; }

    [JsonPropertyName("sumNet")]
    public string? SumNet { get; set; }

    [JsonPropertyName("sumGross")]
    public string? SumGross { get; set; }

    [JsonPropertyName("sumTax")]
    public string? SumTax { get; set; }

    [JsonPropertyName("comment")]
    public string? Comment { get; set; }

    [JsonPropertyName("create")]
    public string? Create { get; set; }

    [JsonPropertyName("update")]
    public string? Update { get; set; }

    [JsonPropertyName("mapAll")]
    public bool? MapAll { get; set; }
}
