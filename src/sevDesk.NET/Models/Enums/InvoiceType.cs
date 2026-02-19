namespace sevDesk.NET.Models.Enums;

/// <summary>
/// Type of an invoice in sevDesk.
/// </summary>
public enum InvoiceType
{
    /// <summary>Normal invoice (Rechnung).</summary>
    RE,

    /// <summary>Recurring invoice (Wiederkehrende Rechnung).</summary>
    WKR,

    /// <summary>Cancellation invoice (Stornorechnung).</summary>
    SR,

    /// <summary>Partial invoice (Teilrechnung).</summary>
    TR,

    /// <summary>Final invoice (Endrechnung).</summary>
    ER,

    /// <summary>Advance invoice (Abschlagsrechnung).</summary>
    AR
}
