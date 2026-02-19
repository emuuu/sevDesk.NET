namespace sevDesk.NET.Models.Enums;

/// <summary>
/// Status of an order in sevDesk.
/// </summary>
public enum OrderStatus
{
    /// <summary>Draft order.</summary>
    Draft = 100,

    /// <summary>Delivered order.</summary>
    Delivered = 200,

    /// <summary>Rejected order.</summary>
    Rejected = 300,

    /// <summary>Accepted order.</summary>
    Accepted = 500,

    /// <summary>Calculated / completed order.</summary>
    Calculated = 1000
}
