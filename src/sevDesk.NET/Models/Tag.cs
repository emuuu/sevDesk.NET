namespace sevDesk.NET.Models;

/// <summary>
/// Represents a sevDesk tag.
/// </summary>
public class Tag
{
    /// <summary>Gets the tag ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the tag name.</summary>
    public string? Name { get; init; }

    /// <summary>Gets or sets the object reference this tag is attached to.</summary>
    public SevDeskObjectReference? Object { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }
}
