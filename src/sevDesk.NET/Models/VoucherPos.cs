namespace sevDesk.NET.Models;

/// <summary>
/// Represents a position (line item) on a sevDesk voucher.
/// </summary>
public class VoucherPos
{
    /// <summary>Gets the position ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the voucher reference.</summary>
    public SevDeskObjectReference? Voucher { get; init; }

    /// <summary>Gets or sets the accounting type reference.</summary>
    public SevDeskObjectReference? AccountingType { get; init; }

    /// <summary>Gets or sets the estimated accounting type reference.</summary>
    public SevDeskObjectReference? EstimatedAccountingType { get; init; }

    /// <summary>Gets or sets the net amount.</summary>
    public decimal? Net { get; init; }

    /// <summary>Gets or sets the tax rate.</summary>
    public decimal? TaxRate { get; init; }

    /// <summary>Gets or sets whether this is an asset.</summary>
    public bool? IsAsset { get; init; }

    /// <summary>Gets or sets the sum net.</summary>
    public decimal? SumNet { get; init; }

    /// <summary>Gets or sets the sum gross.</summary>
    public decimal? SumGross { get; init; }

    /// <summary>Gets or sets the sum tax.</summary>
    public decimal? SumTax { get; init; }

    /// <summary>Gets or sets the comment.</summary>
    public string? Comment { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }
}
