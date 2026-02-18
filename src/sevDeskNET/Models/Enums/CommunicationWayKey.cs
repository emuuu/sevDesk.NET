namespace sevDeskNET.Models.Enums;

/// <summary>
/// Key identifying the purpose of a communication way in sevDesk.
/// </summary>
public enum CommunicationWayKey
{
    /// <summary>Work communication way.</summary>
    Work = 1,

    /// <summary>Private communication way.</summary>
    Private = 2,

    /// <summary>Fax communication way.</summary>
    Fax = 3,

    /// <summary>Mobile communication way.</summary>
    Mobile = 4,

    /// <summary>Invoice email address.</summary>
    InvoiceEmail = 5,

    /// <summary>Autobox communication way.</summary>
    Autobox = 6,

    /// <summary>Newsletter email.</summary>
    Newsletter = 7,

    /// <summary>Empty / unset.</summary>
    Empty = 8
}
