using sevDesk.NET.Models.Enums;

namespace sevDesk.NET.Models;

/// <summary>
/// Represents a sevDesk order (offer, order confirmation, or delivery note).
/// </summary>
public class Order
{
    /// <summary>Gets the order ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the order number.</summary>
    public string? OrderNumber { get; init; }

    /// <summary>Gets or sets the contact reference.</summary>
    public SevDeskObjectReference? Contact { get; init; }

    /// <summary>Gets or sets the order date.</summary>
    public DateTime? OrderDate { get; init; }

    /// <summary>Gets or sets the order status.</summary>
    public OrderStatus? Status { get; init; }

    /// <summary>Gets or sets the order type.</summary>
    public OrderType? OrderType { get; init; }

    /// <summary>Gets or sets the header text.</summary>
    public string? Header { get; init; }

    /// <summary>Gets or sets the head text (above positions).</summary>
    public string? HeadText { get; init; }

    /// <summary>Gets or sets the foot text (below positions).</summary>
    public string? FootText { get; init; }

    /// <summary>Gets or sets the contact person reference.</summary>
    public SevDeskObjectReference? ContactPerson { get; init; }

    /// <summary>Gets or sets the address text.</summary>
    public string? Address { get; init; }

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

    /// <summary>Gets or sets the tax rate.</summary>
    public decimal? TaxRate { get; init; }

    /// <summary>Gets or sets the tax text.</summary>
    public string? TaxText { get; init; }

    /// <summary>Gets or sets the send date.</summary>
    public DateTime? SendDate { get; init; }

    /// <summary>Gets or sets the delivery date.</summary>
    public DateTime? DeliveryDate { get; init; }

    /// <summary>Gets or sets whether this is a small settlement.</summary>
    public bool? SmallSettlement { get; init; }

    /// <summary>Gets or sets the tax set reference.</summary>
    public SevDeskObjectReference? TaxSet { get; init; }

    /// <summary>Gets or sets the origin reference.</summary>
    public SevDeskObjectReference? Origin { get; init; }

    /// <summary>Gets or sets the customer internal note.</summary>
    public string? CustomerInternalNote { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }
}
