namespace sevDesk.NET.Models;

/// <summary>
/// Represents a document-level discount or surcharge of a sevDesk document (sevDesk object
/// <c>Discounts</c>), e.g. a customer discount of 3 % on the sum of all positions.
/// </summary>
/// <remarks>
/// Not to be confused with the early-payment discount of an invoice
/// (<see cref="Invoice.Discount"/> and <see cref="Invoice.DiscountTime"/>) or with the discount
/// of a single position (<see cref="InvoicePos.Discount"/>). A document-level discount refers to
/// the document as a whole and cannot be restricted to individual positions.
/// </remarks>
public class DocumentDiscount
{
    /// <summary>Gets the discount ID. <c>0</c> for a discount that has not been saved yet.</summary>
    public int Id { get; init; }

    /// <summary>Gets the document the discount belongs to. Read-only.</summary>
    public SevDeskObjectReference? Object { get; init; }

    /// <summary>Gets or sets the text shown on the document, e.g. "Kundenrabatt".</summary>
    public string? Text { get; init; }

    /// <summary>
    /// Gets or sets whether <see cref="Value"/> is a percentage (<see langword="true"/>) or an
    /// absolute amount in the document currency (<see langword="false"/>).
    /// </summary>
    public bool IsPercentage { get; init; }

    /// <summary>
    /// Gets or sets the value of the discount, as a positive number: the percentage if
    /// <see cref="IsPercentage"/> is set, the amount otherwise.
    /// </summary>
    public decimal Value { get; init; }

    /// <summary>
    /// Gets or sets whether this is a surcharge rather than a discount. Defaults to
    /// <see langword="false"/>, a discount.
    /// </summary>
    public bool IsSurcharge { get; init; }

    /// <summary>
    /// Gets whether the discount applies to the net amount. Read-only — sevDesk derives it from
    /// the document and does not accept it on save.
    /// </summary>
    /// <remarks>
    /// Mapped from the <c>isNet</c> field as <c>"1"</c> = net. The sevDesk API documentation
    /// contradicts itself on this field; the mapping follows the values the API returns for
    /// discounts on net invoices.
    /// </remarks>
    public bool? IsNet { get; init; }

    /// <summary>Gets the creation timestamp.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update timestamp.</summary>
    public DateTime? Update { get; init; }
}
