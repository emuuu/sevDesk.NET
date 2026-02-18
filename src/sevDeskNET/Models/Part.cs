namespace sevDeskNET.Models;

/// <summary>
/// Represents a sevDesk part (product / service item).
/// </summary>
public class Part
{
    /// <summary>Gets the part ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the part name.</summary>
    public string? Name { get; init; }

    /// <summary>Gets or sets the part number.</summary>
    public string? PartNumber { get; init; }

    /// <summary>Gets or sets the description.</summary>
    public string? Text { get; init; }

    /// <summary>Gets or sets the unity (unit of measure) reference.</summary>
    public SevDeskObjectReference? Unity { get; init; }

    /// <summary>Gets or sets the net price.</summary>
    public decimal? Price { get; init; }

    /// <summary>Gets or sets the gross price.</summary>
    public decimal? PriceGross { get; init; }

    /// <summary>Gets or sets the net purchase price.</summary>
    public decimal? PriceNet { get; init; }

    /// <summary>Gets or sets the tax rate.</summary>
    public decimal? TaxRate { get; init; }

    /// <summary>Gets or sets the internal comment.</summary>
    public string? InternalComment { get; init; }

    /// <summary>Gets or sets whether this is a stock-tracked item.</summary>
    public bool? StockEnabled { get; init; }

    /// <summary>Gets or sets the current stock level.</summary>
    public decimal? Stock { get; init; }

    /// <summary>Gets or sets the category reference.</summary>
    public SevDeskObjectReference? Category { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }
}
