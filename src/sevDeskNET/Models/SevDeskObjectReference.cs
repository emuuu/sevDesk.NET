namespace sevDeskNET.Models;

/// <summary>
/// Represents a reference to a sevDesk object (e.g. a contact, invoice, etc.).
/// </summary>
public class SevDeskObjectReference
{
    /// <summary>
    /// Gets the ID of the referenced object.
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Gets the object type name (e.g. "Contact", "Invoice").
    /// </summary>
    public required string ObjectName { get; init; }
}
