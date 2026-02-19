namespace sevDesk.NET.Models;

/// <summary>
/// Represents a sevDesk unity (unit of measure).
/// </summary>
public class Unity
{
    /// <summary>Gets the unity ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the unity name.</summary>
    public string? Name { get; init; }

    /// <summary>Gets or sets the translation code.</summary>
    public string? TranslationCode { get; init; }

    /// <summary>Gets or sets the unity system.</summary>
    public string? UnitySystem { get; init; }

    /// <summary>Gets or sets the UN/ECE trade unit code.</summary>
    public string? UneceTradeUnitCode { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }
}
