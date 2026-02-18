using sevDeskNET.Models.Enums;

namespace sevDeskNET.Models;

/// <summary>
/// Represents a sevDesk check account (bank account).
/// </summary>
public class CheckAccount
{
    /// <summary>Gets the account ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the account name.</summary>
    public string? Name { get; init; }

    /// <summary>Gets or sets the account type.</summary>
    public CheckAccountType? Type { get; init; }

    /// <summary>Gets or sets the IBAN.</summary>
    public string? Iban { get; init; }

    /// <summary>Gets or sets the BIC/SWIFT code.</summary>
    public string? Bic { get; init; }

    /// <summary>Gets or sets the bank name.</summary>
    public string? BankName { get; init; }

    /// <summary>Gets or sets the currency (ISO 4217).</summary>
    public string? Currency { get; init; }

    /// <summary>Gets or sets whether this is the default account.</summary>
    public bool? DefaultAccount { get; init; }

    /// <summary>Gets or sets the status (active=100, archived=0).</summary>
    public int? Status { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }
}
