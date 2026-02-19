namespace sevDesk.NET.Models;

/// <summary>
/// Represents a sevDesk document (uploaded file).
/// </summary>
public class Document
{
    /// <summary>Gets the document ID.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets the filename.</summary>
    public string? Filename { get; init; }

    /// <summary>Gets or sets the file extension.</summary>
    public string? Extension { get; init; }

    /// <summary>Gets or sets the file size in bytes.</summary>
    public long? Size { get; init; }

    /// <summary>Gets or sets the MIME type.</summary>
    public string? MimeType { get; init; }

    /// <summary>Gets or sets the object reference this document belongs to.</summary>
    public SevDeskObjectReference? Object { get; init; }

    /// <summary>Gets or sets the folder reference.</summary>
    public SevDeskObjectReference? Folder { get; init; }

    /// <summary>Gets or sets the status.</summary>
    public int? Status { get; init; }

    /// <summary>Gets the creation date.</summary>
    public DateTime? Create { get; init; }

    /// <summary>Gets the last update date.</summary>
    public DateTime? Update { get; init; }
}
