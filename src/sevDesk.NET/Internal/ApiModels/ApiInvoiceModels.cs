using System.Text.Json.Serialization;

namespace sevDesk.NET.Internal.ApiModels;

internal class ApiInvoice
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("invoiceNumber")]
    public string? InvoiceNumber { get; set; }

    [JsonPropertyName("contact")]
    public ApiObjectReference? Contact { get; set; }

    [JsonPropertyName("invoiceDate")]
    public string? InvoiceDate { get; set; }

    [JsonPropertyName("deliveryDate")]
    public string? DeliveryDate { get; set; }

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("invoiceType")]
    public string? InvoiceType { get; set; }

    [JsonPropertyName("header")]
    public string? Header { get; set; }

    [JsonPropertyName("headText")]
    public string? HeadText { get; set; }

    [JsonPropertyName("footText")]
    public string? FootText { get; set; }

    [JsonPropertyName("timeToPay")]
    public int? TimeToPay { get; set; }

    [JsonPropertyName("discountTime")]
    public int? DiscountTime { get; set; }

    [JsonPropertyName("discount")]
    public decimal? Discount { get; set; }

    [JsonPropertyName("contactPerson")]
    public ApiObjectReference? ContactPerson { get; set; }

    [JsonPropertyName("address")]
    public string? Address { get; set; }

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

    [JsonPropertyName("taxRate")]
    public decimal? TaxRate { get; set; }

    [JsonPropertyName("taxText")]
    public string? TaxText { get; set; }

    [JsonPropertyName("sendDate")]
    public string? SendDate { get; set; }

    [JsonPropertyName("paymentMethod")]
    public string? PaymentMethod { get; set; }

    [JsonPropertyName("costCentre")]
    public ApiObjectReference? CostCentre { get; set; }

    [JsonPropertyName("sendType")]
    public string? SendType { get; set; }

    [JsonPropertyName("origin")]
    public ApiObjectReference? Origin { get; set; }

    [JsonPropertyName("customerInternalNote")]
    public string? CustomerInternalNote { get; set; }

    [JsonPropertyName("smallSettlement")]
    public string? SmallSettlement { get; set; }

    [JsonPropertyName("taxSet")]
    public ApiObjectReference? TaxSet { get; set; }

    [JsonPropertyName("create")]
    public string? Create { get; set; }

    [JsonPropertyName("update")]
    public string? Update { get; set; }
}

internal class ApiInvoicePos
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("invoice")]
    public ApiObjectReference? Invoice { get; set; }

    [JsonPropertyName("part")]
    public ApiObjectReference? Part { get; set; }

    [JsonPropertyName("quantity")]
    public decimal? Quantity { get; set; }

    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("unity")]
    public ApiObjectReference? Unity { get; set; }

    [JsonPropertyName("taxRate")]
    public decimal? TaxRate { get; set; }

    [JsonPropertyName("positionNumber")]
    public int? PositionNumber { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("discount")]
    public decimal? Discount { get; set; }

    [JsonPropertyName("optional")]
    public string? Optional { get; set; }

    [JsonPropertyName("sumNet")]
    public string? SumNet { get; set; }

    [JsonPropertyName("sumGross")]
    public string? SumGross { get; set; }

    [JsonPropertyName("sumTax")]
    public string? SumTax { get; set; }

    [JsonPropertyName("create")]
    public string? Create { get; set; }

    [JsonPropertyName("update")]
    public string? Update { get; set; }

    [JsonPropertyName("mapAll")]
    public bool? MapAll { get; set; }
}
