namespace sevDesk.NET.Models;

/// <summary>
/// Represents a country from the sevDesk country catalogue. Country references on
/// contact addresses and invoices resolve against this catalogue.
/// </summary>
public class StaticCountry
{
    /// <summary>Gets the country ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the ISO 3166-1 alpha-2 country code (lower case, e.g. <c>de</c>).</summary>
    public string? Code { get; init; }

    /// <summary>Gets or sets the localized country name.</summary>
    public string? Name { get; init; }

    /// <summary>Gets or sets the English country name.</summary>
    public string? NameEn { get; init; }

    /// <summary>Gets or sets the translation code.</summary>
    public string? TranslationCode { get; init; }

    /// <summary>Gets or sets the locale the <see cref="Name"/> is returned in.</summary>
    public string? Locale { get; init; }

    /// <summary>Gets or sets the sort priority within the catalogue.</summary>
    public int? Priority { get; init; }
}
