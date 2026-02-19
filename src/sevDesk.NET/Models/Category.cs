namespace sevDesk.NET.Models;

/// <summary>
/// Represents a sevDesk category.
/// </summary>
public class Category
{
    /// <summary>Gets the category ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the category name.</summary>
    public string? Name { get; init; }

    /// <summary>Gets or sets the object type this category applies to.</summary>
    public string? ObjectType { get; init; }

    /// <summary>Gets or sets the priority.</summary>
    public int? Priority { get; init; }

    /// <summary>Gets or sets the color hex code.</summary>
    public string? Color { get; init; }

    /// <summary>Gets or sets a post-it style note.</summary>
    public string? PostingAccount { get; init; }

    /// <summary>Gets or sets the translation code.</summary>
    public string? TranslationCode { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }
}
