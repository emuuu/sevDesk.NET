using sevDesk.NET.Models.Enums;

namespace sevDesk.NET.Models;

/// <summary>
/// Represents a sevDesk invoice.
/// </summary>
public class Invoice
{
    /// <summary>Gets the invoice ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the invoice number.</summary>
    public string? InvoiceNumber { get; init; }

    /// <summary>Gets or sets the contact reference.</summary>
    public SevDeskObjectReference? Contact { get; init; }

    /// <summary>Gets or sets the invoice date.</summary>
    public DateTime? InvoiceDate { get; init; }

    /// <summary>Gets or sets the delivery date.</summary>
    public DateTime? DeliveryDate { get; init; }

    /// <summary>Gets or sets the invoice status.</summary>
    public InvoiceStatus? Status { get; init; }

    /// <summary>Gets or sets the invoice type.</summary>
    public InvoiceType? InvoiceType { get; init; }

    /// <summary>Gets or sets the header text.</summary>
    public string? Header { get; init; }

    /// <summary>Gets or sets the head text (above positions).</summary>
    public string? HeadText { get; init; }

    /// <summary>Gets or sets the foot text (below positions).</summary>
    public string? FootText { get; init; }

    /// <summary>Gets or sets the time to pay in days.</summary>
    public int? TimeToPay { get; init; }

    /// <summary>Gets or sets the discount time in days.</summary>
    public int? DiscountTime { get; init; }

    /// <summary>Gets or sets the discount percentage.</summary>
    public decimal? Discount { get; init; }

    /// <summary>Gets or sets the contact person reference.</summary>
    public SevDeskObjectReference? ContactPerson { get; init; }

    /// <summary>Gets or sets the address text.</summary>
    public string? Address { get; init; }

    /// <summary>Gets or sets the currency (ISO 4217).</summary>
    public string? Currency { get; init; }

    /// <summary>Gets or sets the total net amount.</summary>
    public decimal? SumNet { get; init; }

    /// <summary>Gets or sets the total gross amount.</summary>
    public decimal? SumGross { get; init; }

    /// <summary>Gets or sets the total tax amount.</summary>
    public decimal? SumTax { get; init; }

    /// <summary>Gets or sets the tax type (default, eu, noteu, custom).</summary>
    public string? TaxType { get; init; }

    /// <summary>Gets or sets the tax rate.</summary>
    public decimal? TaxRate { get; init; }

    /// <summary>Gets or sets the tax text.</summary>
    public string? TaxText { get; init; }

    /// <summary>Gets or sets the send date.</summary>
    public DateTime? SendDate { get; init; }

    /// <summary>Gets or sets the payment method reference.</summary>
    public SevDeskObjectReference? PaymentMethod { get; init; }

    /// <summary>Gets or sets the cost centre reference.</summary>
    public SevDeskObjectReference? CostCentre { get; init; }

    /// <summary>Gets or sets the send type (VPR, VPDF, VM, VP).</summary>
    public string? SendType { get; init; }

    /// <summary>Gets or sets the origin reference.</summary>
    public SevDeskObjectReference? Origin { get; init; }

    /// <summary>Gets or sets the customer internal note.</summary>
    public string? CustomerInternalNote { get; init; }

    /// <summary>Gets or sets whether this is a small settlement (Kleinunternehmer).</summary>
    public bool? SmallSettlement { get; init; }

    /// <summary>Gets or sets the tax set reference.</summary>
    public SevDeskObjectReference? TaxSet { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }

    /// <summary>Gets or sets the tax rule reference.</summary>
    public SevDeskObjectReference? TaxRule { get; init; }
}
