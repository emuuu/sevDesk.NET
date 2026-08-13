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

    /// <summary>
    /// Gets the full contact when it was requested via <c>embed=contact</c>;
    /// <see langword="null"/> otherwise. <see cref="Contact"/> always carries the reference.
    /// Read-only — only the reference is written back on create and update.
    /// </summary>
    public Contact? EmbeddedContact { get; init; }

    /// <summary>Gets or sets the invoice date.</summary>
    public DateTime? InvoiceDate { get; init; }

    /// <summary>Gets or sets the delivery date — the start of the service period.</summary>
    public DateTime? DeliveryDate { get; init; }

    /// <summary>
    /// Gets or sets the end of the service period. Together with <see cref="DeliveryDate"/> this
    /// spans a period rather than a single day; <see langword="null"/> when the invoice covers one date.
    /// </summary>
    public DateTime? DeliveryDateUntil { get; init; }

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

    /// <summary>Gets or sets the address text (the rendered multi-line block).</summary>
    public string? Address { get; init; }

    /// <summary>Gets or sets the recipient name of the invoice address.</summary>
    public string? AddressName { get; init; }

    /// <summary>Gets or sets the second recipient name line of the invoice address.</summary>
    public string? AddressName2 { get; init; }

    /// <summary>Gets or sets the street of the invoice address.</summary>
    public string? AddressStreet { get; init; }

    /// <summary>Gets or sets the postal code of the invoice address.</summary>
    public string? AddressZip { get; init; }

    /// <summary>Gets or sets the city of the invoice address.</summary>
    public string? AddressCity { get; init; }

    /// <summary>Gets or sets the country reference (<c>StaticCountry</c>) of the invoice address.</summary>
    public SevDeskObjectReference? AddressCountry { get; init; }

    /// <summary>Gets or sets the parent (company) name of the invoice address.</summary>
    public string? AddressParentName { get; init; }

    /// <summary>Gets or sets the second parent (company) name line of the invoice address.</summary>
    public string? AddressParentName2 { get; init; }

    /// <summary>Gets or sets the salutation of the invoice address.</summary>
    public string? AddressGender { get; init; }

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

    /// <summary>Gets or sets the e-invoice (XRechnung/ZUGFeRD) reference identifier.</summary>
    public string? EinvoiceReference { get; init; }

    /// <summary>Gets or sets whether this invoice is an e-invoice.</summary>
    public bool? PropertyIsEInvoice { get; init; }

    /// <summary>Gets the amount already paid on this invoice. Calculated by the API — read-only.</summary>
    public decimal? PaidAmount { get; init; }

    /// <summary>Gets the date the invoice was paid. Set by the API when payments are booked — read-only.</summary>
    public DateTime? PayDate { get; init; }

    /// <summary>
    /// Gets the invoice positions when they were requested via <c>embed=positions</c>;
    /// <see langword="null"/> otherwise. Read-only — use
    /// <see cref="Clients.IInvoiceClient.SaveInvoiceAsync"/> to write positions.
    /// </summary>
    public IReadOnlyList<InvoicePos>? Positions { get; init; }
}
