using sevDesk.NET.Models.Enums;

namespace sevDesk.NET.Models;

/// <summary>
/// Represents a sevDesk contact (customer, supplier, etc.).
/// </summary>
public class Contact
{
    /// <summary>Gets the contact ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the customer number.</summary>
    public string? CustomerNumber { get; init; }

    /// <summary>Gets or sets the contact's first name (note: sevDesk API uses "surename").</summary>
    public string? Surename { get; init; }

    /// <summary>Gets or sets the contact's family name.</summary>
    public string? Familyname { get; init; }

    /// <summary>Gets or sets the company name.</summary>
    public string? Name { get; init; }

    /// <summary>Gets or sets the company name (second line).</summary>
    public string? Name2 { get; init; }

    /// <summary>Gets or sets the contact status.</summary>
    public ContactStatus? Status { get; init; }

    /// <summary>Gets or sets the title (e.g. "Herr", "Frau").</summary>
    public string? Title { get; init; }

    /// <summary>Gets or sets the academic title.</summary>
    public string? AcademicTitle { get; init; }

    /// <summary>Gets or sets the gender (m/f/null).</summary>
    public string? Gender { get; init; }

    /// <summary>Gets or sets the category reference.</summary>
    public SevDeskObjectReference? Category { get; init; }

    /// <summary>Gets or sets the description.</summary>
    public string? Description { get; init; }

    /// <summary>Gets or sets the VAT number.</summary>
    public string? VatNumber { get; init; }

    /// <summary>Gets or sets the bank account.</summary>
    public string? BankAccount { get; init; }

    /// <summary>Gets or sets the bank number.</summary>
    public string? BankNumber { get; init; }

    /// <summary>Gets or sets the default cashback time in days.</summary>
    public int? DefaultCashbackTime { get; init; }

    /// <summary>Gets or sets the default cashback percentage.</summary>
    public decimal? DefaultCashbackPercent { get; init; }

    /// <summary>Gets or sets the default time to pay in days.</summary>
    public int? DefaultTimeToPay { get; init; }

    /// <summary>Gets or sets the tax number.</summary>
    public string? TaxNumber { get; init; }

    /// <summary>Gets or sets the tax office.</summary>
    public string? TaxOffice { get; init; }

    /// <summary>Gets or sets whether the contact is exempt from VAT.</summary>
    public bool? ExemptVat { get; init; }

    /// <summary>Gets or sets the birthday.</summary>
    public DateTime? Birthday { get; init; }

    /// <summary>Gets or sets the default discount percentage.</summary>
    public decimal? DefaultDiscountPercentage { get; init; }

    /// <summary>Gets or sets whether this is a government agency.</summary>
    public bool? GovernmentAgency { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }
}
