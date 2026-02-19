namespace sevDesk.NET.Models.Enums;

/// <summary>
/// Status of a voucher in sevDesk.
/// </summary>
public enum VoucherStatus
{
    /// <summary>Draft voucher.</summary>
    Draft = 50,

    /// <summary>Unpaid voucher.</summary>
    Unpaid = 100,

    /// <summary>Paid voucher.</summary>
    Paid = 1000
}
