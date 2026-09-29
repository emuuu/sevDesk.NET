using System.Text.Json.Serialization;

namespace sevDesk.NET.Internal.ApiModels;

internal class ApiInvoice
{
    /// <summary>
    /// Omitted while zero: a new object has no id yet, and sevDesk answers <c>"id":0</c> by
    /// looking up object 0 and rejecting the request.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Id { get; set; }

    /// <summary>Required by <c>Invoice/Factory/saveInvoice</c>; set by <see cref="ModelMapper.ToApiFactory(Models.Invoice)"/>.</summary>
    [JsonPropertyName("objectName")]
    public string? ObjectName { get; set; }

    /// <summary>Required by <c>Invoice/Factory/saveInvoice</c>; set by <see cref="ModelMapper.ToApiFactory(Models.Invoice)"/>.</summary>
    [JsonPropertyName("mapAll")]
    public bool? MapAll { get; set; }

    [JsonPropertyName("invoiceNumber")]
    public string? InvoiceNumber { get; set; }

    /// <summary>
    /// The contact. Typed as the full <see cref="ApiContact"/> because <c>embed=contact</c> expands
    /// this from the bare <c>{id, objectName}</c> reference into the complete contact object;
    /// see <see cref="ApiContact.IsEmbedded"/>. Only the reference fields are ever written back.
    /// </summary>
    [JsonPropertyName("contact")]
    public ApiContact? Contact { get; set; }

    [JsonPropertyName("invoiceDate")]
    public string? InvoiceDate { get; set; }

    [JsonPropertyName("deliveryDate")]
    public string? DeliveryDate { get; set; }

    [JsonPropertyName("deliveryDateUntil")]
    public string? DeliveryDateUntil { get; set; }

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

    [JsonPropertyName("addressName")]
    public string? AddressName { get; set; }

    [JsonPropertyName("addressName2")]
    public string? AddressName2 { get; set; }

    [JsonPropertyName("addressStreet")]
    public string? AddressStreet { get; set; }

    [JsonPropertyName("addressZip")]
    public string? AddressZip { get; set; }

    [JsonPropertyName("addressCity")]
    public string? AddressCity { get; set; }

    [JsonPropertyName("addressCountry")]
    public ApiObjectReference? AddressCountry { get; set; }

    [JsonPropertyName("addressParentName")]
    public string? AddressParentName { get; set; }

    [JsonPropertyName("addressParentName2")]
    public string? AddressParentName2 { get; set; }

    [JsonPropertyName("addressGender")]
    public string? AddressGender { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("sumNet")]
    public decimal? SumNet { get; set; }

    [JsonPropertyName("sumGross")]
    public decimal? SumGross { get; set; }

    [JsonPropertyName("sumTax")]
    public decimal? SumTax { get; set; }

    [JsonPropertyName("sumDiscounts")]
    public decimal? SumDiscounts { get; set; }

    [JsonPropertyName("sumDiscountNet")]
    public decimal? SumDiscountNet { get; set; }

    [JsonPropertyName("sumDiscountGross")]
    public decimal? SumDiscountGross { get; set; }

    [JsonPropertyName("taxType")]
    public string? TaxType { get; set; }

    [JsonPropertyName("taxRate")]
    public decimal? TaxRate { get; set; }

    [JsonPropertyName("taxText")]
    public string? TaxText { get; set; }

    [JsonPropertyName("sendDate")]
    public string? SendDate { get; set; }

    [JsonPropertyName("paymentMethod")]
    public ApiObjectReference? PaymentMethod { get; set; }

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

    [JsonPropertyName("taxRule")]
    public ApiObjectReference? TaxRule { get; set; }

    [JsonPropertyName("einvoiceReference")]
    public string? EinvoiceReference { get; set; }

    [JsonPropertyName("propertyIsEInvoice")]
    public string? PropertyIsEInvoice { get; set; }

    [JsonPropertyName("paidAmount")]
    public decimal? PaidAmount { get; set; }

    [JsonPropertyName("payDate")]
    public string? PayDate { get; set; }

    /// <summary>
    /// Positions embedded via <c>embed=positions</c>. Read-only — never serialized back
    /// to the API, which expects positions in the dedicated <c>invoicePosSave</c> array.
    /// </summary>
    [JsonPropertyName("positions")]
    public List<ApiInvoicePos>? Positions { get; set; }

    /// <summary>
    /// Discounts embedded via <c>embed=discounts</c>. Read-only — never serialized back
    /// to the API, which expects discounts in the dedicated <c>discountSave</c> array.
    /// </summary>
    [JsonPropertyName("discounts")]
    public List<ApiDiscount>? Discounts { get; set; }
}

internal class ApiInvoicePos
{
    /// <summary>
    /// Omitted while zero: a new object has no id yet, and sevDesk answers <c>"id":0</c> by
    /// looking up object 0 and rejecting the request.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Id { get; set; }

    [JsonPropertyName("objectName")]
    public string? ObjectName { get; set; }

    [JsonPropertyName("invoice")]
    public ApiObjectReference? Invoice { get; set; }

    [JsonPropertyName("part")]
    public ApiObjectReference? Part { get; set; }

    [JsonPropertyName("quantity")]
    public decimal? Quantity { get; set; }

    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    [JsonPropertyName("priceNet")]
    public decimal? PriceNet { get; set; }

    [JsonPropertyName("priceGross")]
    public decimal? PriceGross { get; set; }

    [JsonPropertyName("priceTax")]
    public decimal? PriceTax { get; set; }

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
