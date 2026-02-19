namespace sevDesk.NET.Models.Enums;

/// <summary>
/// Status of a credit note in sevDesk.
/// </summary>
public enum CreditNoteStatus
{
    /// <summary>Draft credit note.</summary>
    Draft = 100,

    /// <summary>Open credit note.</summary>
    Open = 200,

    /// <summary>Paid / settled credit note.</summary>
    Paid = 1000
}
