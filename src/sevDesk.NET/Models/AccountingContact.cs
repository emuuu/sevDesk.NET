namespace sevDesk.NET.Models;

/// <summary>
/// Represents the accounting contact of a sevDesk contact, holding the
/// bookkeeping numbers (debitor / creditor) used by DATEV exports.
/// </summary>
/// <remarks>
/// The API does not return a contact reference on this object, only
/// <see cref="ContactName"/>. To resolve an accounting contact to a specific contact,
/// query it through the <c>contactId</c> argument of
/// <see cref="Clients.IAccountingContactClient.ListAsync"/>.
/// </remarks>
public class AccountingContact
{
    /// <summary>Gets the accounting contact ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the name of the contact this accounting contact belongs to.</summary>
    public string? ContactName { get; init; }

    /// <summary>
    /// Gets or sets the debitor (customer) number. Kept as a string because the API
    /// returns it as one and bookkeeping numbers may carry leading zeros.
    /// </summary>
    public string? DebitorNumber { get; init; }

    /// <summary>Gets or sets the creditor (supplier) number. See <see cref="DebitorNumber"/>.</summary>
    public string? CreditorNumber { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }
}
