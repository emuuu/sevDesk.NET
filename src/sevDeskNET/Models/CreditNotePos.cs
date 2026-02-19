namespace sevDeskNET.Models;

/// <summary>
/// Represents a position (line item) on a sevDesk credit note.
/// </summary>
public class CreditNotePos
{
    /// <summary>Gets the position ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the credit note reference.</summary>
    public SevDeskObjectReference? CreditNote { get; init; }

    /// <summary>Gets or sets the part reference.</summary>
    public SevDeskObjectReference? Part { get; init; }

    /// <summary>Gets or sets the quantity.</summary>
    public decimal? Quantity { get; init; }

    /// <summary>Gets or sets the price (net, per unit).</summary>
    public decimal? Price { get; init; }

    /// <summary>Gets or sets the name / description.</summary>
    public string? Name { get; init; }

    /// <summary>Gets or sets the unity (unit of measure) reference.</summary>
    public SevDeskObjectReference? Unity { get; init; }

    /// <summary>Gets or sets the tax rate.</summary>
    public decimal? TaxRate { get; init; }

    /// <summary>Gets or sets the position number.</summary>
    public int? PositionNumber { get; init; }

    /// <summary>Gets or sets the text (additional description).</summary>
    public string? Text { get; init; }

    /// <summary>Gets or sets the discount percentage.</summary>
    public decimal? Discount { get; init; }

    /// <summary>Gets or sets whether this is optional.</summary>
    public bool? Optional { get; init; }

    /// <summary>Gets or sets the total net amount for this position.</summary>
    public decimal? SumNet { get; init; }

    /// <summary>Gets or sets the total gross amount for this position.</summary>
    public decimal? SumGross { get; init; }

    /// <summary>Gets or sets the total tax for this position.</summary>
    public decimal? SumTax { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }
}
