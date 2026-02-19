namespace sevDesk.NET.Models;

/// <summary>
/// Represents a postal address for a sevDesk contact.
/// </summary>
public class ContactAddress
{
    /// <summary>Gets the address ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the contact reference.</summary>
    public SevDeskObjectReference? Contact { get; init; }

    /// <summary>Gets or sets the street name.</summary>
    public string? Street { get; init; }

    /// <summary>Gets or sets the postal code / ZIP.</summary>
    public string? Zip { get; init; }

    /// <summary>Gets or sets the city name.</summary>
    public string? City { get; init; }

    /// <summary>Gets or sets the country reference.</summary>
    public SevDeskObjectReference? Country { get; init; }

    /// <summary>Gets or sets the category (e.g. invoice address, delivery address).</summary>
    public SevDeskObjectReference? Category { get; init; }

    /// <summary>Gets or sets the name (e.g. company name at this address).</summary>
    public string? Name { get; init; }

    /// <summary>Gets or sets the name2 (additional address line).</summary>
    public string? Name2 { get; init; }

    /// <summary>Gets or sets the name3 (additional address line).</summary>
    public string? Name3 { get; init; }

    /// <summary>Gets or sets the name4 (additional address line).</summary>
    public string? Name4 { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }
}
