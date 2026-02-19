using System.Text.Json.Serialization;

namespace sevDeskNET.Internal.ApiModels;

internal class ApiObjectReference
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("objectName")]
    public string? ObjectName { get; set; }
}

internal class ApiSaveInvoiceRequest
{
    [JsonPropertyName("invoice")]
    public required ApiInvoice Invoice { get; set; }

    [JsonPropertyName("invoicePosSave")]
    public List<ApiInvoicePos>? InvoicePosSave { get; set; }
}

internal class ApiSaveOrderRequest
{
    [JsonPropertyName("order")]
    public required ApiOrder Order { get; set; }

    [JsonPropertyName("orderPosSave")]
    public List<ApiOrderPos>? OrderPosSave { get; set; }
}

internal class ApiSaveVoucherRequest
{
    [JsonPropertyName("voucher")]
    public required ApiVoucher Voucher { get; set; }

    [JsonPropertyName("voucherPosSave")]
    public List<ApiVoucherPos>? VoucherPosSave { get; set; }

    [JsonPropertyName("filename")]
    public string? Filename { get; set; }
}

internal class ApiSaveCreditNoteRequest
{
    [JsonPropertyName("creditNote")]
    public required ApiCreditNote CreditNote { get; set; }

    [JsonPropertyName("creditNotePosSave")]
    public List<ApiCreditNotePos>? CreditNotePosSave { get; set; }
}

internal class ApiChangeStatusRequest
{
    [JsonPropertyName("value")]
    public int Value { get; set; }
}

internal class ApiSendEmailRequest
{
    [JsonPropertyName("toEmail")]
    public required string ToEmail { get; set; }

    [JsonPropertyName("subject")]
    public required string Subject { get; set; }

    [JsonPropertyName("text")]
    public required string Text { get; set; }
}

internal class ApiBookAmountRequest
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("date")]
    public string? Date { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("checkAccount")]
    public ApiObjectReference? CheckAccount { get; set; }
}

internal class ApiCreateFromInvoiceRequest
{
    [JsonPropertyName("invoice")]
    public required ApiObjectReference Invoice { get; set; }
}

internal class ApiGetNextNumberResponse
{
    [JsonPropertyName("objects")]
    public string? Objects { get; set; }
}
