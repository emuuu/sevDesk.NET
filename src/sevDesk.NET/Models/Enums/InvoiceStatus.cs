namespace sevDesk.NET.Models.Enums;

/// <summary>
/// Status of an invoice in sevDesk.
/// </summary>
public enum InvoiceStatus
{
    /// <summary>Draft invoice.</summary>
    Draft = 100,

    /// <summary>Open / sent invoice.</summary>
    Open = 200,

    /// <summary>Paid invoice.</summary>
    Paid = 1000
}
