using sevDeskNET.Models.Enums;

namespace sevDeskNET.Models;

/// <summary>
/// Represents a sevDesk credit note.
/// </summary>
public class CreditNote
{
    /// <summary>Gets the credit note ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the credit note number.</summary>
    public string? CreditNoteNumber { get; init; }

    /// <summary>Gets or sets the contact reference.</summary>
    public SevDeskObjectReference? Contact { get; init; }

    /// <summary>Gets or sets the credit note date.</summary>
    public DateTime? CreditNoteDate { get; init; }

    /// <summary>Gets or sets the credit note status.</summary>
    public CreditNoteStatus? Status { get; init; }

    /// <summary>Gets or sets the header text.</summary>
    public string? Header { get; init; }

    /// <summary>Gets or sets the head text.</summary>
    public string? HeadText { get; init; }

    /// <summary>Gets or sets the foot text.</summary>
    public string? FootText { get; init; }

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

    /// <summary>Gets or sets the tax type.</summary>
    public string? TaxType { get; init; }

    /// <summary>Gets or sets the tax rate.</summary>
    public decimal? TaxRate { get; init; }

    /// <summary>Gets or sets the tax text.</summary>
    public string? TaxText { get; init; }

    /// <summary>Gets or sets the tax set reference.</summary>
    public SevDeskObjectReference? TaxSet { get; init; }

    /// <summary>Gets or sets the send date.</summary>
    public DateTime? SendDate { get; init; }

    /// <summary>Gets or sets whether this is a small settlement.</summary>
    public bool? SmallSettlement { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }
}
