using sevDesk.NET.Models.Enums;

namespace sevDesk.NET.Models;

/// <summary>
/// Represents a sevDesk voucher (expense or revenue).
/// </summary>
public class Voucher
{
    /// <summary>Gets the voucher ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the voucher date.</summary>
    public DateTime? VoucherDate { get; init; }

    /// <summary>Gets or sets the supplier reference.</summary>
    public SevDeskObjectReference? Supplier { get; init; }

    /// <summary>Gets or sets the voucher status.</summary>
    public VoucherStatus? Status { get; init; }

    /// <summary>Gets or sets the voucher type.</summary>
    public VoucherType? VoucherType { get; init; }

    /// <summary>Gets or sets the description.</summary>
    public string? Description { get; init; }

    /// <summary>Gets or sets the pay date.</summary>
    public DateTime? PayDate { get; init; }

    /// <summary>Gets or sets the currency (ISO 4217).</summary>
    public string? Currency { get; init; }

    /// <summary>Gets or sets the total net amount.</summary>
    public decimal? SumNet { get; init; }

    /// <summary>Gets or sets the total gross amount.</summary>
    public decimal? SumGross { get; init; }

    /// <summary>Gets or sets the total tax amount.</summary>
    public decimal? SumTax { get; init; }

    /// <summary>Gets or sets the tax type.</summary>
    public string? TaxType { get; init; }

    /// <summary>Gets or sets the credit/debit flag (C or D).</summary>
    public string? CreditDebit { get; init; }

    /// <summary>Gets or sets the document reference.</summary>
    public SevDeskObjectReference? Document { get; init; }

    /// <summary>Gets or sets the cost centre reference.</summary>
    public SevDeskObjectReference? CostCentre { get; init; }

    /// <summary>Gets or sets the paid amount.</summary>
    public decimal? PaidAmount { get; init; }

    /// <summary>Gets or sets the tax set reference.</summary>
    public SevDeskObjectReference? TaxSet { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }
}
