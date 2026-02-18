namespace sevDeskNET.Models;

/// <summary>
/// Represents a sevDesk tax rule.
/// </summary>
public class TaxRule
{
    /// <summary>Gets the tax rule ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the tax rule name.</summary>
    public string? Name { get; init; }

    /// <summary>Gets or sets the tax rate percentage.</summary>
    public decimal? TaxRate { get; init; }

    /// <summary>Gets or sets whether this is a system default tax rule.</summary>
    public bool? IsDefault { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }
}
