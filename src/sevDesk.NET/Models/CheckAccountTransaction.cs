namespace sevDesk.NET.Models;

/// <summary>
/// Represents a transaction on a sevDesk check account.
/// </summary>
public class CheckAccountTransaction
{
    /// <summary>Gets the transaction ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the check account reference.</summary>
    public SevDeskObjectReference? CheckAccount { get; init; }

    /// <summary>Gets or sets the value date.</summary>
    public DateTime? ValueDate { get; init; }

    /// <summary>Gets or sets the entry date.</summary>
    public DateTime? EntryDate { get; init; }

    /// <summary>Gets or sets the amount.</summary>
    public decimal? Amount { get; init; }

    /// <summary>Gets or sets the payee/payer name.</summary>
    public string? PayeeName { get; init; }

    /// <summary>Gets or sets the purpose text.</summary>
    public string? Purpose { get; init; }

    /// <summary>Gets or sets the status (100=created, 200=linked, 300=private).</summary>
    public int? Status { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }
}
