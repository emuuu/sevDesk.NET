namespace sevDesk.NET.Internal;

/// <summary>
/// Outcome of a confirmed write to a sevDesk <c>Factory</c> endpoint: the object exists and its
/// identifier is known.
/// </summary>
/// <param name="Id">The identifier parsed out of the factory response.</param>
/// <param name="RawResponse">The raw factory response body, kept for error reporting.</param>
internal readonly record struct FactoryWriteResult(int Id, string RawResponse);
