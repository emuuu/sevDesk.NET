using sevDeskNET.Models.Enums;

namespace sevDeskNET.Models;

/// <summary>
/// Represents a communication way (email, phone, etc.) for a sevDesk contact.
/// </summary>
public class CommunicationWay
{
    /// <summary>Gets the communication way ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the contact reference.</summary>
    public SevDeskObjectReference? Contact { get; init; }

    /// <summary>Gets or sets the type (EMAIL, PHONE, WEB, MOBILE).</summary>
    public CommunicationWayType? Type { get; init; }

    /// <summary>Gets or sets the value (email address, phone number, URL).</summary>
    public string? Value { get; init; }

    /// <summary>Gets or sets the key (purpose).</summary>
    public SevDeskObjectReference? Key { get; init; }

    /// <summary>Gets or sets whether this is the main communication way.</summary>
    public bool? Main { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }
}
