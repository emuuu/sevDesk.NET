namespace sevDesk.NET.Models;

/// <summary>
/// Represents a sevDesk tax rule.
/// </summary>
public class TaxRule
{
    /// <summary>Gets the tax rule ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the tax rule name.</summary>
    public string? Name { get; init; }

    /// <summary>Gets or sets the description.</summary>
    public string? Description { get; init; }

    /// <summary>Gets or sets the tax rule code.</summary>
    public string? Code { get; init; }

    /// <summary>Gets or sets the country client reference (StaticCountry).</summary>
    public SevDeskObjectReference? CountryClient { get; init; }

    /// <summary>Gets or sets the country contact type.</summary>
    public string? CountryContactType { get; init; }
}
